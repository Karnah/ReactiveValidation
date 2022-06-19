using System.Collections.Generic;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Context for revalidation operation.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    internal class AggregatedValidationContext<TObject>
        where TObject : IValidatableObject
    {
        private readonly TObject _validatableObject;
        private readonly IReadOnlyDictionary<string, IStringSource> _displayNamesSources;
        private readonly ValidationCache<TObject> _validationCache;

        /// <summary>
        /// Create new aggregated validation context.
        /// </summary>
        /// <param name="validatableObject">Object which being validating.</param>
        /// <param name="displayNamesSources">Sources of display names.</param>
        public AggregatedValidationContext(TObject validatableObject, IReadOnlyDictionary<string, IStringSource> displayNamesSources)
        {
            _validatableObject = validatableObject;
            _displayNamesSources = displayNamesSources;
            _validationCache = new ValidationCache<TObject>(validatableObject);
        }

        /// <summary>
        /// Create validation context factory for specified property.
        /// </summary>
        public ValidationContextFactory<TObject> CreateContextFactory(string propertyName)
        {
            return new ValidationContextFactory<TObject>(_validatableObject, _validationCache, propertyName, _displayNamesSources[propertyName], _validationCache.GetPropertyValue(propertyName));
        }
    }
}
