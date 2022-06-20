using System;
using System.Collections;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Validator which check property value not equal specified value.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    /// <typeparam name="TProp">Type of validatable property.</typeparam>
    /// <typeparam name="TParam">The type of comparison value.</typeparam>
    public class NotEqualValidator<TObject, TProp, TParam> : BaseSyncPropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
    {
        private readonly IEqualityComparer _comparer;
        private readonly ValidatorParameter<TObject, TParam> _valueToCompare;

        /// <summary>
        /// Initialize a new instance of <see cref="NotEqualValidator{TObject,TProp,TParam}" /> class.
        /// </summary>
        /// <param name="valueToCompareExpression">Expression of value to compare.</param>
        /// <param name="comparer">The values comparer.</param>
        /// <param name="validationMessageType">Type of validation message.</param>
        public NotEqualValidator(Expression<Func<TObject, TParam>> valueToCompareExpression, IEqualityComparer comparer,
            ValidationMessageType validationMessageType) : base(
            new LanguageStringSource(ValidatorsNames.NotEqualValidator), validationMessageType,
            valueToCompareExpression)
        {
            _comparer = comparer;
            _valueToCompare = new ValidatorParameter<TObject, TParam>(valueToCompareExpression);
        }

        /// <inheritdoc />
        protected override bool IsValid(ValidationContext<TObject, TProp> context)
        {
            var propertyValue = context.PropertyValue;
            var paramValue = context.GetParamValue(_valueToCompare);
            var isEquals = _comparer?.Equals(propertyValue, paramValue) ?? Equals(propertyValue, paramValue);
            if (isEquals)
            {
                context.RegisterMessageArgument("ValueToCompare", _valueToCompare, paramValue);
            }

            return !isEquals;
        }
    }

    /// <summary>
    /// Validator which check property value not equal specified value.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    /// <typeparam name="TProp">Type of validatable property.</typeparam>
    public class NotEqualValidator<TObject, TProp> : NotEqualValidator<TObject, TProp, TProp>
        where TObject : IValidatableObject
    {
        /// <summary>
        /// Initialize a new instance of <see cref="NotEqualValidator{TObject,TProp}" /> class.
        /// </summary>
        /// <param name="valueToCompareExpression">Expression of value to compare.</param>
        /// <param name="comparer">The values comparer.</param>
        /// <param name="validationMessageType">Type of validation message.</param>
        public NotEqualValidator(Expression<Func<TObject, TProp>> valueToCompareExpression, IEqualityComparer comparer,
            ValidationMessageType validationMessageType) : base(valueToCompareExpression, comparer,
            validationMessageType)
        {
        }
    }
}