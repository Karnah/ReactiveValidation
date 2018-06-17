using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class WrappingValidator<TObject, TProp> : IPropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
    {
        private readonly Func<TObject, bool> _condition;

        public WrappingValidator(
            Func<TObject, bool> condition,
            IPropertyValidator<TObject, TProp> innerValidator,
            params LambdaExpression[] relatedProperties)
        {
            _condition = condition;
            InnerValidator = innerValidator;

            UnionRelatedProperties(relatedProperties);
        }


        private void UnionRelatedProperties(LambdaExpression[] baseRelatedProperties)
        {
            if (baseRelatedProperties?.Any() != true)
            {
                RelatedProperties = InnerValidator.RelatedProperties;
            }
            else
            {
                var relatedProperties = new HashSet<string>();
                foreach (var expression in baseRelatedProperties)
                {
                    var propertyName = ReactiveValidationHelper.GetPropertyName(typeof(TObject), expression);
                    if (string.IsNullOrEmpty(propertyName) == false)
                        relatedProperties.Add(propertyName);
                }

                foreach (var relatedProperty in InnerValidator.RelatedProperties)
                {
                    relatedProperties.Add(relatedProperty);
                }

                RelatedProperties = relatedProperties;
            }
        }


        public IPropertyValidator<TObject, TProp> InnerValidator { get; }

        public IEnumerable<string> RelatedProperties { get; private set; }


        public IEnumerable<ValidationMessage> ValidateProperty(ValidationContext<TObject, TProp> context)
        {
            if (_condition.Invoke(context.ValidatableObject) == false)
                return new ValidationMessage[0];

            return InnerValidator.ValidateProperty(context);
        }
    }
}
