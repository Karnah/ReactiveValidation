using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using ReactiveValidation.Exceptions;
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

        private readonly object _lock = new object();

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

                ValidationMessages = null;
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

                foreach (var validatableProperty in _validatableProperties)
                {
                    var info = validatableProperty.Value;
                    bool isTarget = string.IsNullOrEmpty(propertyName) || info.PropertyName == propertyName;
                    bool isErrorsChanged = false;

                    foreach (var propertyValidator in info.Validators)
                    {
                        if (!isTarget && !propertyValidator.RelatedProperties.Contains(propertyName))
                            continue;

                        var contextProvider = aggregatedValidationContext.CreateContextFactory(info.PropertyName);
                        var messages = ValidateProperty(propertyValidator, contextProvider);
                        if (messages.SequenceEqual(info.ValidatorsValidationMessages[propertyValidator]))
                            continue;

                        info.ValidatorsValidationMessages[propertyValidator] = messages;
                        isErrorsChanged = true;
                    }

                    if (isErrorsChanged)
                        changedProperties.Add(info.PropertyName);
                }

                if (changedProperties.Count == 0)
                    return;

                ValidationMessages = _validatableProperties
                    .Values
                    .SelectMany(vp => vp.ValidationMessages)
                    .ToList();

                IsValid = !ValidationMessages.Any(vm => vm?.ValidationMessageType == ValidationMessageType.Error ||
                                                        vm?.ValidationMessageType == ValidationMessageType.SimpleError);

                HasWarnings = ValidationMessages.Any(vm => vm?.ValidationMessageType == ValidationMessageType.Warning ||
                                                           vm?.ValidationMessageType == ValidationMessageType.SimpleWarning);

                foreach (var changedProperty in changedProperties)
                {
                    Instance.OnPropertyMessagesChanged(changedProperty);
                }
            }
        }

        /// <summary>
        /// Validate property by specified validator.
        /// </summary>
        /// <param name="propertyValidator">Property validator.</param>
        /// <param name="contextFactory">Context factory.</param>
        private static IReadOnlyList<ValidationMessage> ValidateProperty(
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
