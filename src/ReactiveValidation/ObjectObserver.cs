using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using ReactiveValidation.Helpers;

namespace ReactiveValidation
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
            _instance.PropertyChanged += InstanceOnPropertyChanged;

            _observingProperties = settings.ToDictionary(s => s.Key, s => new ObservingProperty
            {
                PropertyName = s.Key,
                Settings = s.Value
            });

            foreach (var observingProperty in _observingProperties)
            {
                SubscribePropertyValue(observingProperty.Value);
            }
        }


        /// <summary>
        /// Event of changing property of observable object.
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs> PropertyChanged;


        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Subscribe on new value of property.
        /// </summary>
        /// <param name="observingProperty">Info of observing property.</param>
        private void SubscribePropertyValue(ObservingProperty observingProperty)
        {
            var propertyName = observingProperty.PropertyName;
            var propertyValue = ReactiveValidationHelper.GetPropertyValue<object>(_instance, propertyName);
            if (propertyValue == null)
                return;

            var settings = observingProperty.Settings;
            observingProperty.PreviousValue = propertyValue;

            if (settings.PropertyValueFactoryMethod != null)
            {
                var validatableObject = (IValidatableObject) propertyValue;
                if (settings.PropertyValueFactoryMethod != null)
                    validatableObject.Validator = settings.PropertyValueFactoryMethod.Invoke(validatableObject);
            }

            if (settings.TrackValueChanged)
            {
                if (observingProperty.ValueChangedAction == null)
                    observingProperty.ValueChangedAction = (sender, args) => OnPropertyChanged(propertyName);

                var notifyPropertyChanged = (INotifyPropertyChanged) propertyValue;
                notifyPropertyChanged.PropertyChanged += observingProperty.ValueChangedAction;
            }

            if (settings.TrackValueErrorsChanged)
            {
                if (observingProperty.ErrorsChangedAction == null)
                    observingProperty.ErrorsChangedAction = (sender, args) => OnErrorsChanged(propertyName);

                var validatableObject = (IValidatableObject) propertyValue;
                validatableObject.ErrorsChanged += observingProperty.ErrorsChangedAction;
            }

            if (settings.TrackCollectionChanged)
            {
                if (observingProperty.CollectionChangedAction == null)
                    observingProperty.CollectionChangedAction = (sender, args) => OnCollectionChanged(propertyName, args);

                var notifyCollectionChanged = (INotifyCollectionChanged) propertyValue;
                notifyCollectionChanged.CollectionChanged += observingProperty.CollectionChangedAction;
            }

            if (settings.TrackCollectionItemChanged)
            {
                if (observingProperty.ValueChangedAction == null)
                    observingProperty.ValueChangedAction = (sender, args) => OnPropertyChanged(propertyName);

                foreach (INotifyPropertyChanged item in (IEnumerable) propertyValue)
                {
                    item.PropertyChanged += observingProperty.ValueChangedAction;
                }
            }

            if (settings.TrackCollectionItemErrorsChanged)
            {
                if (observingProperty.ErrorsChangedAction == null)
                    observingProperty.ErrorsChangedAction = (sender, args) => OnErrorsChanged(propertyName);

                foreach (IValidatableObject item in (IEnumerable) propertyValue)
                {
                    item.ErrorsChanged += observingProperty.ErrorsChangedAction;
                }
            }

            if (settings.CollectionItemFactoryMethod != null)
            {
                foreach (IValidatableObject item in (IEnumerable) propertyValue)
                {
                    item.Validator = settings.CollectionItemFactoryMethod.Invoke(item);
                }
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

            if (settings.TrackValueErrorsChanged || settings.PropertyValueFactoryMethod != null)
            {
                var validatableObject = (IValidatableObject) propertyValue;
                validatableObject.ErrorsChanged -= observingProperty.ErrorsChangedAction;
                validatableObject.Validator?.Dispose();
            }

            if (settings.TrackCollectionChanged)
            {
                var notifyCollectionChanged = (INotifyCollectionChanged) propertyValue;
                notifyCollectionChanged.CollectionChanged -= observingProperty.CollectionChangedAction;
            }

            if (settings.TrackCollectionItemChanged)
            {
                foreach (INotifyPropertyChanged item in (IEnumerable) propertyValue)
                {
                    item.PropertyChanged -= observingProperty.ValueChangedAction;
                }
            }

            if (settings.TrackCollectionItemErrorsChanged || settings.CollectionItemFactoryMethod != null)
            {
                foreach (IValidatableObject item in (IEnumerable) propertyValue)
                {
                    item.ErrorsChanged -= observingProperty.ErrorsChangedAction;
                    item.Validator?.Dispose();
                }
            }
        }

        /// <summary>
        /// Handle instance <see cref="INotifyPropertyChanged.PropertyChanged" /> event.
        /// </summary>
        private void InstanceOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var propertyName = args.PropertyName;
            if (_observingProperties.TryGetValue(propertyName, out var observingProperty))
            {
                UnsubscribePropertyValue(observingProperty);
                SubscribePropertyValue(observingProperty);
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
        /// Handle changed errors of value.
        /// </summary>
        /// <param name="propertyName">Name of property value or collection.</param>
        private void OnErrorsChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Handle event of changing collection items.
        /// </summary>
        private void OnCollectionChanged(string propertyName, NotifyCollectionChangedEventArgs args)
        {
            var observingProperty = _observingProperties[propertyName];
            var settings = observingProperty.Settings;

            // Unsubscribe from removed items.
            if (args.OldItems?.Count > 0)
            {
                foreach (var oldItem in args.OldItems)
                {
                    if (settings.TrackCollectionItemChanged)
                    {
                        var notifyPropertyChanged = (INotifyPropertyChanged) oldItem;
                        notifyPropertyChanged.PropertyChanged -= observingProperty.ValueChangedAction;
                    }

                    if (settings.TrackCollectionItemErrorsChanged)
                    {
                        var validatableObject = (IValidatableObject)oldItem;
                        validatableObject.ErrorsChanged -= observingProperty.ErrorsChangedAction;
                    }
                }
            }

            // Subscribe on new items.
            if (args.NewItems?.Count > 0)
            {
                foreach (var newItem in args.NewItems)
                {
                    if (settings.TrackCollectionItemChanged)
                    {
                        var notifyPropertyChanged = (INotifyPropertyChanged) newItem;
                        notifyPropertyChanged.PropertyChanged += observingProperty.ValueChangedAction;
                    }

                    if (settings.TrackCollectionItemErrorsChanged)
                    {
                        var validatableObject = (IValidatableObject) newItem;
                        validatableObject.ErrorsChanged += observingProperty.ErrorsChangedAction;
                    }

                    if (settings.CollectionItemFactoryMethod != null)
                    {
                        var validatableObject = (IValidatableObject) newItem;
                        validatableObject.Validator = settings.CollectionItemFactoryMethod.Invoke(validatableObject);
                    }
                }
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(observingProperty.PropertyName));
        }

        /// <summary>
        /// Unsubscribe from all events.
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            foreach (var observingProperty in _observingProperties)
            {
                UnsubscribePropertyValue(observingProperty.Value);
            }
        }
    }
}
