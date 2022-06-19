using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Validator which check that property value is inner valid.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    /// <typeparam name="TProp">Type of validatable property.</typeparam>
    public class ModelIsValidValidator<TObject, TProp> : BaseSyncPropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
        where TProp : IValidatableObject
    {
        /// <summary>
        /// Initialize a new instance of <see cref="ModelIsValidValidator{TObject,TProp}" /> class.
        /// </summary>
        /// <param name="validationMessageType">Type of validation message.</param>
        public ModelIsValidValidator(ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.ModelIsValidValidator), validationMessageType)
        { }


        /// <inheritdoc />
        protected override bool IsValid(ValidationContext<TObject, TProp> context)
        {
            if (context.PropertyValue?.Validator?.IsValid == false)
                return false;

            return true;
        }
    }
}
