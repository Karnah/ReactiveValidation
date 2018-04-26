using System;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class GreaterThanOrEqualValidator <TObject, TProp> : AbstractComparisonValidator<TObject, TProp>
        where TObject : IValidatableObject
        where TProp : IComparable
    {
        public GreaterThanOrEqualValidator(
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.GreaterThanOrEqualValidator), valueToCompareExpression, validationMessageType)
        { }


        protected override bool IsValid(int comparationResult)
        {
            return comparationResult >= 0;
        }
    }
}
