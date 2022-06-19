using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Validator which check that property value between specified values.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    /// <typeparam name="TProp">Type of validatable property.</typeparam>
    /// <typeparam name="TParam">The type of comparison value.</typeparam>
    public class BetweenValidator<TObject, TProp, TParam> : BaseSyncPropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
    {
        private readonly IComparer _comparer;
        private readonly ValidatorParameter<TObject, TParam> _from;
        private readonly ValidatorParameter<TObject, TParam> _to;

        /// <summary>
        /// Initialize a new instance of <see cref="BetweenValidator{TObject,TProp,TParam}" /> class.
        /// </summary>
        /// <param name="fromExpression">Expression of the lowest allowed value.</param>
        /// <param name="toExpression">Expression of the highest allowed value.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="validationMessageType">The type of validatable message.</param>
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


        /// <inheritdoc />
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

    /// <summary>
    /// Validator which check that property value between specified values.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    /// <typeparam name="TProp">Type of validatable property.</typeparam>
    public class BetweenValidator<TObject, TProp> : BetweenValidator<TObject, TProp, TProp>
        where TObject : IValidatableObject
        where TProp : IComparable<TProp>
    {
        /// <summary>
        /// Initialize a new instance of <see cref="BetweenValidator{TObject,TProp}" /> class.
        /// </summary>
        /// <param name="fromExpression">Expression of the lowest allowed value.</param>
        /// <param name="toExpression">Expression of the highest allowed value.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="validationMessageType">The type of validatable message.</param>
        public BetweenValidator(
            Expression<Func<TObject, TProp>> fromExpression,
            Expression<Func<TObject, TProp>> toExpression,
            IComparer comparer,
            ValidationMessageType validationMessageType)
            : base(fromExpression, toExpression, comparer, validationMessageType)
        { }
    }
}
