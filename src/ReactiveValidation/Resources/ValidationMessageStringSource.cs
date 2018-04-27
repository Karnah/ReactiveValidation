using System.Collections.Generic;
using System.Text;

namespace ReactiveValidation
{
    public class ValidationMessageStringSource : IStringSource
    {
        private readonly IStringSource _patternStringSource;
        private readonly Dictionary<string, IStringSource> _arguments;

        public ValidationMessageStringSource(IStringSource patternStringSource, Dictionary<string, IStringSource> arguments)
        {
            _patternStringSource = patternStringSource;
            _arguments = arguments;
        }


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
