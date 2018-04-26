using System.Collections.Generic;
using System.Linq;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class CollectionElementsAreValidValidator<TObject, TCollection, TProp> : PropertyValidator<TObject, TCollection>
        where TObject : IValidatableObject
        where TCollection : IEnumerable<TProp>
        where TProp : IValidatableObject
    {
        public CollectionElementsAreValidValidator(ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.CollectionElementsAreValidValidator), validationMessageType)
        {}


        protected override bool IsValid(ValidationContext<TObject, TCollection> context)
        {
            if (context.PropertyValue?.Any() != true)
                return true;

            return context.PropertyValue.All(element => element?.Validator.IsValid != false);
        }
    }
}
