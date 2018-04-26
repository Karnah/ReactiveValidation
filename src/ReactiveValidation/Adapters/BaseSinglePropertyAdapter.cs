using System;
using System.Collections.Generic;

using ReactiveValidation.Validators;

namespace ReactiveValidation.Adapters
{
    internal abstract class BaseSinglePropertyAdapter<TObject, TProp> : BasePropertiesAdapter<TObject, TProp>
        where TObject : IValidatableObject
    {
        private TProp _lastValue;
        private IEnumerable<IDisposable> _propertySubscriptions;
        private bool _isPropertyTypeObservable;

        protected readonly string PropertyName;

        protected BaseSinglePropertyAdapter(
            ObjectValidator<TObject> validator,
            IReadOnlyCollection<IPropertyValidator<TObject, TProp>> propertyValidators,
            string propertyName)
            : base(validator, propertyValidators)
        {
            PropertyName = propertyName;

            CheckIfPropertyTypeIsObservable();
        }

        private void CheckIfPropertyTypeIsObservable()
        {
            _isPropertyTypeObservable = IsPropertyTypeObservable();
        }


        protected abstract bool IsPropertyTypeObservable();

        protected abstract IEnumerable<IDisposable> SubsribeToProperty(TProp property);


        protected void ResubscribeProperty(TProp newValue)
        {
            if (_isPropertyTypeObservable == false)
                return;

            if (Equals(_lastValue, newValue) == true)
                return;

            foreach (var subscription in _propertySubscriptions) {
                subscription.Dispose();
            }

            _lastValue = newValue;
            _propertySubscriptions = SubsribeToProperty(newValue);
        }


        public override void Revalidate()
        {
            RevalidateProperty(PropertyName);
        }

        protected override bool IsTargetProperty(string propertyName)
        {
            return PropertyName == propertyName;
        }

        protected override void RevalidateProperty(string propertyName)
        {
            InnerRevalidateProperty(propertyName);

            var propertyValue = GetPropertyValue(propertyName);
            ResubscribeProperty(propertyValue);
        }


        protected class NotificationsSubscriber<T> : IDisposable
        {
            private readonly T _value;
            private readonly Action<T> _unsubscribe;

            public NotificationsSubscriber(T value, Action<T> subsribe, Action<T> unsubscribe)
            {
                subsribe.Invoke(value);

                _value = value;
                _unsubscribe = unsubscribe;
            }


            public void Dispose()
            {
                _unsubscribe.Invoke(_value);
            }
        }
    }
}
