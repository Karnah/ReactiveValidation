using System;
using System.Collections.Generic;
using System.Linq;

using ReactiveValidation.Validators;

namespace ReactiveValidation.Adapters
{
    internal abstract class BaseSinglePropertyAdapter<TObject, TProp> : BasePropertiesAdapter<TObject, TProp>
        where TObject : IValidatableObject
    {
        private TProp _lastValue;
        private IEnumerable<IDisposable> _propertyObservers;

        protected readonly string PropertyName;

        protected BaseSinglePropertyAdapter(
            ObjectValidator<TObject> validator,
            IReadOnlyCollection<IPropertyValidator<TObject, TProp>> propertyValidators,
            string propertyName)
            : base(validator, propertyValidators)
        {
            PropertyName = propertyName;
        }


        protected abstract IEnumerable<Func<TObject, TProp, Action, IDisposable>> ObserverBuilders { get; }


        protected void ResubscribeProperty(TProp newValue)
        {
            if (ObserverBuilders?.Any() != true)
                return;

            if (Equals(_lastValue, newValue))
                return;

            if (_propertyObservers?.Any() == true) {
                foreach (var propertyObserver in _propertyObservers) {
                    propertyObserver?.Dispose();
                }
            }

            _lastValue = newValue;
            _propertyObservers = newValue == null
                ? null
                : ObserverBuilders.Select(ob => ob.Invoke(Instance, newValue, Revalidate)).ToList();
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
    }
}
