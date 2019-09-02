using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using ReactiveValidation.Extensions;
using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Base class of validators for property value.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    /// <typeparam name="TProp">Type of validatable property.</typeparam>
    public abstract class PropertyValidator<TObject, TProp> : IPropertyValidator<TObject>, IPropertyValidatorSettings<TObject>
        where TObject : IValidatableObject
    {
        private readonly ValidationMessageType _validationMessageType;
        private readonly IStringSource _stringSource;

        private Func<TObject, bool> _condition;
        private IStringSource _overridenStringSource;

        /// <summary>
        /// Create new validator for property value.
        /// </summary>
        /// <param name="stringSource">Source for validation message.</param>
        /// <param name="validationMessageType">Type of validation message.</param>
        /// <param name="relatedProperties">Properties which can affect on state of validatable property.</param>
        protected PropertyValidator(IStringSource stringSource, ValidationMessageType validationMessageType, params LambdaExpression[] relatedProperties)
        {
            _stringSource = stringSource;
            _validationMessageType = validationMessageType;

            RelatedProperties = GetRelatedProperties(relatedProperties);
        }


        /// <inheritdoc />
        public IReadOnlyList<string> RelatedProperties { get; }


        /// <inheritdoc />
        public void SetStringSource(IStringSource stringSource)
        {
            _overridenStringSource.GuardNotCallTwice($"Methods 'WithMessage'/'WithLocalizedMessage' already have been called for {this.GetType()}");
            _overridenStringSource = stringSource;
        }

        /// <inheritdoc />
        public void ValidateWhen(Func<TObject, bool> condition, params LambdaExpression[] relatedProperties)
        {
            _condition.GuardNotCallTwice($"Method 'When' already have been called for {this.GetType()}");
            _condition = condition;

            GetRelatedProperties(relatedProperties);
        }


        /// <inheritdoc />
        public IReadOnlyList<ValidationMessage> ValidateProperty(ValidationContextFactory<TObject> contextFactory)
        {
            if (_condition != null && !_condition.Invoke(contextFactory.ValidatableObject))
                return new ValidationMessage[0];

            var context = contextFactory.CreateContext<TProp>();
            if (IsValid(context))
                return new ValidationMessage[0];

            var messageSource = context.GetMessageSource(_overridenStringSource ?? _stringSource);
            var validationMessage = new ValidationMessage(messageSource, _validationMessageType);
            return new []{ validationMessage };
        }

        /// <summary>
        /// Check if property is valid.
        /// </summary>
        /// <param name="context">Validation context.</param>
        /// <returns>
        /// <see langword="true" />, if property are valid.
        /// <see langword="false" /> otherwise.
        /// </returns>
        protected abstract bool IsValid(ValidationContext<TObject, TProp> context);

        /// <summary>
        /// Get names of related properties.
        /// </summary>
        private static IReadOnlyList<string> GetRelatedProperties(LambdaExpression[] relatedPropertiesExpressions)
        {
            if (relatedPropertiesExpressions?.Any() != true)
                return new string[0];

            var relatedProperties = new HashSet<string>();
            foreach (var expression in relatedPropertiesExpressions)
            {
                var propertyName = ReactiveValidationHelper.GetPropertyName(typeof(TObject), expression);
                if (string.IsNullOrEmpty(propertyName) == false)
                    relatedProperties.Add(propertyName);
            }

            return relatedProperties.ToList();
        }
    }
}
