using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using ReactiveValidation.Adapters;
using ReactiveValidation.Helpers;
using ReactiveValidation.Internal;

namespace ReactiveValidation
{
    /// <inheritdoc cref="IObjectValidator" />
    internal class ObjectValidator<TObject> : BaseNotifyPropertyChanged, IObjectValidator
        where TObject : IValidatableObject
    {
        private readonly List<IPropertiesAdapter> _adapters;
        private readonly IDictionary<string, Dictionary<IPropertiesAdapter, IReadOnlyList<ValidationMessage>>> _validationResults;
        private readonly IDictionary<string, IStringSource> _displayNamesSources;

        private bool _isValid;
        private bool _hasWarnings;
        private IReadOnlyList<ValidationMessage> _validationMessages;

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
        public ObjectValidator(TObject instance)
        {
            Instance = instance;
            Instance.PropertyChanged += OnInstancePropertyChanged;

            _adapters = new List<IPropertiesAdapter>();
            _validationResults = new SortedDictionary<string, Dictionary<IPropertiesAdapter, IReadOnlyList<ValidationMessage>>>();
            _displayNamesSources = GetDisplayNames();

            if (ValidationOptions.LanguageManager.TrackCultureChanged)
            {
                _cultureChangedEventHandler = OnCultureChanged;
                ValidationOptions.LanguageManager.CultureChanged += _cultureChangedEventHandler;
            }
        }

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

            lock (_validationResults)
            {
                if (!_validationResults.ContainsKey(propertyName))
                    return null;

                return _validationResults[propertyName]
                    .SelectMany(vm => vm.Value)
                    .ToList();
            }
        }

        /// <inheritdoc />
        public void Revalidate()
        {
            foreach (var validator in _adapters)
            {
                validator.Revalidate();
            }
        }

        #endregion

        /// <summary>
        /// Instance of validatable object.
        /// </summary>
        public TObject Instance { get; }


        /// <summary>
        /// Register new property adapter.
        /// </summary>
        /// <param name="adapter">Properties adapter.</param>
        /// <param name="propertiesNames">Names of validatable properties.</param>
        public void RegisterAdapter(IPropertiesAdapter adapter, params string[] propertiesNames)
        {
            _adapters.Add(adapter);

            lock (_validationResults)
            {
                foreach (var propertyName in propertiesNames)
                {
                    if (_validationResults.ContainsKey(propertyName) == false)
                        _validationResults.Add(propertyName, new Dictionary<IPropertiesAdapter, IReadOnlyList<ValidationMessage>>());

                    _validationResults[propertyName].Add(adapter, new ValidationMessage[0]);
                }
            }
        }

        /// <summary>
        /// Set validation messages for specified property.
        /// </summary>
        /// <param name="propertyName">Name of validated property.</param>
        /// <param name="adapter">Adapter which validated property.</param>
        /// <param name="messages">List of messages.</param>
        public void SetValidationMessages(string propertyName, IPropertiesAdapter adapter, IReadOnlyList<ValidationMessage> messages)
        {
            lock (_validationResults)
            {
                _validationResults[propertyName][adapter] = messages;

                var aggregatedMessages = _validationResults
                    .SelectMany(vm => vm.Value)
                    .SelectMany(vm => vm.Value)
                    .ToList();

                IsValid = !aggregatedMessages.Any(vm => vm?.ValidationMessageType == ValidationMessageType.Error ||
                                                        vm?.ValidationMessageType == ValidationMessageType.SimpleError);

                HasWarnings = aggregatedMessages.Any(vm => vm?.ValidationMessageType == ValidationMessageType.Warning ||
                                                           vm?.ValidationMessageType == ValidationMessageType.SimpleWarning);

                ValidationMessages = aggregatedMessages;
            }

            Instance.OnPropertyMessagesChanged(propertyName);
        }

        /// <summary>
        /// Create new validation context.
        /// </summary>
        /// <typeparam name="TProp">Type of validatable property.</typeparam>
        /// <param name="propertyName">Name of validatable property.</param>
        public ValidationContext<TObject, TProp> GetValidationContext<TProp>(string propertyName)
        {
            var propertyValue = ReactiveValidationHelper.GetPropertyValue<TProp>(Instance, propertyName);

            return new ValidationContext<TObject, TProp>(Instance, propertyName, _displayNamesSources[propertyName], propertyValue);
        }

        /// <summary>
        /// Get property value by its name for current instance.
        /// </summary>
        /// <typeparam name="TProp">Type of property.</typeparam>
        /// <param name="propertyName">Name of property.</param>
        /// <returns>Value of property.</returns>
        public TProp GetPropertyValue<TProp>(string propertyName)
        {
            var propertyValue = ReactiveValidationHelper.GetPropertyValue<TProp>(Instance, propertyName);
            return propertyValue;
        }


        /// <summary>
        /// Handle instance <see cref="INotifyPropertyChanged.PropertyChanged" /> event.
        /// </summary>
        private void OnInstancePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var propertyName = args.PropertyName;
            foreach (var adapter in _adapters)
            {
                adapter.Revalidate(propertyName);
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
        private static IDictionary<string, IStringSource> GetDisplayNames()
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
    }
}
