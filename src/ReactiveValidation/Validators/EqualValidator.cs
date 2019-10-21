using System;
using System.Collections;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class EqualValidator<TObject, TProp, TParam> : PropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
    {
        private readonly IEqualityComparer _comparer;
        private readonly ValidatorParameter<TObject, TParam> _valueToCompare;

        public EqualValidator(
            Expression<Func<TObject, TParam>> valueToCompareExpression,
            IEqualityComparer comparer,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.EqualValidator), validationMessageType, valueToCompareExpression)
        {
            _comparer = comparer;
            _valueToCompare = new ValidatorParameter<TObject, TParam>(valueToCompareExpression);
        }

        protected override bool IsValid(ValidationContext<TObject, TProp> context)
        {
            var propertyValue = context.PropertyValue;
            if (propertyValue == null)
                return true;

            var paramValue = context.GetParamValue(_valueToCompare);
            var isEquals = _comparer?.Equals(context.PropertyValue, paramValue) ?? Equals(context.PropertyValue, paramValue);
            if (isEquals == false) {
                context.RegisterMessageArgument("ValueToCompare", _valueToCompare, paramValue);
            }

            return isEquals;
        }
    }

    public class EqualValidator<TObject, TProp> : EqualValidator<TObject, TProp, TProp>
        where TObject : IValidatableObject
    {
        public EqualValidator(
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            IEqualityComparer comparer,
            ValidationMessageType validationMessageType)
            : base(valueToCompareExpression, comparer, validationMessageType)
        { }
    }
}
