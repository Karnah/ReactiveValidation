using System;
using System.Collections;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class NotEqualValidator<TObject, TProp, TParam> : PropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
    {
        private readonly IEqualityComparer _comparer;
        private readonly ParameterInfo<TObject, TParam> _valueToCompare;

        public NotEqualValidator(
            Expression<Func<TObject, TParam>> valueToCompareExpression,
            IEqualityComparer comparer,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.NotEqualValidator), validationMessageType, valueToCompareExpression)
        {
            _comparer = comparer;
            _valueToCompare = valueToCompareExpression.GetParameterInfo();
        }


        protected override bool IsValid(ValidationContext<TObject, TProp> context)
        {
            var propertyValue = context.PropertyValue;
            if (propertyValue == null)
                return true;

            var paramValue = context.GetParamValue(_valueToCompare);
            var isEquals = _comparer?.Equals(propertyValue, paramValue) ?? Equals(context.PropertyValue, paramValue);
            if (isEquals == true) {
                context.RegisterMessageArgument("ValueToCompare", _valueToCompare, paramValue);
            }

            return isEquals == false;
        }
    }

    public class NotEqualValidator<TObject, TProp> : NotEqualValidator<TObject, TProp, TProp>
        where TObject : IValidatableObject
    {
        public NotEqualValidator(
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            IEqualityComparer comparer,
            ValidationMessageType validationMessageType)
            : base(valueToCompareExpression, comparer, validationMessageType)
        { }
    }
}
