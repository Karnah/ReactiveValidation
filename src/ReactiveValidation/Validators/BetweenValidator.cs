using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class BetweenValidator <TObject, TProp> : PropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
        where TProp : IComparable<TProp>
    {
        private readonly IComparer<TProp> _comparer;
        private readonly ParameterInfo<TObject, TProp> _from;
        private readonly ParameterInfo<TObject, TProp> _to;

        public BetweenValidator(
            Expression<Func<TObject, TProp>> fromExpression,
            Expression<Func<TObject, TProp>> toExpression,
            IComparer<TProp> comparer,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.BetweenValidator), validationMessageType, fromExpression, toExpression)
        {
            _comparer = comparer ?? Comparer<TProp>.Default;
            _from = fromExpression.GetParameterInfo();
            _to = toExpression.GetParameterInfo();
        }


        protected override bool IsValid(ValidationContext<TObject, TProp> context)
        {
            var fromValue = context.GetParamValue(_from);
            var toValue = context.GetParamValue(_to);

            var isLessLowBound = _comparer.Compare(context.PropertyValue, fromValue) < 0;
            var isGreaterTopBound = _comparer.Compare(context.PropertyValue, toValue) > 0;

            if (!isLessLowBound && !isGreaterTopBound)
                return true;
            context.RegisterMessageArgument("From", _from, fromValue);
            context.RegisterMessageArgument("To", _to, toValue);

            return false;

        }
    }
}
