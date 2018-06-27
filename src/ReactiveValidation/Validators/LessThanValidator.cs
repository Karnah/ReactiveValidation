using System;
using System.Collections;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class LessThanValidator<TObject, TProp, TParam> : AbstractComparisonValidator<TObject, TProp, TParam>
        where TObject : IValidatableObject
    {
        public LessThanValidator(
            Expression<Func<TObject, TParam>> valueToCompareExpression,
            IComparer comparer,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.LessThanValidator), valueToCompareExpression, comparer, validationMessageType)
        { }


        protected override bool IsValid(int comparationResult)
        {
            return comparationResult < 0;
        }
    }

    public class LessThanValidator<TObject, TProp> : LessThanValidator<TObject, TProp, TProp>
        where TObject : IValidatableObject
        where TProp : IComparable<TProp>
    {
        public LessThanValidator(
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            IComparer comparer,
            ValidationMessageType validationMessageType)
            : base(valueToCompareExpression, comparer, validationMessageType)
        { }
    }
}
