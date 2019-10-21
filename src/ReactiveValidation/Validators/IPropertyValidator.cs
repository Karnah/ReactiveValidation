using System.Collections.Generic;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Validator which check property value.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    public interface IPropertyValidator<TObject>
        where TObject : IValidatableObject
    {
        /// <remarks>
        /// Properties which can affect on state of validatable property.
        /// </remarks>
        IReadOnlyList<string> RelatedProperties { get; }


        /// <summary>
        /// Get validation messages of property.
        /// </summary>
        /// <param name="contextFactory">Factory which allows create validation context for property.</param>
        /// <returns>List of validation messages.</returns>
        IReadOnlyList<ValidationMessage> ValidateProperty(ValidationContextFactory<TObject> contextFactory);
    }
}