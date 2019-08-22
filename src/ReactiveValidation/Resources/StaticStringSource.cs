namespace ReactiveValidation
{
    /// <summary>
    /// String source which always return one string.
    /// </summary>
    public class StaticStringSource : IStringSource
    {
        private readonly string _message;

        /// <summary>
        /// Create new instance of static string source.
        /// </summary>
        /// <param name="message">Message.</param>
        public StaticStringSource(string message)
        {
            _message = message;
        }

        /// <inheritdoc />
        public string GetString()
        {
            return _message;
        }
    }
}
