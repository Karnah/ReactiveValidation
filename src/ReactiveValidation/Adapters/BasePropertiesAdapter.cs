using System;
using System.Collections.Generic;

using ReactiveValidation.Validators;

namespace ReactiveValidation.Adapters
{
    /// <inheritdoc />
    internal abstract class BasePropertiesAdapter <TObject, TProp> : IPropertiesAdapter
        where TObject : IValidatableObject
    {
        /// <summary>
        /// Validator of object instance.
        /// </summary>
        private readonly ObjectValidator<TObject> _objectValidator;

        /// <remarks>
        /// If state of property A depend on property B (for example, optional field if flag is false),
        /// Then if B changed, we should revalidate property A.
        /// Property B is related property in this case.
        /// </remarks>
        private readonly SortedSet<string> _relatedProperties;
        private readonly IReadOnlyCollection<IPropertyValidator<TObject, TProp>> _propertyValidators;

        /// <inheritdoc />
        protected BasePropertiesAdapter(
            ObjectValidator<TObject> objectValidator,
            IReadOnlyCollection<IPropertyValidator<TObject, TProp>> propertyValidators)
        {
            _objectValidator = objectValidator;
            _propertyValidators = propertyValidators;

            _relatedProperties = GetRelatedProperties();
        }


        /// <inheritdoc />
        public abstract void Revalidate();

        /// <inheritdoc />
        public void Revalidate(string propertyName)
        {
            // If changed one of related properties - we should revalidate all target properties.
            if (_relatedProperties.Contains(propertyName))
            {
                Revalidate();
            }
            // If changed target property - revalidate only its.
            else if (IsTargetProperty(propertyName))
            {
                RevalidateProperty(propertyName);
            }
        }


        /// <summary>
        /// Instance of validatable object.
        /// </summary>
        internal TObject Instance => _objectValidator.Instance;


        /// <summary>
        /// Check if this adapter should validate specified property.
        /// </summary>
        protected abstract bool IsTargetProperty(string propertyName);

        /// <summary>
        /// Revalidate specified property.
        /// </summary>
        protected abstract void RevalidateProperty(string propertyName);


        /// <summary>
        /// Get actual value of property.
        /// </summary>
        protected TProp GetPropertyValue(string propertyName)
        {
            return _objectValidator.GetPropertyValue<TProp>(propertyName);
        }

        /// <summary>
        /// Create context for validating specified property.
        /// </summary>
        protected ValidationContext<TObject, TProp> GetValidationContext(string propertyName)
        {
            return _objectValidator.GetValidationContext<TProp>(propertyName);
        }

        /// <summary>
        /// Revalidate specified property.
        /// </summary>
        protected void InnerRevalidateProperty(string propertyName)
        {
            var parentContext = GetValidationContext(propertyName);
            var messages = new List<ValidationMessage>();
            foreach (var propertyValidator in _propertyValidators)
            {
                var context = new ValidationContext<TObject, TProp>(parentContext);

                IEnumerable<ValidationMessage> validationMessages;
                try
                {
                    validationMessages = propertyValidator.ValidateProperty(context);
                }
                catch (Exception e)
                {
                    validationMessages = new[] { new ValidationMessage(new ExceptionSource(e)) };
                }

                messages.AddRange(validationMessages);
            }

            _objectValidator.SetValidationMessages(parentContext.PropertyName, this, messages);
        }


        /// <summary>
        /// Get list of all related properties.
        /// </summary>
        private SortedSet<string> GetRelatedProperties()
        {
            var relatedProperties = new SortedSet<string>();
            foreach (var propertyValidator in _propertyValidators)
            {
                foreach (var relatedProperty in propertyValidator.RelatedProperties)
                {
                    relatedProperties.Add(relatedProperty);
                }
            }

            return relatedProperties;
        }
    }
}
