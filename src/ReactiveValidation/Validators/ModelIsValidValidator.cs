using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class ModelIsValidValidator<TObject, TProp> : PropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
        where TProp : IValidatableObject
    {
        public ModelIsValidValidator(ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.ModelIsValidValidator), validationMessageType)
        { }


        protected override bool IsValid(ValidationContext<TObject, TProp> context)
        {
            return context.PropertyValue?.Validator?.IsValid != false;
        }
    }
}
