using System.Collections.Concurrent;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Cache which store property values, result of functions and etc during validation context.
    /// </summary>
    public class ValidationContextCache
    {
        private readonly ConcurrentDictionary<string, object> _propertiesValuesCache;
        private readonly ConcurrentDictionary<object, object> _objectValuesCache;
        
        /// <summary>
        /// Create new instance of <see cref="ValidationContextCache" />.
        /// </summary>
        public ValidationContextCache()
        {
            _propertiesValuesCache = new ConcurrentDictionary<string, object>();
            _objectValuesCache = new ConcurrentDictionary<object, object>();
        }
        
        /// <summary>
        /// Try get value of property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyValue">Property value.</param>
        /// <returns>
        /// <see langword="true" />, if value has fonded in cache.
        /// <see langword="false" /> if there is not value for this property in cache.
        /// </returns>
        public bool TryGetPropertyValue(string propertyName, out object propertyValue)
        {
            return _propertiesValuesCache.TryGetValue(propertyName, out propertyValue);
        }

        /// <summary>
        /// Set property value to cache.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyValue">Property value.</param>
        public void SetPropertyValue(string propertyName, object propertyValue)
        {
            _propertiesValuesCache.TryAdd(propertyName, propertyValue);
        }

        /// <summary>
        /// Try get value value from cache by key.
        /// </summary>
        /// <param name="keyObject">Key.</param>
        /// <param name="value">Value.</param>
        /// <returns>
        /// <see langword="true" />, if value has fonded in cache.
        /// <see langword="false" /> if there is not value for this key in cache.
        /// </returns>
        public bool TryGetValue(object keyObject, out object value)
        {
            return _objectValuesCache.TryGetValue(keyObject, out value);
        }

        /// <summary>
        /// Set value in cache by key.
        /// </summary>
        /// <param name="keyObject">Key.</param>
        /// <param name="value">Value.</param>
        public void SetValue(object keyObject, object value)
        {
            _objectValuesCache.TryAdd(keyObject, value);
        }
    }
}