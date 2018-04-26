using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class ComplexValidator<TObject, TProp> : IPropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
    {
        private readonly Func<TObject, bool> _condition;
        private readonly IEnumerable<IPropertyValidator<TObject, TProp>> _innerValidators;

        public ComplexValidator(
            Func<TObject, bool> condition,
            IEnumerable<IPropertyValidator<TObject, TProp>> innerValidators,
            params LambdaExpression[] relatedProperties)
        {
            _condition = condition;
            _innerValidators = innerValidators;

            UnionRelatedProperties(relatedProperties);
        }


        private void UnionRelatedProperties(LambdaExpression[] baseRelatedProperties)
        {
            var relatedProperties = new HashSet<string>();

            if (baseRelatedProperties != null) {
                foreach (var expression in baseRelatedProperties) {
                    var propertyName = ReactiveValidationHelper.GetPropertyName(typeof(TObject), expression);
                    if (string.IsNullOrEmpty(propertyName) == false)
                        relatedProperties.Add(propertyName);
                }
            }

            foreach (var innerValidator in _innerValidators) {
                foreach (var innerValidatorRelatedProperty in innerValidator.RelatedProperties) {
                    relatedProperties.Add(innerValidatorRelatedProperty);
                }
            }

            RelatedProperties = relatedProperties;
        }


        public IEnumerable<string> RelatedProperties { get; private set; }


        public IEnumerable<ValidationMessage> ValidateProperty(ValidationContext<TObject, TProp> parentContext)
        {
            if (_condition != null && _condition.Invoke(parentContext.ValidatableObject) == false)
                return new ValidationMessage[0];

            var validationMessages = new List<ValidationMessage>();
            foreach (var validator in _innerValidators) {
                var context = new ValidationContext<TObject, TProp>(parentContext);
                var innerMessages = validator.ValidateProperty(context)
                    .Where(vm => vm != ValidationMessage.Empty)
                    .ToList();
                if (innerMessages.Any() == true) {
                    validationMessages.AddRange(innerMessages);
                }
            }

            return validationMessages;
        }
    }
}
