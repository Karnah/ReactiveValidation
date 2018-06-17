using System;
using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class PredicateValidator<TObject, TProp> : PropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
    {
        private readonly Func<ValidationContext<TObject, TProp>, bool> _predicate;

        public PredicateValidator(Func<ValidationContext<TObject, TProp>, bool> predicate, ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.PredicateValidator), validationMessageType)
        {
            _predicate = predicate;
        }


        protected override bool IsValid(ValidationContext<TObject, TProp> context)
        {
            return _predicate.Invoke(context);
        }
    }
}