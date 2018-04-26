using System;
using System.Collections.Generic;
using System.Linq;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class EachElementValidator<TObject, TCollection, TProp> : PropertyValidator<TObject, TCollection>
        where TObject : IValidatableObject
        where TCollection : IEnumerable<TProp>
    {
        private readonly Func<TProp, bool> _validCondition;

        public EachElementValidator(Func<TProp, bool> validCondition, ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.EachElementValidator), validationMessageType)
        {
            _validCondition = validCondition;
        }


        protected override bool IsValid(ValidationContext<TObject, TCollection> context)
        {
            if (context.PropertyValue?.Any() != true)
                return true;

            return context.PropertyValue.All(element => _validCondition.Invoke(element) != false);
        }
    }
}
