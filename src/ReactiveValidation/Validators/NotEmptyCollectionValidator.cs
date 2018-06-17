using System.Collections.Generic;
using System.Linq;
using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class NotEmptyCollectionValidator<TObject, TCollection, TProp> : PropertyValidator<TObject, TCollection>
        where TObject : IValidatableObject
        where TCollection : IEnumerable<TProp>
    {
        public NotEmptyCollectionValidator(ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.NotEmptyCollectionValidator), validationMessageType)
        {
        }


        protected override bool IsValid(ValidationContext<TObject, TCollection> context)
        {
            return context.PropertyValue?.Any() == true;
        }
    }
}
