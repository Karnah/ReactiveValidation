using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class BetweenValidator <TObject, TProp> : PropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
        where TProp : IComparable
    {
        private readonly ParameterInfo<TObject, TProp> _from;
        private readonly ParameterInfo<TObject, TProp> _to;

        public BetweenValidator(
            Expression<Func<TObject, TProp>> fromExpression,
            Expression<Func<TObject, TProp>> toExpression,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.BetweenValidator), validationMessageType, fromExpression, toExpression)
        {
            _from = fromExpression.GetParameterInfo();
            _to = toExpression.GetParameterInfo();
        }


        protected override bool IsValid(ValidationContext<TObject, TProp> context)
        {
            var fromValue = context.GetParamValue(_from);
            var toValue = context.GetParamValue(_to);

            var isLessLowBound = Comparer<TProp>.Default.Compare(context.PropertyValue, fromValue) < 0;
            var isGreaterTopBound = Comparer<TProp>.Default.Compare(context.PropertyValue, toValue) > 0;

            if (isLessLowBound || isGreaterTopBound) {
                context.RegisterMessageArgument("From", _from, fromValue);
                context.RegisterMessageArgument("To", _to, toValue);

                return false;
            }

            return true;
        }
    }
}
