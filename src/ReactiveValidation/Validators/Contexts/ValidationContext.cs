using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Validation context.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    /// <typeparam name="TProp">Type of validatable property.</typeparam>
    public class ValidationContext<TObject, TProp>
        where TObject : IValidatableObject
    {
        private readonly Dictionary<string, IStringSource> _messageArguments;

        /// <summary>
        /// Create new instance of validation context.
        /// </summary>
        /// <param name="validatableObject">Instance of validatable object.</param>
        /// <param name="validationContextCache">Cache which store property values, result of functions and etc.</param>
        /// <param name="propertyName">Name of validatable property.</param>
        /// <param name="displayPropertySource">Source of display name of validatable property.</param>
        /// <param name="propertyValue">Value of validatable property.</param>
        public ValidationContext(TObject validatableObject, ValidationContextCache validationContextCache, string propertyName, IStringSource? displayPropertySource, TProp propertyValue)
        {
            ValidatableObject = validatableObject;
            ValidationContextCache = validationContextCache;
            PropertyName = propertyName;
            DisplayPropertySource = displayPropertySource;
            PropertyValue = propertyValue;

            _messageArguments = new Dictionary<string, IStringSource>
            {
                { nameof(PropertyName), DisplayPropertySource ?? new StaticStringSource(propertyName) }
            };
        }

        /// <summary>
        /// Create new instance of validation context.
        /// </summary>
        /// <param name="parentContext">Parent validation context.</param>
        public ValidationContext(ValidationContext<TObject, TProp> parentContext)
            : this(parentContext.ValidatableObject, parentContext.ValidationContextCache, parentContext.PropertyName, parentContext.DisplayPropertySource, parentContext.PropertyValue)
        {
        }


        /// <summary>
        /// Instance of validatable object.
        /// </summary>
        public TObject ValidatableObject { get; }

        /// <summary>
        /// Cache which store property values, result of functions and etc.
        /// </summary>
        public ValidationContextCache ValidationContextCache { get; }

        /// <summary>
        /// Name of validatable property.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Source of display name of validatable property.
        /// </summary>
        public IStringSource? DisplayPropertySource { get; }

        /// <summary>
        /// Value of validatable property.
        /// </summary>
        public TProp PropertyValue { get; }


        /// <summary>
        /// Get value of validator parameter.
        /// </summary>
        /// <typeparam name="TParam">Type of parameter.</typeparam>
        /// <param name="validatorParameter">Validator parameter.</param>
        public TParam GetParamValue<TParam>(ValidatorParameter<TObject, TParam> validatorParameter)
        {
            var value = validatorParameter.FuncValue.Invoke(ValidatableObject);
            return value;
        }

        /// <summary>
        /// Register parameter with placeholder.
        /// </summary>
        /// <typeparam name="TParam">Type of parameter.</typeparam>
        /// <param name="placeholder">Placeholder in base source.</param>
        /// <param name="validatorParameter">Validator parameter.</param>
        /// <param name="paramValue">Validator value.</param>
        public void RegisterMessageArgument<TParam>(string placeholder, ValidatorParameter<TObject, TParam>? validatorParameter, TParam paramValue)
        {
            if (validatorParameter?.DisplayNameSource != null)
            {
                _messageArguments.Add(placeholder, validatorParameter.DisplayNameSource);
            }
            else
            {
                var stringSource = new StaticStringSource(validatorParameter?.Name ?? paramValue?.ToString() ?? string.Empty);
                _messageArguments.Add(placeholder, stringSource);
            }
        }

        /// <summary>
        /// Get source with arguments.
        /// </summary>
        /// <param name="stringSource">Base message source.</param>
        public IStringSource GetMessageSource(IStringSource stringSource)
        {
            return new ValidationMessageStringSource(stringSource, _messageArguments);
        }
    }
}