using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Condition of executing for property validators.
    /// </summary>
    public class ValidationCondition<TObject>
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

        /// <summary>
        /// Properties which can affect on state of validatable condition.
        /// </summary>
        public IReadOnlyList<LambdaExpression> RelatedProperties { get; }

        /// <summary>
        /// Check if property validator should not execute.
        /// </summary>
        /// <returns>
        /// <see langword="true" />, if validator should not be executed.
        /// <see langword="false" /> otherwise.
        /// </returns>
        public bool ShouldIgnoreValidation(ValidationCache<TObject> contextFactory)
        {
            return !contextFactory.GetConditionValue(_conditionFunc);
        }
    }
}