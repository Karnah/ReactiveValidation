using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using ReactiveValidation.Adapters;
using ReactiveValidation.Helpers;

namespace ReactiveValidation
{
    internal class ObjectValidator<TObject> : IObjectValidator, INotifyPropertyChanged
        where TObject : IValidatableObject
    {
        private readonly TObject _instance;
        private readonly List<IPropertiesAdapter> _adapters;
        private readonly IDictionary<string, Dictionary<IPropertiesAdapter, IEnumerable<ValidationMessage>>> _validationResults;
        private readonly Dictionary<string, IStringSource> _displayNamesSources;

        private bool _isValid;
        private bool _hasWarnings;
        private IEnumerable<ValidationMessage> _validationMessages;

        public ObjectValidator(TObject instance)
        {
            _instance = instance;
            _instance.PropertyChanged += OnPropertyChanged;

            _adapters = new List<IPropertiesAdapter>();
            _validationResults = new SortedDictionary<string, Dictionary<IPropertiesAdapter, IEnumerable<ValidationMessage>>>();
            _displayNamesSources = new Dictionary<string, IStringSource>();

            FillDisplayNames();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var propertyName = args.PropertyName;

            foreach (var adapter in _adapters) {
                adapter.Revalidate(propertyName);
            }
        }

        private void FillDisplayNames()
        {
            const BindingFlags bindingAttributes = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var properties = typeof(TObject).GetProperties(bindingAttributes);
            foreach (var property in properties) {
                _displayNamesSources.Add(property.Name, ValidationOptions.DisplayNameResolver.Invoke(typeof(TObject), property, null));
            }
        }


        public bool IsValid {
            get => _isValid;
            private set {
                if (_isValid == value)
                    return;

                _isValid = value;
                OnPropertyChanged();
            }
        }

        public bool HasWarnings {
            get => _hasWarnings;
            private set {
                if (_hasWarnings == value)
                    return;

                _hasWarnings = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<ValidationMessage> ValidationMessages {
            get => _validationMessages;
            private set {
                if (Equals(_validationMessages, value))
                    return;

                _validationMessages = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;


        public IEnumerable<ValidationMessage> GetMessages(string propertyName)
        {
            lock (_validationResults) {
                if (_validationResults.ContainsKey(propertyName) == false)
                    return null;

                return _validationResults[propertyName]
                    .SelectMany(vm => vm.Value)
                    .OrderBy(vm => vm.ValidationMessageType)
                    .ThenBy(vm => vm.Message);
            }
        }

        public void Revalidate()
        {
            foreach (var validator in _adapters) {
                validator.Revalidate();
            }
        }


        internal void RegisterAdapter(IPropertiesAdapter adapter, params string[] propertiesNames)
        {
            _adapters.Add(adapter);

            lock (_validationResults) {
                foreach (var propertyName in propertiesNames) {
                    if (_validationResults.ContainsKey(propertyName) == false)
                        _validationResults.Add(propertyName,
                            new Dictionary<IPropertiesAdapter, IEnumerable<ValidationMessage>>());

                    _validationResults[propertyName].Add(adapter, new ValidationMessage[0]);
                }
            }
        }

        internal void SetValidationMessages(string propertyName, IPropertiesAdapter adapter, IEnumerable<ValidationMessage> messages)
        {
            lock (_validationResults) {
                _validationResults[propertyName][adapter] = messages;


                var aggregatedMessages = _validationResults.SelectMany(vm => vm.Value).SelectMany(vm => vm.Value).ToList();

                IsValid =  aggregatedMessages.Any(vm => vm?.ValidationMessageType == ValidationMessageType.Error ||
                                                        vm?.ValidationMessageType == ValidationMessageType.SimpleError) == false;

                HasWarnings = aggregatedMessages.Any(vm => vm?.ValidationMessageType == ValidationMessageType.Warning ||
                                                           vm?.ValidationMessageType == ValidationMessageType.SimpleWarning);

                ValidationMessages = aggregatedMessages;
            }

            _instance.OnPropertyMessagesChanged(propertyName);
        }


        internal ValidationContext<TObject, TProp> GetValidationContext<TProp>(string propertyName)
        {
            var propertyValue = ReactiveValidationHelper.GetPropertyValue<TProp>(_instance, propertyName);

            return new ValidationContext<TObject, TProp>(_instance, propertyName, _displayNamesSources[propertyName], propertyValue);
        }

        internal TProp GetPropertyValue<TProp>(string propertyName)
        {
            var propertyValue = ReactiveValidationHelper.GetPropertyValue<TProp>(_instance, propertyName);
            return propertyValue;
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
