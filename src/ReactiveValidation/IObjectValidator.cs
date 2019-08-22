using System.Collections.Generic;

namespace ReactiveValidation
{
    /// <summary>
    /// Class which store all validation information about object.
    /// </summary>
    public interface IObjectValidator
    {
        /// <summary>
        /// <see lngword="true" /> if doesn't exists any message with <see cref="ValidationMessageType.Error" /> or <see cref="ValidationMessageType.SimpleError" /> type.
        /// <see langword="false" /> otherwise.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// <see lngword="true" /> if exists any message with <see cref="ValidationMessageType.Warning" /> or <see cref="ValidationMessageType.SimpleWarning" /> type.
        /// <see lngword="false" /> otherwise.
        /// </summary>
        bool HasWarnings { get; }

        /// <summary>
        /// List of all validation messages;
        /// </summary>
        IReadOnlyList<ValidationMessage> ValidationMessages { get; }


        /// <summary>
        /// Get all validation messages for property.
        /// </summary>
        /// <param name="propertyName">Name of property.</param>
        IReadOnlyList<ValidationMessage> GetMessages(string propertyName);

        /// <summary>
        /// Revalidate all properties.
        /// </summary>
        void Revalidate();
    }
}
