using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class LessThanOrEqualValidator <TObject, TProp> : AbstractComparisonValidator<TObject, TProp>
        where TObject : IValidatableObject
        where TProp : IComparable<TProp>
    {
        public LessThanOrEqualValidator(
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            IComparer<TProp> comparer,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.LessThanOrEqualValidator), valueToCompareExpression, comparer, validationMessageType)
        { }

        protected override bool IsValid(int comparationResult)
        {
            return comparationResult <= 0;
        }
    }
}
