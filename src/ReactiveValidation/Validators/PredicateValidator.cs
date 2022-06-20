using System;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Validator which check property value by predicate.
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TProp">The type of validatable property.</typeparam>
    public class PredicateValidator<TObject, TProp> : BaseSyncPropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
    {
        private readonly Func<ValidationContext<TObject, TProp>, bool> _predicate;

        /// <summary>
        /// Initialize a new instance of <see cref="PredicateValidator{TObject,TProp}" /> class.
        /// </summary>
        /// <param name="predicate">Predicate which check property value.</param>
        /// <param name="validationMessageType">The type validatable message.</param>
        public PredicateValidator(Func<ValidationContext<TObject, TProp>, bool> predicate, ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.PredicateValidator), validationMessageType)
        {
            _predicate = predicate;
        }


        /// <inheritdoc />
        protected override bool IsValid(ValidationContext<TObject, TProp> context)
        {
            return _predicate.Invoke(context);
        }
    }
}
