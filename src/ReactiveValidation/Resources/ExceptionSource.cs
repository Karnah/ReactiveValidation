using System;

namespace ReactiveValidation
{
    /// <summary>
    /// String source which return message of exception.
    /// </summary>
    public class ExceptionSource : IStringSource
    {
        private readonly Exception _exception;

        /// <summary>
        /// Create new instance of exception source.
        /// </summary>
        /// <param name="exception">Exception.</param>
        public ExceptionSource(Exception exception)
        {
            _exception = exception;
        }

        /// <inheritdoc />
        public string GetString()
        {
            return _exception.Message;
        }
    }
}
