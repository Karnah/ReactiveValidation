using System;

namespace ReactiveValidation
{
    /// <summary>
    /// Builder which allow create validator for object.
    /// </summary>
    /// <remarks>
    /// All builders can create validators only for one type.
    /// This interface uses only just because of problems with generic class.
    /// </remarks>
    public interface IObjectValidatorBuilder
    {
        /// <summary>
        /// Type of instance for which can create validator.
        /// </summary>
        Type SupportedType { get; }

        /// <summary>
        /// Create validator for specified instance.
        /// </summary>
        /// <param name="instance">Validatable instance.</param>
        /// <returns>Validator of specified instance.</returns>
        IObjectValidator Build(IValidatableObject instance);
    }
}
