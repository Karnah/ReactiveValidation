using System.Collections.Generic;

using ReactiveValidation.Validators;

namespace ReactiveValidation.Adapters
{
    internal abstract class BasePropertiesAdapter <TObject, TProp> : IPropertiesAdapter
        where TObject : IValidatableObject
    {
        private readonly ObjectValidator<TObject> _objectValidator;

        private readonly SortedSet<string> _relatedProperties;
        private readonly IReadOnlyCollection<IPropertyValidator<TObject, TProp>> _propertyValidators;

        protected BasePropertiesAdapter(
            ObjectValidator<TObject> objectValidator,
            IReadOnlyCollection<IPropertyValidator<TObject, TProp>> propertyValidators)
        {
            _objectValidator = objectValidator;
            _propertyValidators = propertyValidators;

            _relatedProperties = new SortedSet<string>();

            SubsribeRelatedProperties();
        }


        private void SubsribeRelatedProperties()
        {
            foreach (var propertyValidator in _propertyValidators) {
                foreach (var relatedProperty in propertyValidator.RelatedProperties) {
                    _relatedProperties.Add(relatedProperty);
                }
            }
        }


        public abstract void Revalidate();

        public void Revalidate(string propertyName)
        {
            //If changed one of related properties - we should revalidate all target properties
            if (_relatedProperties.Contains(propertyName))
            {
                Revalidate();
            }
            //If changed target property - revalidate only its
            else if (IsTargetProperty(propertyName) == true)
            {
                RevalidateProperty(propertyName);
            }
        }


        protected abstract bool IsTargetProperty(string propertyName);

        protected abstract void RevalidateProperty(string propertyName);


        protected TProp GetPropertyValue(string propertyName)
        {
            return _objectValidator.GetPropertyValue<TProp>(propertyName);
        }

        protected ValidationContext<TObject, TProp> GetValidationContext(string propertyName)
        {
            return _objectValidator.GetValidationContext<TProp>(propertyName);
        }

        protected void InnerRevalidateProperty(string propertyName)
        {
            var parentContext = GetValidationContext(propertyName);
            var messages = new List<ValidationMessage>();
            foreach (var propertyValidator in _propertyValidators) {
                var context = new ValidationContext<TObject, TProp>(parentContext);
                var validationMessage = propertyValidator.ValidateProperty(context);

                messages.AddRange(validationMessage);
            }

            _objectValidator.SetValidationMessages(parentContext.PropertyName, this, messages);
        }
    }
}
