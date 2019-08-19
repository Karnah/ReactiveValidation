namespace ReactiveValidation
{
    /// <summary>
    /// Builder which allow create validator for object.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    public interface IObjectValidatorBuilder<in TObject>
        where TObject : IValidatableObject
    {
        /// <summary>
        /// Create validator for specified instance.
        /// </summary>
        /// <param name="instance">Validatable instance.</param>
        /// <returns>Validator of specified instance.</returns>
        IObjectValidator Build(TObject instance);
    }
}
