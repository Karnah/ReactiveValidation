using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ReactiveValidation.Exceptions;
using ReactiveValidation.Helpers;
using ReactiveValidation.Helpers.Nito.AsyncEx;
using ReactiveValidation.Internal;
using ReactiveValidation.Validators;

namespace ReactiveValidation
{
    /// <inheritdoc cref="IObjectValidator" />
    internal class ObjectValidator<TObject> : BaseNotifyPropertyChanged, IObjectValidator
        where TObject : IValidatableObject
    {
        private readonly IReadOnlyDictionary<string, IStringSource> _displayNamesSources;

        private readonly ObjectObserver<TObject> _observer;
        private readonly IDictionary<string, ValidatableProperty<TObject>> _validatableProperties;

        private readonly object _lock = new();
        private readonly AsyncManualResetEvent _asyncValidationWaiter = new(true);


        private bool _isValid;
        private bool _hasWarnings;
        private IReadOnlyList<ValidationMessage> _validationMessages;

        private bool _isDisposed;

        /// <remarks>
        /// This is very important thing. <see cref="LanguageManager.CultureChanged" /> uses a weak reference to delegates.
        /// If there is no reference to delegate, target will be collected by GC.
        /// ObjectValidator keep this reference until it will be collected.
        /// After this WeakReference will be collected too.
        /// </remarks>
        /// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly EventHandler<CultureChangedEventArgs> _cultureChangedEventHandler;

        /// <summary>
        /// Create new instance of object validator.
        /// </summary>
        /// <param name="instance">Instance of validatable object.</param>
        /// <param name="ruleBuilders">List of rules builders.</param>
        public ObjectValidator(TObject instance, IReadOnlyList<IRuleBuilder<TObject>> ruleBuilders)
        {
            Instance = instance;

            _displayNamesSources = GetDisplayNames();
            _validatableProperties = GetValidatableProperties(ruleBuilders);

            _observer = new ObjectObserver<TObject>(Instance, GetPropertySettings(ruleBuilders));
            _observer.PropertyChanged += OnPropertyChanged;

            if (ValidationOptions.LanguageManager.TrackCultureChanged)
            {
                _cultureChangedEventHandler = OnCultureChanged;
                ValidationOptions.LanguageManager.CultureChanged += _cultureChangedEventHandler;
            }
        }


        /// <summary>
        /// Instance of validatable object.
        /// </summary>
        public TObject Instance { get; }

        #region IObjectValidator

        /// <inheritdoc />
        public bool IsValid
        {
            get => _isValid;
            private set => SetAndRaiseIfChanged(ref _isValid, value);
        }

        /// <inheritdoc />
        public bool HasWarnings
        {
            get => _hasWarnings;
            private set => SetAndRaiseIfChanged(ref _hasWarnings, value);
        }

        /// <inheritdoc />
        public bool IsAsyncValidating
        {
            get => !_asyncValidationWaiter.IsSet;
            private set
            {
                var isValueChanged = _asyncValidationWaiter.IsSet == value;
                if (!isValueChanged)
                    return;
                
                if (value)
                    _asyncValidationWaiter.Reset();
                else
                    _asyncValidationWaiter.Set();
                    
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public IReadOnlyList<ValidationMessage> ValidationMessages
        {
            get => _validationMessages;
            private set => SetAndRaiseIfChanged(ref _validationMessages, value);
        }


        /// <inheritdoc />
        public IReadOnlyList<ValidationMessage> GetMessages(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return null;

            lock (_lock)
            {
                if (!_validatableProperties.ContainsKey(propertyName))
                    return null;

                return _validatableProperties[propertyName].ValidationMessages;
            }
        }

        /// <inheritdoc />
        public void Revalidate()
        {
            RevalidateInternal();
        }

        /// <inheritdoc />
        public Task WaitValidatingAsync(CancellationToken cancellationToken = default)
        {
            return _asyncValidationWaiter.WaitAsync(cancellationToken);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose object validator.
        /// </summary>
        private void Dispose(bool isDisposing)
        {
            if (!isDisposing || _isDisposed)
                return;

            lock (_lock)
            {
                if (_isDisposed)
                    return;

                _observer.Dispose();

                if (ValidationOptions.LanguageManager.TrackCultureChanged)
                    ValidationOptions.LanguageManager.CultureChanged -= _cultureChangedEventHandler;

                // Cancel all async tasks.
                // We don't need to wait them, because they won't affect on anything.
                var asyncTokenSources = _validatableProperties
                    .Values
                    .SelectMany(p => p.AsyncValidatorCancellationTokenSources.Values);
                foreach (var tokenSource in asyncTokenSources)
                {
                    tokenSource?.Cancel();
                }
                
                ValidationMessages = Array.Empty<ValidationMessage>();
                IsValid = true;
                HasWarnings = false;

                foreach (var validatableProperty in _validatableProperties.Keys)
                {
                    Instance.OnPropertyMessagesChanged(validatableProperty);
                }

                _isDisposed = true;
            }
        }

        #endregion

        /// <summary>
        /// Handle property changed event.
        /// </summary>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            RevalidateInternal(args.PropertyName);
        }

        /// <summary>
        /// Revalidate properties.
        /// </summary>
        /// <param name="propertyName">
        /// Name of changed property.
        /// <see langword="null" /> if all properties should be revalidated.
        /// </param>
        private void RevalidateInternal(string propertyName = null)
        {
            lock (_lock)
            {
                var aggregatedValidationContext = new AggregatedValidationContext<TObject>(Instance, _displayNamesSources);
                var changedProperties = new List<string>();
                var propertiesAsyncValidators = new List<(string, List<IPropertyValidator<TObject>>)>();

                foreach (var validatableProperty in _validatableProperties)
                {
                    var info = validatableProperty.Value;
                    bool isTarget = string.IsNullOrEmpty(propertyName) || info.PropertyName == propertyName;
                    bool isMessagesChanged = false;
                    bool isPropertyValid = true;

                    // First we check all sync validators.
                    foreach (var propertyValidator in info.SyncValidators)
                    {
                        if (!isTarget && !propertyValidator.RelatedProperties.Contains(propertyName))
                            continue;

                        var contextProvider = aggregatedValidationContext.CreateContextFactory(info.PropertyName);
                        var messages = ValidatePropertyValidator(propertyValidator, contextProvider);
                        isPropertyValid &= messages.IsValid();
                        if (messages.SequenceEqual(info.ValidatorsValidationMessages[propertyValidator]))
                            continue;

                        info.ValidatorsValidationMessages[propertyValidator] = messages;
                        isMessagesChanged = true;
                    }

                    // Then we should reset messages of async validators.
                    // They can be restored after async check.
                    var asyncValidators = new List<IPropertyValidator<TObject>>();
                    foreach (var propertyValidator in info.AsyncValidators)
                    {
                        if (!isTarget && !propertyValidator.RelatedProperties.Contains(propertyName))
                            continue;
                        
                        var messages = info.ValidatorsValidationMessages[propertyValidator];
                        if (messages.Any())
                        {
                            info.ValidatorsValidationMessages[propertyValidator] = Array.Empty<ValidationMessage>();
                            isMessagesChanged = true;
                        }

                        // Cancel previous execution (if it is running).
                        info.AsyncValidatorCancellationTokenSources[propertyValidator]?.Cancel();
                        
                        // We should use async validators only if all previous checks are success. 
                        if (isPropertyValid)
                        {
                            // Create new token source for future execution.
                            info.AsyncValidatorCancellationTokenSources[propertyValidator] =
                                new CancellationTokenSource();
                            
                            asyncValidators.Add(propertyValidator);
                        }
                    }
                    
                    if (isMessagesChanged)
                        changedProperties.Add(info.PropertyName);

                    if (asyncValidators.Any())
                        propertiesAsyncValidators.Add((info.PropertyName, asyncValidators));
                }

                NotifyChangedProperties(changedProperties);
                ValidateInternalAsync(aggregatedValidationContext, propertiesAsyncValidators);
            }
        }

        /// <summary>
        /// Validate using async validators.
        /// </summary>
        private async void ValidateInternalAsync(
            AggregatedValidationContext<TObject> aggregatedValidationContext,
            IReadOnlyList<(string, List<IPropertyValidator<TObject>>)> propertiesAsyncValidators)
        {
            if (propertiesAsyncValidators.Count == 0)
                return;

            IsAsyncValidating = true;

            try
            {
                var tasks = new List<Task>();

                foreach (var (propertyName, propertyValidators) in propertiesAsyncValidators)
                {
                    tasks.Add(ValidatePropertyAsync(aggregatedValidationContext, propertyName, propertyValidators));
                }

                await Task.WhenAll(tasks)
                    .ConfigureAwait(false);
            }
            finally
            {
                lock (_lock)
                {
                    IsAsyncValidating = _validatableProperties
                        .Values
                        .SelectMany(vp => vp.AsyncValidatorCancellationTokenSources.Values)
                        .Any(s => s?.IsCancellationRequested == false);
                }
            }
        }

        /// <summary>
        /// Validate property with async validators.
        /// </summary>
        private async Task ValidatePropertyAsync(
            AggregatedValidationContext<TObject> aggregatedValidationContext,
            string propertyName,
            IReadOnlyList<IPropertyValidator<TObject>> propertyValidators)
        {
            var info = _validatableProperties[propertyName];
            foreach (var propertyValidator in propertyValidators)
            {
                // Skip this validator because it should be revalidated with new property value.
                var tokenSource = info.AsyncValidatorCancellationTokenSources[propertyValidator];
                if (tokenSource.IsCancellationRequested)
                    continue;

                var contextProvider = aggregatedValidationContext.CreateContextFactory(info.PropertyName);
                var messages = await ValidatePropertyValidatorAsync(propertyValidator, contextProvider, tokenSource.Token)
                    .ConfigureAwait(false);

                if (tokenSource.IsCancellationRequested)
                    return;
                
                lock (_lock)
                {
                    if (tokenSource.IsCancellationRequested)
                        return;

                    tokenSource.Cancel();
                    
                    // Inside RevalidateInternal we have reset messages.
                    // So we need only check if there are something new.
                    if (messages.Any())
                    {
                        info.ValidatorsValidationMessages[propertyValidator] = messages;
                        NotifyChangedProperties(new[] { propertyName });
                        
                        // For async validators using rule "First failure".
                        return;
                    }
                }
            }
        }
        
        /// <summary>
        /// Validate property by specified validator.
        /// </summary>
        /// <param name="propertyValidator">Property validator.</param>
        /// <param name="contextFactory">Context factory.</param>
        private static IReadOnlyList<ValidationMessage> ValidatePropertyValidator(
            IPropertyValidator<TObject> propertyValidator,
            ValidationContextFactory<TObject> contextFactory)
        {
            try
            {
                return propertyValidator.ValidateProperty(contextFactory);
            }
            catch (Exception e)
            {
                return new[]
                {
                    new ValidationMessage(new ExceptionSource(e)),
                };
            }
        }

        /// <summary>
        /// Validate property by specified async validator.
        /// </summary>
        /// <param name="propertyValidator">Property validator.</param>
        /// <param name="contextFactory">Context factory.</param>
        /// <param name="cancellationToken">Token for cancelling validation.</param>
        private static async Task<IReadOnlyList<ValidationMessage>> ValidatePropertyValidatorAsync(
            IPropertyValidator<TObject> propertyValidator,
            ValidationContextFactory<TObject> contextFactory,
            CancellationToken cancellationToken)
        {
            try
            {
                return await propertyValidator.ValidatePropertyAsync(contextFactory, cancellationToken);
            }
            // Ignore only if exception was thrown by cancellationToken, not because of user code.
            catch (TaskCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return Array.Empty<ValidationMessage>();
            } 
            catch (Exception e)
            {
                return new[]
                {
                    new ValidationMessage(new ExceptionSource(e)),
                };
            }
        }

        /// <summary>
        /// Update state and notify if properties changed.
        /// </summary>
        /// <param name="changedProperties">List of changed properties.</param>
        private void NotifyChangedProperties(IReadOnlyList<string> changedProperties)
        {
            if (changedProperties.Count == 0)
                return;
            
            ValidationMessages = _validatableProperties
                .Values
                .SelectMany(vp => vp.ValidationMessages)
                .ToList();
            IsValid = ValidationMessages.IsValid();
            HasWarnings = ValidationMessages.HasWarnings();

            foreach (var changedProperty in changedProperties)
            {
                Instance.OnPropertyMessagesChanged(changedProperty);
            }
        }

        /// <summary>
        /// Handle application culture changed event.
        /// </summary>
        private void OnCultureChanged(object sender, CultureChangedEventArgs e)
        {
            foreach (var validationMessage in ValidationMessages)
            {
                validationMessage.UpdateMessage();
            }
        }

        /// <summary>
        /// Create list of display name for all properties of instance.
        /// </summary>
        private static IReadOnlyDictionary<string, IStringSource> GetDisplayNames()
        {
            const BindingFlags bindingAttributes = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var displayNamesSources = new Dictionary<string, IStringSource>();
            var properties = typeof(TObject).GetProperties(bindingAttributes);
            foreach (var property in properties)
            {
                displayNamesSources.Add(property.Name, ValidationOptions.DisplayNameResolver.GetPropertyNameSource(typeof(TObject), property, null));
            }

            return displayNamesSources;
        }

        /// <summary>
        /// Get information of validatable properties.
        /// </summary>
        /// <param name="ruleBuilders">Rule builders.</param>
        private IDictionary<string, ValidatableProperty<TObject>> GetValidatableProperties(IReadOnlyList<IRuleBuilder<TObject>> ruleBuilders)
        {
            var propertyValidators = new Dictionary<string, List<IPropertyValidator<TObject>>>();
            foreach (var ruleBuilder in ruleBuilders)
            {
                var validators = ruleBuilder.GetValidators();
                foreach (var validatableProperty in ruleBuilder.ValidatableProperties)
                {
                    if (!propertyValidators.ContainsKey(validatableProperty))
                        propertyValidators[validatableProperty] = new List<IPropertyValidator<TObject>>();

                    propertyValidators[validatableProperty].AddRange(validators);
                }
            }

            return propertyValidators.ToDictionary(
                pv => pv.Key,
                pv => new ValidatableProperty<TObject>(pv.Key, _displayNamesSources[pv.Key], pv.Value));
        }

        /// <summary>
        /// Get information of validatable properties settings.
        /// </summary>
        /// <param name="ruleBuilders">Rule builders.</param>
        private static Dictionary<string, ObservingPropertySettings> GetPropertySettings(IReadOnlyList<IRuleBuilder<TObject>> ruleBuilders)
        {
            var propertySettings = new Dictionary<string, ObservingPropertySettings>();
            foreach (var ruleBuilder in ruleBuilders)
            {
                var settings = ruleBuilder.ObservingPropertiesSettings;
                if (settings.IsDefaultSettings)
                    continue;

                foreach (var validatableProperty in ruleBuilder.ValidatableProperties)
                {
                    if (!propertySettings.TryGetValue(validatableProperty, out var previousSettings))
                    {
                        previousSettings = new ObservingPropertySettings();
                        propertySettings[validatableProperty] = previousSettings;
                    }

                    if (settings.PropertyValueFactoryMethod != null && previousSettings.PropertyValueFactoryMethod != null)
                        throw new MethodAlreadyCalledException($"Validator factory method used twice for {typeof(TObject)}.{validatableProperty}");

                    if (settings.CollectionItemFactoryMethod != null && previousSettings.CollectionItemFactoryMethod != null)
                        throw new MethodAlreadyCalledException($"Validator item factory method used twice for {typeof(TObject)}.{validatableProperty}");

                    previousSettings.TrackValueChanged |= settings.TrackValueChanged;
                    previousSettings.TrackValueErrorsChanged |= settings.TrackValueErrorsChanged;
                    previousSettings.TrackCollectionChanged |= settings.TrackCollectionChanged;
                    previousSettings.TrackCollectionItemChanged |= settings.TrackCollectionItemChanged;
                    previousSettings.TrackCollectionItemErrorsChanged |= settings.TrackCollectionItemErrorsChanged;
                    previousSettings.PropertyValueFactoryMethod = previousSettings.PropertyValueFactoryMethod ?? settings.PropertyValueFactoryMethod;
                    previousSettings.CollectionItemFactoryMethod = previousSettings.CollectionItemFactoryMethod ?? settings.CollectionItemFactoryMethod;
                }
            }

            return propertySettings;
        }
    }
}
