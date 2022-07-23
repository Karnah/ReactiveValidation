using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using ReactiveValidation.Helpers;

namespace ReactiveValidation.ObjectObserver
{
    /// <summary>
    /// Class which track changing properties and raise events for <see cref="ObjectValidator{TObject}" />.
    /// </summary>
    /// <typeparam name="TObject">Type of observable object.</typeparam>
    internal class ObjectObserver<TObject> : IDisposable
        where TObject : INotifyPropertyChanged
    {
        private readonly TObject _instance;
        private readonly Dictionary<string, ObservingProperty> _observingProperties;

        /// <summary>
        /// Create new object observer.
        /// </summary>
        /// <param name="instance">Instance of observing object.</param>
        /// <param name="settings">Settings of properties.</param>
        public ObjectObserver(TObject instance, Dictionary<string, ObservingPropertySettings> settings)
        {
            _instance = instance;
            _instance.PropertyChanged += OnInstancePropertyChanged;

            _observingProperties = settings.ToDictionary(
                s => s.Key,
                s => CreateObservingProperty(s.Key, s.Value));

            foreach (var observingProperty in _observingProperties)
            {
                SubscribePropertyValue(observingProperty.Value);
            }
        }


        /// <summary>
        /// Event of changing property of observable object.
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs>? PropertyChanged;


        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Create and setup <see cref="ObservingProperty" />.
        /// </summary>
        private ObservingProperty CreateObservingProperty(string propertyName, ObservingPropertySettings settings)
        {
            var observingProperty = new ObservingProperty(propertyName, settings);
            
            if (settings.TrackValueChanged || settings.TrackCollectionItemChanged)
                observingProperty.ValueChangedAction = OnPropertyInternalValueChanged;
            
            if (settings.TrackValueErrorsChanged || settings.TrackCollectionItemErrorsChanged)
                observingProperty.ErrorsChangedAction = OnPropertyInternalValueChanged;

            if (settings.TrackCollectionChanged)
                observingProperty.CollectionChangedAction = (_, args) => OnCollectionChanged(propertyName, args);

            // Raise event when some of internal property of value changed.
            void OnPropertyInternalValueChanged(object? sender, EventArgs args)
            {
                OnPropertyChanged(propertyName);
            }

            return observingProperty;
        }
        
        /// <summary>
        /// Unsubscribe from previous value of property and subscribe to new.
        /// </summary>
        private void ResubscribePropertyValue(ObservingProperty observingProperty)
        {
            UnsubscribePropertyValue(observingProperty);
            SubscribePropertyValue(observingProperty);
        }
        
        /// <summary>
        /// Subscribe on new value of property.
        /// </summary>
        /// <param name="observingProperty">Info of observing property.</param>
        private void SubscribePropertyValue(ObservingProperty observingProperty)
        {
            var propertyName = observingProperty.PropertyName;
            var propertyValue = ReactiveValidationHelper.GetPropertyValue<object?>(_instance, propertyName);
            if (propertyValue == null)
                return;

            var settings = observingProperty.Settings;
            observingProperty.PreviousValue = propertyValue;

            if (settings.TrackValueChanged)
            {
                var notifyPropertyChanged = (INotifyPropertyChanged) propertyValue;
                notifyPropertyChanged.PropertyChanged += observingProperty.ValueChangedAction;
            }

            if (settings.TrackValueErrorsChanged)
            {
                var validatableObject = (INotifyDataErrorInfo) propertyValue;
                validatableObject.ErrorsChanged += observingProperty.ErrorsChangedAction;
            }

            if (settings.PropertyValueFactoryMethod != null)
            {
                var validatableObject = (IValidatableObject) propertyValue;
                validatableObject.Validator = settings.PropertyValueFactoryMethod.Invoke(validatableObject);
            }
            
            if (settings.TrackCollectionChanged)
            {
                var notifyCollectionChanged = (INotifyCollectionChanged) propertyValue;
                notifyCollectionChanged.CollectionChanged += observingProperty.CollectionChangedAction;
            }

            if (settings.TrackCollectionItemChanged || settings.TrackCollectionItemErrorsChanged || settings.CollectionItemFactoryMethod != null)
            {
                foreach (var item in (IEnumerable) propertyValue)
                    SubscribeCollectionItem(item, observingProperty);
            }
        }

        /// <summary>
        /// Unsubscribe from previous value of property.
        /// </summary>
        /// <param name="observingProperty">Info of observing property.</param>
        private static void UnsubscribePropertyValue(ObservingProperty observingProperty)
        {
            var propertyValue = observingProperty.PreviousValue;
            if (propertyValue == null)
                return;

            var settings = observingProperty.Settings;
            if (settings.TrackValueChanged)
            {
                var notifyPropertyChanged = (INotifyPropertyChanged) propertyValue;
                notifyPropertyChanged.PropertyChanged -= observingProperty.ValueChangedAction;
            }

            if (settings.TrackValueErrorsChanged)
            {
                var notifyDataErrorInfo = (INotifyDataErrorInfo)propertyValue;
                notifyDataErrorInfo.ErrorsChanged -= observingProperty.ErrorsChangedAction;
            }
            
            if (settings.PropertyValueFactoryMethod != null)
            {
                var validatableObject = (IValidatableObject) propertyValue;
                validatableObject.Validator = null;
            }

            if (settings.TrackCollectionChanged)
            {
                var notifyCollectionChanged = (INotifyCollectionChanged) propertyValue;
                notifyCollectionChanged.CollectionChanged -= observingProperty.CollectionChangedAction;
            }

            if (settings.TrackCollectionItemChanged || settings.TrackCollectionItemErrorsChanged || settings.CollectionItemFactoryMethod != null)
            {
                foreach (var item in (IEnumerable) propertyValue)
                    UnsubscribeCollectionItem(item, observingProperty);
            }
        }

        /// <summary>
        /// Handle instance <see cref="INotifyPropertyChanged.PropertyChanged" /> event.
        /// </summary>
        private void OnInstancePropertyChanged(object? sender, PropertyChangedEventArgs args)
        {
            var propertyName = args.PropertyName;
            
            // If propertyName is empty, it's mean that all properties are changed.
            if (string.IsNullOrEmpty(propertyName))
            {
                foreach (var observingProperty in _observingProperties.Values)
                {
                    ResubscribePropertyValue(observingProperty);
                }
            }
            else
            {
                if (_observingProperties.TryGetValue(propertyName, out var observingProperty))
                {
                    ResubscribePropertyValue(observingProperty);
                } 
            }  
                
            PropertyChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Handle <see cref="INotifyPropertyChanged" /> of item.
        /// </summary>
        /// <param name="propertyName">Name of property value or collection.</param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Handle event of changing collection items.
        /// </summary>
        private void OnCollectionChanged(string propertyName, NotifyCollectionChangedEventArgs args)
        {
            var observingProperty = _observingProperties[propertyName];

            // Unsubscribe from removed items.
            if (args.OldItems?.Count > 0)
            {
                foreach (var oldItem in args.OldItems)
                    UnsubscribeCollectionItem(oldItem, observingProperty);
            }

            // Subscribe on new items.
            if (args.NewItems?.Count > 0)
            {
                foreach (var newItem in args.NewItems)
                    SubscribeCollectionItem(newItem, observingProperty);
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(observingProperty.PropertyName));
        }

        /// <summary>
        /// Subscribe to events of collection's item.
        /// </summary>
        private static void SubscribeCollectionItem(object? item, ObservingProperty observingProperty)
        {
            if (item == null)
                return;

            var settings = observingProperty.Settings;
            if (settings.TrackCollectionItemChanged)
            {
                var notifyPropertyChanged = (INotifyPropertyChanged) item;
                notifyPropertyChanged.PropertyChanged += observingProperty.ValueChangedAction;
            }

            if (settings.TrackCollectionItemErrorsChanged)
            {
                var validatableObject = (INotifyDataErrorInfo) item;
                validatableObject.ErrorsChanged += observingProperty.ErrorsChangedAction;
            }

            if (settings.CollectionItemFactoryMethod != null)
            {
                var validatableObject = (IValidatableObject) item;
                validatableObject.Validator = settings.CollectionItemFactoryMethod.Invoke(validatableObject);
            }
        }
        
        /// <summary>
        /// Unsubscribe from events of collection's item.
        /// </summary>
        private static void UnsubscribeCollectionItem(object? item, ObservingProperty observingProperty)
        {
            if (item == null)
                return;

            var settings = observingProperty.Settings;
            if (settings.TrackCollectionItemChanged)
            {
                var notifyPropertyChanged = (INotifyPropertyChanged)item;
                notifyPropertyChanged.PropertyChanged -= observingProperty.ValueChangedAction;
            }

            if (settings.TrackCollectionItemErrorsChanged)
            {
                var validatableObject = (INotifyDataErrorInfo)item;
                validatableObject.ErrorsChanged -= observingProperty.ErrorsChangedAction;
            }

            if (settings.CollectionItemFactoryMethod != null)
            {
                var validatableObject = (IValidatableObject)item;
                validatableObject.Validator = null;
            }
        }
        
        /// <summary>
        /// Unsubscribe from all events.
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            _instance.PropertyChanged -= OnInstancePropertyChanged;
            
            foreach (var observingProperty in _observingProperties)
            {
                UnsubscribePropertyValue(observingProperty.Value);
            }
        }
    }
}
