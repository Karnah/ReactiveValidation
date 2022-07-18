using System;
using System.Collections;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;
using ReactiveValidation.Resources.StringSources;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Validator which check that property value is greater or equal to specified value.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    /// <typeparam name="TProp">Type of validatable property.</typeparam>
    /// <typeparam name="TParam">The type of comparison value.</typeparam>
    public class GreaterThanOrEqualValidator<TObject, TProp, TParam> : AbstractComparisonValidator<TObject, TProp, TParam>
        where TObject : IValidatableObject
    {
        /// <summary>
        /// Initialize a new instance of <see cref="GreaterThanOrEqualValidator{TObject,TProp,TParam}" /> class.
        /// </summary>
        /// <param name="valueToCompareExpression">Expression of value to compare.</param>
        /// <param name="comparer">The values comparer.</param>
        /// <param name="validationMessageType">Type of validation message.</param>
        public GreaterThanOrEqualValidator(
            Expression<Func<TObject, TParam>> valueToCompareExpression,
            IComparer? comparer,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.GreaterThanOrEqualValidator), valueToCompareExpression, comparer, validationMessageType)
        { }


        /// <inheritdoc />
        protected override bool IsValid(int comparisonResult)
        {
            return comparisonResult >= 0;
        }
    }


    /// <summary>
    /// Validator which check that property value is greater or equal to specified value.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    /// <typeparam name="TProp">Type of validatable property.</typeparam>
    public class GreaterThanOrEqualValidator<TObject, TProp> : GreaterThanOrEqualValidator<TObject, TProp, TProp>
        where TObject : IValidatableObject
        where TProp : IComparable<TProp>
    {
        /// <summary>
        /// Initialize a new instance of <see cref="GreaterThanOrEqualValidator{TObject,TProp}" /> class.
        /// </summary>
        /// <param name="valueToCompareExpression">Expression of value to compare.</param>
        /// <param name="comparer">The values comparer.</param>
        /// <param name="validationMessageType">Type of validation message.</param>
        public GreaterThanOrEqualValidator(
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            IComparer? comparer,
            ValidationMessageType validationMessageType)
            : base(valueToCompareExpression, comparer, validationMessageType)
        { }
    }
}
