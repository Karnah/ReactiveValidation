using System.Collections.Generic;
using System.Text;

namespace ReactiveValidation
{
    /// <summary>
    /// String source for validation message.
    /// </summary>
    public class ValidationMessageStringSource : IStringSource
    {
        private readonly IStringSource _patternStringSource;
        private readonly Dictionary<string, IStringSource> _arguments;

        /// <summary>
        /// Create new instance of validation message source.
        /// </summary>
        /// <param name="patternStringSource">Source of message patter.</param>
        /// <param name="arguments">Sources of message arguments.</param>
        public ValidationMessageStringSource(IStringSource patternStringSource, Dictionary<string, IStringSource> arguments)
        {
            _patternStringSource = patternStringSource;
            _arguments = arguments;
        }

        /// <inheritdoc />
        public string GetString()
        {
            var messagePattern = new StringBuilder(_patternStringSource.GetString());

            foreach (var messageArgument in _arguments)
            {
                messagePattern = messagePattern.Replace($"{{{messageArgument.Key}}}", messageArgument.Value.GetString());
            }

            return messagePattern.ToString();
        }
    }
}
