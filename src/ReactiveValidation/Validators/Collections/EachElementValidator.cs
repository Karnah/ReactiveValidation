using System;
using System.Collections.Generic;
using System.Linq;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Validator which check that all items of collection is valid by specified condition.
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TCollection">The type of collection.</typeparam>
    /// <typeparam name="TItem">The type of element of collection.</typeparam>
    public class EachElementValidator<TObject, TCollection, TItem> : BaseSyncPropertyValidator<TObject, TCollection>
        where TObject : IValidatableObject
        where TCollection : IEnumerable<TItem>
    {
        private readonly Func<TItem, bool> _validCondition;

        /// <summary>
        /// Initialize a new instance of <see cref="EachElementValidator{TObject,TCollection,TItem}" /> class.
        /// </summary>
        /// <param name="validCondition">Valid condition for item.</param>
        /// <param name="validationMessageType">The type of validatable message.</param>
        public EachElementValidator(Func<TItem, bool> validCondition, ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.EachElementValidator), validationMessageType)
        {
            _validCondition = validCondition;
        }


        /// <inheritdoc />
        protected override bool IsValid(ValidationContext<TObject, TCollection> context)
        {
            if (context.PropertyValue?.Any() != true)
                return true;

            return context.PropertyValue.All(element => _validCondition.Invoke(element));
        }
    }
}
