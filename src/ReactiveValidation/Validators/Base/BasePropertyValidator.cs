using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ReactiveValidation.Extensions;
using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Base class of validators for property value.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    /// <typeparam name="TProp">Type of validatable property.</typeparam>
    public abstract class BasePropertyValidator<TObject, TProp> : IPropertyValidator<TObject>
        where TObject : IValidatableObject
    {
        private readonly ValidationMessageType _validationMessageType;
        private readonly IStringSource _stringSource;

        private IValidationCondition<TObject>? _condition;
        private IStringSource? _overriddenStringSource;

        /// <summary>
        /// Create new validator for property value.
        /// </summary>
        /// <param name="stringSource">Source for validation message.</param>
        /// <param name="validationMessageType">Type of validation message.</param>
        /// <param name="relatedProperties">Properties which can affect on state of validatable property.</param>
        protected BasePropertyValidator(IStringSource stringSource, ValidationMessageType validationMessageType, params LambdaExpression[] relatedProperties)
        {
            _stringSource = stringSource;
            _validationMessageType = validationMessageType;

            RelatedProperties = GetUnionRelatedProperties(relatedProperties);
        }


        /// <inheritdoc />
        public abstract bool IsAsync { get; }

        /// <inheritdoc />
        public IReadOnlyList<string> RelatedProperties { get; private set; }

        
        /// <inheritdoc />
        public void SetStringSource(IStringSource stringSource)
        {
            _overriddenStringSource.GuardNotCallTwice($"Methods 'WithMessage'/'WithLocalizedMessage' already have been called for {this.GetType()}");
            _overriddenStringSource = stringSource;
        }

        /// <inheritdoc />
        public void ValidateWhen(IValidationCondition<TObject> condition)
        {
            _condition.GuardNotCallTwice($"Method 'When' already have been called for {this.GetType()}");
            _condition = condition;

            RelatedProperties = GetUnionRelatedProperties(condition.RelatedProperties);
        }


        /// <inheritdoc />
        public abstract IReadOnlyList<ValidationMessage> ValidateProperty(
            ValidationContextFactory<TObject> contextFactory);

        /// <inheritdoc />
        public abstract Task<IReadOnlyList<ValidationMessage>> ValidatePropertyAsync(
            ValidationContextFactory<TObject> contextFactory, CancellationToken cancellationToken);


        /// <summary>
        /// Check that validation should be ignored.
        /// </summary>
        protected virtual bool CheckIgnoreValidation(ValidationContextFactory<TObject> validationContextFactory)
        {
            return _condition?.ShouldIgnoreValidation(validationContextFactory) == true;
        }

        /// <summary>
        /// Get validation message(s);
        /// </summary>
        protected virtual IReadOnlyList<ValidationMessage> GetValidationMessages(ValidationContext<TObject, TProp> context)
        {
            var messageSource = context.GetMessageSource(_overriddenStringSource ?? _stringSource);
            var validationMessage = new ValidationMessage(messageSource, _validationMessageType);
            return new []{ validationMessage };
        }


        /// <summary>
        /// Get names of related properties.
        /// </summary>
        private IReadOnlyList<string> GetUnionRelatedProperties(IReadOnlyList<LambdaExpression> relatedPropertiesExpressions)
        {
            // Because this method call from ctor, RelatedProperties can be null at this moment.
            // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
            var existingRelatedProperties = RelatedProperties ?? Array.Empty<string>();
            
            if (relatedPropertiesExpressions.Any() != true)
                return existingRelatedProperties;

            var relatedProperties = new HashSet<string>(existingRelatedProperties);
            foreach (var expression in relatedPropertiesExpressions)
            {
                var propertyName = ReactiveValidationHelper.GetPropertyName(typeof(TObject), expression);
                if (!string.IsNullOrEmpty(propertyName))
                    relatedProperties.Add(propertyName!);
            }

            return relatedProperties.ToList();
        }
    }
}
