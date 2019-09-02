using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

using ReactiveValidation.Helpers;

namespace ReactiveValidation
{
    /// <summary>
    /// Class which track changing properties and raise events for <see cref="ObjectValidator{TObject}"/>
    /// </summary>
    /// <typeparam name="TObject">Type of observable object.</typeparam>
    internal class ObjectObserver<TObject>
        where TObject : INotifyPropertyChanged
    {
        private readonly TObject _instance;
        private readonly Dictionary<string, ObservingPropertyInfo> _observingProperties;

        /// <summary>
        /// Create new object observer.
        /// </summary>
        /// <param name="instance">Instance of observing object.</param>
        public ObjectObserver(TObject instance)
        {
            _instance = instance;
            _observingProperties = new Dictionary<string, ObservingPropertyInfo>();

            _instance.PropertyChanged += InstanceOnPropertyChanged;
        }


        /// <summary>
        /// Event of changing property of observable object.
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs> PropertyChanged;


        /// <summary>
        /// Start track when change property value.
        /// </summary>
        /// <param name="propertyName">Name of property.</param>
        /// <param name="factoryMethod">Method which allow create new object validator.</param>
        public void TrackPropertyValue(string propertyName, Func<IValidatableObject, IObjectValidator> factoryMethod = null)
        {
            if (_observingProperties.TryGetValue(propertyName, out var propertyInfo))
            {
                if (propertyInfo.FactoryMethod != null && factoryMethod != null)
                    throw new Exception("Factory method for property already created");

                return;
            }

            var observingInfo = new ObservingPropertyInfo
            {
                PropertyName = propertyName,
                FactoryMethod = factoryMethod
            };
            _observingProperties.Add(propertyName, observingInfo);

            SubscribePropertyValue(observingInfo);
        }

        /// <summary>
        /// Subscribe on new value of property.
        /// </summary>
        /// <param name="observingInfo">Info of observing property.</param>
        private void SubscribePropertyValue(ObservingPropertyInfo observingInfo)
        {
            var propertyName = observingInfo.PropertyName;
            var propertyValue = ReactiveValidationHelper.GetPropertyValue<object>(_instance, propertyName);
            if (propertyValue == null)
                return;

            switch (propertyValue)
            {
                case IValidatableObject validatableObject:
                {
                    observingInfo.PreviousValue = propertyValue;
                    observingInfo.ErrorsChangedAction = (sender, args) => OnErrorsChanged(propertyName);

                    validatableObject.ErrorsChanged += observingInfo.ErrorsChangedAction;

                    if (observingInfo.FactoryMethod != null)
                        validatableObject.Validator = observingInfo.FactoryMethod.Invoke(validatableObject);

                    break;
                }

                case INotifyCollectionChanged notifyCollectionChanged:
                {
                    observingInfo.PreviousValue = propertyValue;
                    observingInfo.ErrorsChangedAction = (sender, args) => OnErrorsChanged(propertyName);
                    observingInfo.CollectionChangedAction = (sender, args) => OnCollectionChanged(propertyName, args);

                    notifyCollectionChanged.CollectionChanged += observingInfo.CollectionChangedAction;

                    foreach (IValidatableObject item in (IEnumerable) propertyValue)
                    {
                        item.ErrorsChanged += observingInfo.ErrorsChangedAction;

                        if (observingInfo.FactoryMethod != null)
                            item.Validator = observingInfo.FactoryMethod.Invoke(item);
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Unsubscribe from previous value of property.
        /// </summary>
        /// <param name="observingInfo">Info of observing property.</param>
        private static void UnsubscribePropertyValue(ObservingPropertyInfo observingInfo)
        {
            switch (observingInfo.PreviousValue)
            {
                case IValidatableObject validatableObject:
                {
                    validatableObject.ErrorsChanged -= observingInfo.ErrorsChangedAction;
                    break;
                }

                case INotifyCollectionChanged notifyCollectionChanged:
                {
                    notifyCollectionChanged.CollectionChanged -= observingInfo.CollectionChangedAction;

                    foreach (IValidatableObject item in (IEnumerable) observingInfo.PreviousValue)
                        item.ErrorsChanged -= observingInfo.ErrorsChangedAction;

                    break;
                }
            }

            observingInfo.CollectionChangedAction = null;
            observingInfo.ErrorsChangedAction = null;
        }

        /// <summary>
        /// Handle instance <see cref="INotifyPropertyChanged.PropertyChanged" /> event.
        /// </summary>
        private void InstanceOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var propertyName = args.PropertyName;
            if (_observingProperties.TryGetValue(propertyName, out var propertyInfo))
            {
                UnsubscribePropertyValue(propertyInfo);
                SubscribePropertyValue(propertyInfo);
            }

            PropertyChanged?.Invoke(this, args);
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
            var observingInfo = _observingProperties[propertyName];

            // Unsubscribe from removed items.
            if (args.OldItems?.Count > 0)
            {
                foreach (var oldItem in args.OldItems)
                {
                    if (oldItem is IValidatableObject validatableObject)
                        validatableObject.ErrorsChanged -= observingInfo.ErrorsChangedAction;
                }
            }

            // Subscribe on new items.
            if (args.NewItems?.Count > 0)
            {
                foreach (var newItem in args.NewItems)
                {
                    if (newItem is IValidatableObject validatableObject)
                        validatableObject.ErrorsChanged += observingInfo.ErrorsChangedAction;
                }
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(observingInfo.PropertyName));
        }
    }
}
