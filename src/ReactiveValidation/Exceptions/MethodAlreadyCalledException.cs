using System;

namespace ReactiveValidation.Exceptions
{
    /// <summary>
    /// Exception thrown when one method calls more then once, when it don't allow.
    /// </summary>
    public class MethodAlreadyCalledException : Exception
    {
        /// <inheritdoc />
        public MethodAlreadyCalledException(string message) : base(message)
        { }
    }
}
