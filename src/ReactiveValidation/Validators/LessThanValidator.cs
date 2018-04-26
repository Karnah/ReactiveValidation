using System;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class LessThanValidator <TObject, TProp> : AbstractComparisonValidator<TObject, TProp>
        where TObject : IValidatableObject
        where TProp : IComparable 
    {
        public LessThanValidator(Expression<Func<TObject, TProp>> valueToCompareExpression, ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.LessThanValidator), valueToCompareExpression, validationMessageType)
        { }

        protected override bool IsValid(int comparationResult)
        {
            return comparationResult < 0;
        }
    }
}
