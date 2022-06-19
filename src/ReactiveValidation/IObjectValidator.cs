using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReactiveValidation
{
    /// <summary>
    /// Class which store all validation information about object.
    /// </summary>
    public interface IObjectValidator : IDisposable
    {
        /// <summary>
        /// <see langword="true" /> if doesn't exists any message with <see cref="ValidationMessageType.Error" /> or <see cref="ValidationMessageType.SimpleError" /> type.
        /// <see langword="false" /> otherwise.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// <see langword="true" /> if exists any message with <see cref="ValidationMessageType.Warning" /> or <see cref="ValidationMessageType.SimpleWarning" /> type.
        /// <see langword="false" /> otherwise.
        /// </summary>
        bool HasWarnings { get; }

        /// <summary>
        /// <see langword="true" /> if async validation is running.
        /// <see langword="false" /> otherwise.
        /// </summary>
        bool IsAsyncValidating { get; }
        
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

        /// <summary>
        /// Wait until async validation is over.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        Task WaitValidatingAsync(CancellationToken cancellationToken = default);
    }
}
