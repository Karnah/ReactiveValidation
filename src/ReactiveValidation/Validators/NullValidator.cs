using ReactiveValidation.Helpers;
using ReactiveValidation.Resources.StringSources;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Validator which check property value is <see langword="null" />.
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TProp">The type of validatable property.</typeparam>
    public class NullValidator<TObject, TProp> : BaseSyncPropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
    {
        /// <summary>
        /// Initialize a new instance of <see cref="NullValidator{TObject,TProp}" /> class.
        /// </summary>
        /// <param name="validationMessageType">The type validatable message.</param>
        public NullValidator(ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.NullValidator), validationMessageType)
        { }


        /// <inheritdoc />
        protected override bool IsValid(ValidationContext<TObject, TProp> context)
        {
            return context.PropertyValue == null;
        }
    }
}
