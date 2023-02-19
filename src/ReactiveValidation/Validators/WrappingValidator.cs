using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ReactiveValidation.Helpers;
using ReactiveValidation.Resources.StringSources;
using ReactiveValidation.Validators.Conditions;
using ReactiveValidation.Validators.PropertyValueTransformers;
using ReactiveValidation.Validators.Throttle;

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
        private readonly IPropertiesThrottle? _throttle;

        /// <summary>
        /// Create instance of <see cref="WrappingValidator{TObject,TProp}" /> class.
        /// </summary>
        /// <param name="innerValidator">Inner validator.</param>
        /// <param name="condition">Condition the using inner validator.</param>
        /// <param name="valueTransformer">Property value transformer.</param>
        /// <param name="throttle">The properties throttle.</param>
        public WrappingValidator(
            IPropertyValidator<TObject> innerValidator,
            IValidationCondition<TObject>? condition = null,
            IValueTransformer<TObject, TProp>? valueTransformer = null,
            IPropertiesThrottle? throttle = null)
        {
            _condition = condition;
            _valueTransformer = valueTransformer;
            _throttle = throttle;

            InnerValidator = innerValidator;
            RelatedProperties = GetUnionRelatedProperties(innerValidator.RelatedProperties, condition?.RelatedProperties);
        }


        /// <summary>
        /// Wrapped validator.
        /// </summary>
        public IPropertyValidator<TObject> InnerValidator { get; }

        /// <inheritdoc />
        /// <remarks>
        /// Sync validator becomes async when there are throttles.
        /// </remarks>
        public bool IsAsync => InnerValidator.IsAsync || _throttle != null;

        /// <inheritdoc />
        public IReadOnlyList<string> RelatedProperties { get; }


        /// <inheritdoc />
        public IReadOnlyList<ValidationMessage> ValidateProperty(ValidationContextFactory<TObject> contextFactory)
        {
            if (IsAsync)
                throw new NotSupportedException();

            if (_valueTransformer != null)
                contextFactory = contextFactory.GetTransformedContextFactory(_valueTransformer);

            if (_condition != null)
                contextFactory.RegisterValidationCondition(_condition);

            return InnerValidator.ValidateProperty(contextFactory);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<ValidationMessage>> ValidatePropertyAsync(ValidationContextFactory<TObject> contextFactory, CancellationToken cancellationToken)
        {
            if (!IsAsync)
                throw new NotSupportedException();
            
            if (_valueTransformer != null)
                contextFactory = contextFactory.GetTransformedContextFactory(_valueTransformer);
            
            if (_condition != null)
                contextFactory.RegisterValidationCondition(_condition);
            if (_throttle != null)
                contextFactory.RegisterPropertiesThrottle(_throttle);
            
            return await InnerValidator.ValidatePropertyAsync(contextFactory, cancellationToken);
        }

        /// <inheritdoc />
        public void SetStringSource(IStringSource stringSource)
        {
            throw new NotSupportedException($"{nameof(WrappingValidator<TObject, TProp>)} doesn't support settings");
        }

        /// <inheritdoc />
        public void ValidateWhen(IValidationCondition<TObject> condition)
        {
            throw new NotSupportedException($"{nameof(WrappingValidator<TObject, TProp>)} doesn't support settings");
        }

        /// <inheritdoc />
        public void Throttle(IPropertiesThrottle propertiesThrottle)
        {
            throw new NotSupportedException($"{nameof(WrappingValidator<TObject, TProp>)} doesn't support settings");
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
                    relatedProperties.Add(propertyName!);
            }

            return relatedProperties.ToList();
        }
    }
}
