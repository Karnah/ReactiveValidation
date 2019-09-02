using System.Collections.Generic;
using System.Linq;

using ReactiveValidation.Validators;

namespace ReactiveValidation
{
    /// <summary>
    /// Info for validatable property.
    /// </summary>
    public class ValidatablePropertyInfo<TObject>
        where TObject : IValidatableObject
    {
        /// <summary>
        /// Create new class of info for validatable property.
        /// </summary>
        /// <param name="propertyName">Name of property.</param>
        /// <param name="displayNameSource">Source of display name.</param>
        /// <param name="validators">List of all property validators.</param>
        public ValidatablePropertyInfo(string propertyName, IStringSource displayNameSource, IReadOnlyList<IPropertyValidator<TObject>> validators)
        {
            PropertyName = propertyName;
            DisplayNameSource = displayNameSource;
            Validators = validators;
            ValidatorsValidationMessages = Validators.ToDictionary(v => v, _ => (IReadOnlyList<ValidationMessage>) new ValidationMessage[0]);
        }

        /// <summary>
        /// Name of property.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Source of display name.
        /// </summary>
        public IStringSource DisplayNameSource { get; }

        /// <summary>
        /// List of all property validators.
        /// </summary>
        public IReadOnlyList<IPropertyValidator<TObject>> Validators { get; }

        /// <summary>
        /// List of property validators and its current validation messages.
        /// </summary>
        public IDictionary<IPropertyValidator<TObject>, IReadOnlyList<ValidationMessage>> ValidatorsValidationMessages { get; }

        /// <summary>
        /// Validation messages of property.
        /// </summary>
        public IReadOnlyList<ValidationMessage> ValidationMessages => ValidatorsValidationMessages.SelectMany(vm => vm.Value).ToList();
    }
}
