using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class NotNullValidator<TObject, TProp> : PropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
    {
        public NotNullValidator(ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.NotNullValidator), validationMessageType)
        { }


        protected override bool IsValid(ValidationContext<TObject, TProp> context)
        {
            return context.PropertyValue != null;
        }
    }
}
