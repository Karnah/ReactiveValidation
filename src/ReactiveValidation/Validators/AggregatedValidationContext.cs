using System.Collections.Generic;
using ReactiveValidation.Helpers;

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
        private readonly IDictionary<string, object> _propertiesValuesCache;

        /// <summary>
        /// Create new aggregated validation context.
        /// </summary>
        /// <param name="validatableObject">Object which being validating.</param>
        /// <param name="displayNamesSources">Sources of display names.</param>
        public AggregatedValidationContext(TObject validatableObject, IReadOnlyDictionary<string, IStringSource> displayNamesSources)
        {
            _validatableObject = validatableObject;
            _displayNamesSources = displayNamesSources;
            _propertiesValuesCache = new Dictionary<string, object>();
        }

        /// <summary>
        /// Create validation context factory for specified property.
        /// </summary>
        public ValidationContextFactory<TObject> CreateContextFactory(string propertyName)
        {
            return new ValidationContextFactory<TObject>(_validatableObject, propertyName, _displayNamesSources[propertyName], GetPropertyValue(propertyName));
        }

        /// <summary>
        /// Get current value of property.
        /// </summary>
        private object GetPropertyValue(string propertyName)
        {
            if (!_propertiesValuesCache.ContainsKey(propertyName))
            {
                var propertyValue = ReactiveValidationHelper.GetPropertyValue<object>(_validatableObject, propertyName);
                _propertiesValuesCache[propertyName] = propertyValue;
            }

            return _propertiesValuesCache[propertyName];
        }
    }
}
