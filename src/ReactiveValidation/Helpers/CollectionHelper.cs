using System.Collections.Generic;
using System.Linq;

namespace ReactiveValidation.Helpers
{
    /// <summary>
    /// Method which helps work with collections.
    /// </summary>
    internal static class CollectionHelper
    {
        /// <summary>
        /// Check if there are not error messages in collection.
        /// </summary>
        public static bool IsValid(this IReadOnlyList<ValidationMessage> validationMessages)
        {
            return !validationMessages.Any(vm =>
                vm?.ValidationMessageType is ValidationMessageType.Error or ValidationMessageType.SimpleError);
        } 
        
        /// <summary>
        /// Check if there is at least one warning message in collection.
        /// </summary>
        public static bool HasWarnings(this IReadOnlyList<ValidationMessage> validationMessages)
        {
            return !validationMessages.Any(vm =>
                vm?.ValidationMessageType is ValidationMessageType.Warning or ValidationMessageType.SimpleWarning);
        } 
    }
}