using System;
using System.Collections;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Validator which check that property value equals to specified values.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    /// <typeparam name="TProp">Type of validatable property.</typeparam>
    /// <typeparam name="TParam">The type of comparison value.</typeparam>
    public class EqualValidator<TObject, TProp, TParam> : PropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
    {
        private readonly IEqualityComparer _comparer;
        private readonly ValidatorParameter<TObject, TParam> _valueToCompare;

        /// <summary>
        /// Initialize a new instance of <see cref="EqualValidator{TObject,TProp,TParam}" /> class.
        /// </summary>
        /// <param name="valueToCompareExpression">Expression of value to compare.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="validationMessageType">The type of validatable message.</param>
        public EqualValidator(
            Expression<Func<TObject, TParam>> valueToCompareExpression,
            IEqualityComparer comparer,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.EqualValidator), validationMessageType, valueToCompareExpression)
        {
            _comparer = comparer;
            _valueToCompare = new ValidatorParameter<TObject, TParam>(valueToCompareExpression);
        }

        /// <inheritdoc />
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


    /// <summary>
    /// Validator which check that property value equals to specified values.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    /// <typeparam name="TProp">Type of validatable property.</typeparam>
    public class EqualValidator<TObject, TProp> : EqualValidator<TObject, TProp, TProp>
        where TObject : IValidatableObject
    {
        /// <summary>
        /// Initialize a new instance of <see cref="EqualValidator{TObject,TProp}" /> class.
        /// </summary>
        /// <param name="valueToCompareExpression">Expression of value to compare.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="validationMessageType">The type of validatable message.</param>
        public EqualValidator(
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            IEqualityComparer comparer,
            ValidationMessageType validationMessageType)
            : base(valueToCompareExpression, comparer, validationMessageType)
        { }
    }
}
