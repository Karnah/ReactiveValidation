using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class NotEmptyStringValidator<TObject> : PropertyValidator<TObject, string>
        where TObject : IValidatableObject
    {
        public NotEmptyStringValidator(ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.NotEmptyStringValidator), validationMessageType)
        {}


        protected override bool IsValid(ValidationContext<TObject, string> context)
        {
            return string.IsNullOrEmpty(context.PropertyValue) == false;
        }
    }
}
