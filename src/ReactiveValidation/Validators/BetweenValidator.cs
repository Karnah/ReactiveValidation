using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class BetweenValidator<TObject, TProp, TParam> : PropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
    {
        private readonly IComparer _comparer;
        private readonly ValidatorParameter<TObject, TParam> _from;
        private readonly ValidatorParameter<TObject, TParam> _to;

        public BetweenValidator(
            Expression<Func<TObject, TParam>> fromExpression,
            Expression<Func<TObject, TParam>> toExpression,
            IComparer comparer,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.BetweenValidator), validationMessageType, fromExpression, toExpression)
        {
            _comparer = comparer ?? Comparer<TProp>.Default;
            _from = new ValidatorParameter<TObject, TParam>(fromExpression);
            _to = new ValidatorParameter<TObject, TParam>(toExpression);
        }


        protected override bool IsValid(ValidationContext<TObject, TProp> context)
        {
            var propertyValue = context.PropertyValue;
            if (propertyValue == null)
                return true;

            var fromValue = context.GetParamValue(_from);
            var toValue = context.GetParamValue(_to);

            var isLessLowBound = _comparer.Compare(propertyValue, fromValue) < 0;
            var isGreaterTopBound = _comparer.Compare(propertyValue, toValue) > 0;

            if (isLessLowBound || isGreaterTopBound) {
                context.RegisterMessageArgument("From", _from, fromValue);
                context.RegisterMessageArgument("To", _to, toValue);

                return false;
            }

            return true;
        }
    }

    public class BetweenValidator<TObject, TProp> : BetweenValidator<TObject, TProp, TProp>
        where TObject : IValidatableObject
        where TProp : IComparable<TProp>
    {
        public BetweenValidator(
            Expression<Func<TObject, TProp>> fromExpression,
            Expression<Func<TObject, TProp>> toExpression,
            IComparer comparer,
            ValidationMessageType validationMessageType)
            : base(fromExpression, toExpression, comparer, validationMessageType)
        { }
    }
}
