using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Wrap for validators which doesn't support <see cref="IPropertyValidatorSettings{TObject}" />.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    public class WrappingValidator<TObject> : IPropertyValidator<TObject>
        where TObject : IValidatableObject
    {
        private readonly ValidationCondition<TObject> _condition;

        /// <summary>
        /// Create new instance of wrapping validator.
        /// </summary>
        /// <param name="condition">Condition the using inner validator.</param>
        /// <param name="innerValidator">Inner validator.</param>
        public WrappingValidator(
            ValidationCondition<TObject> condition,
            IPropertyValidator<TObject> innerValidator)
        {
            _condition = condition;
            InnerValidator = innerValidator;

            UnionRelatedProperties(condition.RelatedProperties);
        }


        /// <summary>
        /// Wrapped validator.
        /// </summary>
        public IPropertyValidator<TObject> InnerValidator { get; }

        /// <inheritdoc />
        public bool IsAsync => InnerValidator.IsAsync;

        /// <inheritdoc />
        public IReadOnlyList<string> RelatedProperties { get; private set; }


        /// <inheritdoc />
        public IReadOnlyList<ValidationMessage> ValidateProperty(ValidationContextFactory<TObject> contextFactory)
        {
            if (IsAsync)
                throw new NotSupportedException();
            
            if (_condition.ShouldIgnoreValidation(contextFactory.ValidationCache))
                return Array.Empty<ValidationMessage>();

            return InnerValidator.ValidateProperty(contextFactory);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<ValidationMessage>> ValidatePropertyAsync(ValidationContextFactory<TObject> contextFactory, CancellationToken cancellationToken)
        {
            if (!IsAsync)
                throw new NotSupportedException();
            
            if (_condition.ShouldIgnoreValidation(contextFactory.ValidationCache))
                return Array.Empty<ValidationMessage>();

            return await InnerValidator.ValidatePropertyAsync(contextFactory, cancellationToken);
        }

        /// <summary>
        /// Union related properties of inner validator and condition.
        /// </summary>
        /// <param name="conditionRelatedProperties">Related properties of condition.</param>
        private void UnionRelatedProperties(IReadOnlyList<LambdaExpression> conditionRelatedProperties)
        {
            if (conditionRelatedProperties?.Any() != true)
            {
                RelatedProperties = InnerValidator.RelatedProperties;
                return;
            }

            var relatedProperties = new HashSet<string>(InnerValidator.RelatedProperties);
            foreach (var expression in conditionRelatedProperties)
            {
                var propertyName = ReactiveValidationHelper.GetPropertyName(typeof(TObject), expression);
                if (!string.IsNullOrEmpty(propertyName))
                    relatedProperties.Add(propertyName);
            }

            RelatedProperties = relatedProperties.ToList();
        }
    }
}
