namespace ReactiveValidation
{
    /// <summary>
    /// The type of validation message.
    /// </summary>
    public enum ValidationMessageType
    {
        /// <summary>
        /// Used when property has invalid value.
        /// </summary>
        Error,

        /// <summary>
        /// A simple error (for example, an empty field), which should be shown only when mouse over the control.
        /// </summary>
        SimpleError,

        /// <summary>
        /// Allows show message, but don't broke validation.
        /// </summary>
        Warning,

        /// <summary>
        /// Allows show message only if mouse over the control and don't broke validation.
        /// </summary>
        SimpleWarning
    }
}
