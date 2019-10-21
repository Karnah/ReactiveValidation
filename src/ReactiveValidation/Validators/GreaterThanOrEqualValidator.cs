using System;
using System.Collections;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class GreaterThanOrEqualValidator<TObject, TProp, TParam> : AbstractComparisonValidator<TObject, TProp, TParam>
        where TObject : IValidatableObject
    {
        public GreaterThanOrEqualValidator(
            Expression<Func<TObject, TParam>> valueToCompareExpression,
            IComparer comparer,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.GreaterThanOrEqualValidator), valueToCompareExpression, comparer, validationMessageType)
        { }


        protected override bool IsValid(int comparisonResult)
        {
            return comparisonResult >= 0;
        }
    }

    public class GreaterThanOrEqualValidator<TObject, TProp> : GreaterThanOrEqualValidator<TObject, TProp, TProp>
        where TObject : IValidatableObject
        where TProp : IComparable<TProp>
    {
        public GreaterThanOrEqualValidator(
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            IComparer comparer,
            ValidationMessageType validationMessageType)
            : base(valueToCompareExpression, comparer, validationMessageType)
        { }
    }
}
