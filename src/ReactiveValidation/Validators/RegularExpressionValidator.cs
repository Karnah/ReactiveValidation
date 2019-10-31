using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Validator which checks property with regular expression.
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    public class RegularExpressionValidator<TObject> : PropertyValidator<TObject, string>
        where TObject : IValidatableObject
    {
        private readonly ValidatorParameter<TObject, string> _regexPattern;
        private readonly RegexOptions? _regexOptions;

        /// <summary>
        /// Initialize a new instance of <see cref="RegularExpressionValidator{TObject}" /> class.
        /// </summary>
        /// <param name="patternExpression">Expression of pattern.</param>
        /// <param name="validationMessageType">The type validatable message.</param>
        public RegularExpressionValidator(
            Expression<Func<TObject, string>> patternExpression,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.RegularExpressionValidator), validationMessageType)
        {
            _regexPattern = new ValidatorParameter<TObject, string>(patternExpression);
        }

        /// <summary>
        /// Initialize a new instance of <see cref="RegularExpressionValidator{TObject}" /> class with regex options.
        /// </summary>
        /// <param name="patternExpression">Expression of pattern.</param>
        /// <param name="regexOptions">Regex options.</param>
        /// <param name="validationMessageType">The type validatable message.</param>
        public RegularExpressionValidator(
            Expression<Func<TObject, string>> patternExpression,
            RegexOptions regexOptions,
            ValidationMessageType validationMessageType)
            : base(new LanguageStringSource(ValidatorsNames.RegularExpressionValidator), validationMessageType)
        {
            _regexPattern = new ValidatorParameter<TObject, string>(patternExpression);
            _regexOptions = regexOptions;
        }


        /// <inheritdoc />
        protected override bool IsValid(ValidationContext<TObject, string> context)
        {
            if (string.IsNullOrEmpty(context.PropertyValue))
                return true;

            var regexPatternValue = context.GetParamValue(_regexPattern);
            if (string.IsNullOrEmpty(regexPatternValue))
                return true;

            var regex = _regexOptions == null
                ? new Regex(regexPatternValue)
                : new Regex(regexPatternValue, _regexOptions.Value);
            if (regex.IsMatch(context.PropertyValue) == false)
            {
                context.RegisterMessageArgument("RegexPattern", _regexPattern, regexPatternValue);
                return false;
            }

            return true;
        }
    }
}