using System;
using System.Linq.Expressions;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Additional settings for property validator.
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    public interface IPropertyValidatorSettings<out TObject>
    {
        /// <summary>
        /// Change the string source for validatable messages.
        /// </summary>
        /// <param name="stringSource">New string source.</param>
        void SetStringSource(IStringSource stringSource);

        /// <summary>
        /// Validate property only if <paramref name="condition" /> is <see langword="true" />.
        /// Property always valid if condition is <see langword="false" />.
        /// </summary>
        /// <param name="condition">Condition.</param>
        /// <param name="relatedProperties">Properties which uses in condition.</param>
        void ValidateWhen(Func<TObject, bool> condition, params LambdaExpression[] relatedProperties);
    }
}
