using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class NullValidator <TObject, TProp> : PropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
        where TProp : class
    {
        public NullValidator(ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.NullValidator), validationMessageType)
        { }


        protected override bool IsValid(ValidationContext<TObject, TProp> context)
        {
            return context.PropertyValue == null;
        }
    }
}
