using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Wrap for validators which doesn't support <see cref="IPropertyValidatorSettings{TObject}" />.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    public class WrappingValidator<TObject> : IPropertyValidator<TObject>
        where TObject : IValidatableObject
    {
        private readonly Func<TObject, bool> _condition;

        /// <summary>
        /// Create new instance of wrapping validator.
        /// </summary>
        /// <param name="condition">Condition the using inner validator.</param>
        /// <param name="innerValidator">Inner validator.</param>
        /// <param name="relatedProperties">Related properties of condition.</param>
        public WrappingValidator(
            Func<TObject, bool> condition,
            IPropertyValidator<TObject> innerValidator,
            params LambdaExpression[] relatedProperties)
        {
            _condition = condition;
            InnerValidator = innerValidator;

            UnionRelatedProperties(relatedProperties);
        }


        /// <summary>
        /// Wrapped validator.
        /// </summary>
        public IPropertyValidator<TObject> InnerValidator { get; }

        /// <inheritdoc />
        public IReadOnlyList<string> RelatedProperties { get; private set; }


        /// <inheritdoc />
        public IReadOnlyList<ValidationMessage> ValidateProperty(ValidationContextFactory<TObject> contextFactory)
        {
            if (_condition.Invoke(contextFactory.ValidatableObject) == false)
                return new ValidationMessage[0];

            return InnerValidator.ValidateProperty(contextFactory);
        }

        /// <summary>
        /// Union related properties of inner validator and condition.
        /// </summary>
        /// <param name="conditionRelatedProperties">Related properties of condition.</param>
        private void UnionRelatedProperties(LambdaExpression[] conditionRelatedProperties)
        {
            if (conditionRelatedProperties?.Any() != true)
            {
                RelatedProperties = InnerValidator.RelatedProperties;
                return;
            }

            var relatedProperties = new HashSet<string>(InnerValidator.RelatedProperties);
            foreach (var expression in conditionRelatedProperties)
            {
                var propertyName = ReactiveValidationHelper.GetPropertyName(typeof(TObject), expression);
                if (!string.IsNullOrEmpty(propertyName))
                    relatedProperties.Add(propertyName);
            }

            RelatedProperties = relatedProperties.ToList();
        }
    }
}
