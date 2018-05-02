using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class GreaterThanValidator<TObject, TProp> : AbstractComparisonValidator<TObject, TProp>
        where TObject : IValidatableObject
        where TProp : IComparable<TProp>
    {
        public GreaterThanValidator(
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            IComparer<TProp> comparer,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.GreaterThanValidator), valueToCompareExpression, comparer, validationMessageType)
        { }


        protected override bool IsValid(int comparationResult)
        {
            return comparationResult > 0;
        }
    }
}
