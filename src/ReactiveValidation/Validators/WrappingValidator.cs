#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ReactiveValidation.Helpers;
using ReactiveValidation.Validators.PropertyValueTransformers;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Wrapper for validators to add common condition and/or value transformer.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    /// <typeparam name="TProp">Type of validatable property.</typeparam>
    internal class WrappingValidator<TObject, TProp> : IPropertyValidator<TObject>
        where TObject : IValidatableObject
    {
        private readonly IValidationCondition<TObject>? _condition;
        private readonly IValueTransformer<TObject, TProp>? _valueTransformer;

        /// <summary>
        /// Create new instance of wrapping validator.
        /// </summary>
        /// <param name="condition">Condition the using inner validator.</param>
        /// <param name="valueTransformer"></param>
        /// <param name="innerValidator">Inner validator.</param>
        public WrappingValidator(
            IValidationCondition<TObject>? condition,
            IValueTransformer<TObject, TProp>? valueTransformer,
            IPropertyValidator<TObject> innerValidator)
        {
            _condition = condition;
            _valueTransformer = valueTransformer;
            
            InnerValidator = innerValidator;
            RelatedProperties = GetUnionRelatedProperties(innerValidator.RelatedProperties, condition?.RelatedProperties);
        }


        /// <summary>
        /// Wrapped validator.
        /// </summary>
        public IPropertyValidator<TObject> InnerValidator { get; }

        /// <inheritdoc />
        public bool IsAsync => InnerValidator.IsAsync;

        /// <inheritdoc />
        public IReadOnlyList<string> RelatedProperties { get; }


        /// <inheritdoc />
        public IReadOnlyList<ValidationMessage> ValidateProperty(ValidationContextFactory<TObject> contextFactory)
        {
            if (IsAsync)
                throw new NotSupportedException();
            
            if (_condition?.ShouldIgnoreValidation(contextFactory) == true)
                return Array.Empty<ValidationMessage>();

            if (_valueTransformer != null)
                contextFactory = GetTransformedContextFactory(contextFactory, _valueTransformer);

            return InnerValidator.ValidateProperty(contextFactory);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<ValidationMessage>> ValidatePropertyAsync(ValidationContextFactory<TObject> contextFactory, CancellationToken cancellationToken)
        {
            if (!IsAsync)
                throw new NotSupportedException();
            
            if (_condition?.ShouldIgnoreValidation(contextFactory) == true)
                return Array.Empty<ValidationMessage>();

            if (_valueTransformer != null)
                contextFactory = GetTransformedContextFactory(contextFactory, _valueTransformer);
            
            return await InnerValidator.ValidatePropertyAsync(contextFactory, cancellationToken);
        }

        /// <inheritdoc />
        public void SetStringSource(IStringSource stringSource)
        {
            throw new NotSupportedException("Wrapping validator doesn't support settings");
        }

        /// <inheritdoc />
        public void ValidateWhen(IValidationCondition<TObject> condition)
        {
            throw new NotSupportedException("Wrapping validator doesn't support settings");
        }

        /// <summary>
        /// Union related properties of inner validator and condition.
        /// </summary>
        /// <param name="validatorRelatedProperties">Related properties of inner validator.</param>
        /// <param name="conditionRelatedProperties">Related properties of condition.</param>
        private IReadOnlyList<string> GetUnionRelatedProperties(IReadOnlyList<string> validatorRelatedProperties, IReadOnlyList<LambdaExpression>? conditionRelatedProperties)
        {
            if (conditionRelatedProperties?.Any() != true)
                return InnerValidator.RelatedProperties;

            var relatedProperties = new HashSet<string>(validatorRelatedProperties);
            foreach (var expression in conditionRelatedProperties)
            {
                var propertyName = ReactiveValidationHelper.GetPropertyName(typeof(TObject), expression);
                if (!string.IsNullOrEmpty(propertyName))
                    relatedProperties.Add(propertyName);
            }

            return relatedProperties.ToList();
        }

        /// <summary>
        /// Get new context factory with transformed property value.
        /// </summary>
        private static ValidationContextFactory<TObject> GetTransformedContextFactory(
            ValidationContextFactory<TObject> contextFactory,
            IValueTransformer<TObject, TProp> valueTransformer)
        {
            var validationContextCache = contextFactory.ValidationContextCache;
            if (!validationContextCache.TryGetValue(valueTransformer, out var transformedPropertyValue))
            {
                transformedPropertyValue = valueTransformer.Transform(contextFactory.ValidatableObject, contextFactory.PropertyValue);
                validationContextCache.SetValue(valueTransformer, transformedPropertyValue);
            }

            return new ValidationContextFactory<TObject>(
                contextFactory.ValidatableObject,
                contextFactory.ValidationContextCache,
                contextFactory.PropertyName,
                contextFactory.DisplayNameSource,
                transformedPropertyValue);
        }
    }
}
