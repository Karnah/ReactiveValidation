using System;

namespace ReactiveValidation.Exceptions
{
    /// <summary>
    /// Exception thrown when one method calls more then once, when it don't allow.
    /// </summary>
    public class MethodAlreadyCalledException : Exception
    {
        /// <summary>
        /// Initialize a new instance of <see cref="MethodAlreadyCalledException" /> class.
        /// </summary>
        public MethodAlreadyCalledException(string message) : base(message)
        { }
    }
}
