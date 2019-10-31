using System;
using ReactiveValidation.Factory;

namespace ReactiveValidation.Exceptions
{
    /// <summary>
    /// Exception thrown when builder already registered in <see cref="IValidatorFactory" />.
    /// </summary>
    public class ObjectValidatorBuilderAlreadyRegistered : Exception
    {
        /// <summary>
        /// Initialize a new instance of <see cref="ObjectValidatorBuilderAlreadyRegistered" /> class.
        /// </summary>
        public ObjectValidatorBuilderAlreadyRegistered(Type type) : base($"Object validator builder already registered for type {type}")
        {
        }
    }
}
