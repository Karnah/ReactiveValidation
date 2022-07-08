using System;
using ReactiveValidation.ValidatorFactory;

namespace ReactiveValidation.Exceptions
{
    /// <summary>
    /// Exception thrown when <see cref="IValidatorFactory" /> cannot find builder for specified type.
    /// </summary>
    public class ObjectValidatorBuilderNotFound : Exception
    {
        /// <summary>
        /// Initialize a new instance of <see cref="ObjectValidatorBuilderNotFound" /> class.
        /// </summary>
        public ObjectValidatorBuilderNotFound(Type type) : base($"Object validator builder for type {type} not found")
        {
        }
    }
}
