using System;
using ReactiveValidation.Factory;

namespace ReactiveValidation.Exceptions
{
    /// <summary>
    /// Exception thrown when <see cref="IValidatorFactory" /> cannot find builder for specified type.
    /// </summary>
    public class ObjectValidatorBuilderNotFound : Exception
    {
        /// <inheritdoc />
        public ObjectValidatorBuilderNotFound(Type type) : base($"Object validator builder for type {type} not found")
        {
        }
    }
}
