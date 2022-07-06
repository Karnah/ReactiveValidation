using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ReactiveValidation.Validators
{
    /// <inheritdoc />
    public class ValidationCondition<TObject> : IValidationCondition<TObject>
        where TObject : IValidatableObject
    {
        private readonly Func<TObject, bool> _conditionFunc;
        
        /// <summary>
        /// Validation will be executed only if the condition is <see langword="true" />.
        /// </summary>
        /// <param name="conditionFunc">Condition.</param>
        /// <param name="relatedProperties">Properties which can affect on state of validatable condition.</param>
        public ValidationCondition(Func<TObject, bool> conditionFunc, params LambdaExpression[] relatedProperties)
        {
            _conditionFunc = conditionFunc;
            RelatedProperties = relatedProperties;
        }

        
        /// <inheritdoc />
        public IReadOnlyList<LambdaExpression> RelatedProperties { get; }

        
        /// <inheritdoc />
        public bool ShouldIgnoreValidation(ValidationContextFactory<TObject> validationContextFactory)
        {
            var validationCache = validationContextFactory.ValidationContextCache;
            if (validationCache.TryGetValue(this, out var shouldIgnoreObject))
                return (bool)shouldIgnoreObject;

            var shouldIgnore = !_conditionFunc.Invoke(validationContextFactory.ValidatableObject);
            validationCache.SetValue(this, shouldIgnore);
            return shouldIgnore;
        }
    }
}