using ReactiveValidation.Helpers;
using ReactiveValidation.Resources.StringSources;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Validator which check property value is not null or empty string.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    public class NotEmptyStringValidator<TObject> : BaseSyncPropertyValidator<TObject, string>
        where TObject : IValidatableObject
    {
        /// <summary>
        /// Initialize a new instance of <see cref="NotEmptyStringValidator{TObject}" /> class.
        /// </summary>
        /// <param name="validationMessageType">Type of validation message.</param>
        public NotEmptyStringValidator(ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.NotEmptyStringValidator), validationMessageType)
        {}


        /// <inheritdoc />
        protected override bool IsValid(ValidationContext<TObject, string> context)
        {
            return string.IsNullOrEmpty(context.PropertyValue) == false;
        }
    }
}
