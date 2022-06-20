using System.Collections.Generic;
using System.Linq.Expressions;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Condition of executing of validator.
    /// </summary>
    public interface IValidationCondition<TObject>
        where TObject : IValidatableObject
    {
        /// <summary>
        /// Properties which can affect on state of validatable condition.
        /// </summary>
        IReadOnlyList<LambdaExpression> RelatedProperties { get; }

        /// <summary>
        /// Check if property validator should not execute.
        /// </summary>
        /// <returns>
        /// <see langword="true" />, if validator should not be executed.
        /// <see langword="false" /> otherwise.
        /// </returns>
        /// <remarks>
        /// After first execution class should store value of execution at <see cref="ValidationContextCache" />. 
        /// </remarks>
        bool ShouldIgnoreValidation(ValidationContextFactory<TObject> validationContextFactory);
    }
}