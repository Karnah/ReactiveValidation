using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Validator which check condition and call validate for inner validators.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    public class ComplexValidator<TObject> : IPropertyValidator<TObject>
        where TObject : IValidatableObject
    {
        private readonly Func<TObject, bool> _condition;

        /// <summary>
        /// Create new instance of complex validator.
        /// </summary>
        /// <param name="condition">Condition the using inner validators.</param>
        /// <param name="innerValidators">Inner validators.</param>
        /// <param name="relatedProperties">Related properties of condition.</param>
        public ComplexValidator(
            Func<TObject, bool> condition,
            IReadOnlyList<IPropertyValidator<TObject>> innerValidators,
            params LambdaExpression[] relatedProperties)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            _condition = condition;
            InnerValidators = innerValidators;

            UnionRelatedProperties(relatedProperties);
        }


        /// <summary>
        /// Inner validators.
        /// </summary>
        public IReadOnlyList<IPropertyValidator<TObject>> InnerValidators { get; }

        /// <inheritdoc />
        public IReadOnlyList<string> RelatedProperties { get; private set; }


        /// <summary>
        /// Check if don't need call inner validators.
        /// </summary>
        /// <param name="validatableObject">Object which being validated.</param>
        /// <returns>
        /// <see langword="true" />, if all inner validators are valid.
        /// <see langword="false" />, if necessary to check inner validators.
        /// </returns>
        public bool IsAlwaysValid(TObject validatableObject)
        {
            return !_condition.Invoke(validatableObject);
        }

        /// <inheritdoc />
        public IReadOnlyList<ValidationMessage> ValidateProperty(ValidationContextFactory<TObject> contextFactory)
        {
            if (IsAlwaysValid(contextFactory.ValidatableObject))
                return new ValidationMessage[0];

            var validationMessages = new List<ValidationMessage>();
            foreach (var validator in InnerValidators)
            {
                ValidationMessage[] innerMessages;
                try
                {
                    innerMessages = validator.ValidateProperty(contextFactory)
                        .Where(vm => vm != ValidationMessage.Empty)
                        .ToArray();
                }
                catch (Exception e)
                {
                    innerMessages = new[] { new ValidationMessage(new ExceptionSource(e)) };
                }

                if (innerMessages.Any())
                {
                    validationMessages.AddRange(innerMessages);
                }
            }

            return validationMessages;
        }

        /// <summary>
        /// Union related properties of inner validator and condition.
        /// </summary>
        /// <param name="conditionRelatedProperties">Related properties of condition.</param>
        private void UnionRelatedProperties(LambdaExpression[] conditionRelatedProperties)
        {
            var relatedProperties = new HashSet<string>();

            if (conditionRelatedProperties != null)
            {
                foreach (var expression in conditionRelatedProperties)
                {
                    var propertyName = ReactiveValidationHelper.GetPropertyName(typeof(TObject), expression);
                    if (!string.IsNullOrEmpty(propertyName))
                        relatedProperties.Add(propertyName);
                }
            }

            foreach (var innerValidator in InnerValidators)
            {
                foreach (var innerValidatorRelatedProperty in innerValidator.RelatedProperties)
                {
                    if (!string.IsNullOrEmpty(innerValidatorRelatedProperty))
                        relatedProperties.Add(innerValidatorRelatedProperty);
                }
            }

            RelatedProperties = relatedProperties.ToList();
        }
    }
}
