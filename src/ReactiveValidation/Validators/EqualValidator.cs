using System;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class EqualValidator <TObject, TProp> : PropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
    {
        private readonly ParameterInfo<TObject, TProp> _valueToCompare;

        public EqualValidator(
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.EqualValidator), validationMessageType, valueToCompareExpression)
        {
            _valueToCompare = valueToCompareExpression.GetParameterInfo();
        }

        protected override bool IsValid(ValidationContext<TObject, TProp> context)
        {
            var paramValue = context.GetParamValue(_valueToCompare);

            var isEquals = Equals(context.PropertyValue, paramValue);
            if (isEquals == false) {
                context.RegisterMessageArgument("ValueToCompare", _valueToCompare, paramValue);
            }

            return isEquals;
        }
    }
}
