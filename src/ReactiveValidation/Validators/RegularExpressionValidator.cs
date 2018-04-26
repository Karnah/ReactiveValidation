using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public class RegularExpressionValidator<TObject> : PropertyValidator<TObject, string>
        where TObject : IValidatableObject
    {
        private readonly ParameterInfo<TObject, string> _regexPattern;
        private readonly RegexOptions? _regexOptions;

        public RegularExpressionValidator(
            Expression<Func<TObject, string>> patternExpression,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.RegularExpressionValidator), validationMessageType)
        {
            _regexPattern = patternExpression.GetParameterInfo();
        }

        public RegularExpressionValidator(
            Expression<Func<TObject, string>> patternExpression,
            RegexOptions regexOptions,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.RegularExpressionValidator), validationMessageType)
        {
            _regexPattern = patternExpression.GetParameterInfo();
            _regexOptions = regexOptions;
        }


        protected override bool IsValid(ValidationContext<TObject, string> context)
        {
            if (string.IsNullOrEmpty(context.PropertyValue) == true)
                return true;

            var regexPatternValue = context.GetParamValue(_regexPattern);
            if (string.IsNullOrEmpty(regexPatternValue) == true)
                return true;

            var regex = _regexOptions == null
                ? new Regex(regexPatternValue)
                : new Regex(regexPatternValue, _regexOptions.Value);
            if (regex.IsMatch(context.PropertyValue) == false) {
                context.RegisterMessageArgument("RegexPattern", _regexPattern, regexPatternValue);

                return false;
            }

            return true;
        }
    }
}
