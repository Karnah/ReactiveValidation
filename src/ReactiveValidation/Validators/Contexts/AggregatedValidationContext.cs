using System.Collections.Generic;
using ReactiveValidation.Helpers;
using ReactiveValidation.Resources.StringSources;
using ReactiveValidation.ValidatorFactory;

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
        private readonly IReadOnlyDictionary<string, IStringSource?> _displayNamesSources;
        private readonly ValidationContextCache _validationContextCache;
        private readonly IReadOnlyDictionary<string, PropertyChangedStopwatch> _propertyChangedStopwatches;

        /// <summary>
        /// Create new aggregated validation context.
        /// </summary>
        /// <param name="validatableObject">Object which being validating.</param>
        /// <param name="displayNamesSources">Sources of display names.</param>
        /// <param name="propertyChangedStopwatches">Stopwatches for property changed event.</param>
        public AggregatedValidationContext(TObject validatableObject, IReadOnlyDictionary<string, IStringSource?> displayNamesSources, IReadOnlyDictionary<string, PropertyChangedStopwatch> propertyChangedStopwatches)
        {
            _validatableObject = validatableObject;
            _displayNamesSources = displayNamesSources;
            _propertyChangedStopwatches = propertyChangedStopwatches;
            _validationContextCache = new ValidationContextCache();
        }

        /// <summary>
        /// Create validation context factory for specified property.
        /// </summary>
        public ValidationContextFactory<TObject> CreateContextFactory(string propertyName)
        {
            return new ValidationContextFactory<TObject>(_validatableObject, _validationContextCache, _propertyChangedStopwatches, propertyName, _displayNamesSources[propertyName], GetPropertyValue(propertyName));
        }

        /// <summary>
        /// Get property value.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Property value.</returns>
        private object? GetPropertyValue(string propertyName)
        {
            if (!_validationContextCache.TryGetPropertyValue(propertyName, out var propertyValue))
            {
                propertyValue = ReactiveValidationHelper.GetPropertyValue<object>(_validatableObject, propertyName);
                _validationContextCache.SetPropertyValue(propertyName, propertyValue);
            }

            return propertyValue;
        }
    }
}
