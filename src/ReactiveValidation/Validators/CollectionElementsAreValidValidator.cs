using System.Collections.Generic;
using System.Linq;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Validator which check that all items of collection is inner valid.
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TCollection">The type of collection.</typeparam>
    /// <typeparam name="TItem">The type of element of collection.</typeparam>
    public class CollectionElementsAreValidValidator<TObject, TCollection, TItem> : PropertyValidator<TObject, TCollection>
        where TObject : IValidatableObject
        where TCollection : IEnumerable<TItem>
        where TItem : IValidatableObject
    {
        /// <summary>
        /// Initialize a new instance of <see cref="CollectionElementsAreValidValidator{TObject,TCollection,TItem}" /> class.
        /// </summary>
        /// <param name="validationMessageType">The type of validatable message.</param>
        public CollectionElementsAreValidValidator(ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.CollectionElementsAreValidValidator), validationMessageType)
        {}


        /// <inheritdoc />
        protected override bool IsValid(ValidationContext<TObject, TCollection> context)
        {
            if (context.PropertyValue?.Any() != true)
                return true;

            return context.PropertyValue.All(element => element?.Validator.IsValid != false);
        }
    }
}
