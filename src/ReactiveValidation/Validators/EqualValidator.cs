using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class EqualValidator <TObject, TProp> : PropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
    {
        private readonly IEqualityComparer<TProp> _comparer;
        private readonly ParameterInfo<TObject, TProp> _valueToCompare;

        public EqualValidator(
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            IEqualityComparer<TProp> comparer,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.EqualValidator), validationMessageType, valueToCompareExpression)
        {
            _comparer = comparer;
            _valueToCompare = valueToCompareExpression.GetParameterInfo();
        }

        protected override bool IsValid(ValidationContext<TObject, TProp> context)
        {
            var paramValue = context.GetParamValue(_valueToCompare);

            var isEquals = _comparer?.Equals(context.PropertyValue, paramValue) ?? Equals(context.PropertyValue, paramValue);
            if (isEquals == false) {
                context.RegisterMessageArgument("ValueToCompare", _valueToCompare, paramValue);
            }

            return isEquals;
        }
    }
}
