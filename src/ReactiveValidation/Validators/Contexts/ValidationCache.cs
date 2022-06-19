using System;
using System.Collections.Concurrent;
using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Cache which store property values, result of functions and etc.
    /// </summary>
    public class ValidationCache<TObject>
        where TObject : IValidatableObject
    {
        private readonly TObject _validatableObject;
        private readonly ConcurrentDictionary<string, object> _propertiesValuesCache;
        private readonly ConcurrentDictionary<Func<TObject, bool>, bool> _conditionValuesCache;
        
        public ValidationCache(TObject validatableObject)
        {
            _validatableObject = validatableObject;
            _propertiesValuesCache = new ConcurrentDictionary<string, object>();
            _conditionValuesCache = new ConcurrentDictionary<Func<TObject, bool>, bool>();
        }
        
        /// <summary>
        /// Get current value of property.
        /// </summary>
        public object GetPropertyValue(string propertyName)
        {
            if (!_propertiesValuesCache.TryGetValue(propertyName, out var propertyValue))
            {
                propertyValue = ReactiveValidationHelper.GetPropertyValue<object>(_validatableObject, propertyName);
                _propertiesValuesCache.TryAdd(propertyName, propertyValue);
            }

            return propertyValue;
        }

        /// <summary>
        /// Get result value of executing condition.
        /// </summary>
        public bool GetConditionValue(Func<TObject, bool> condition)
        {
            if (!_conditionValuesCache.TryGetValue(condition, out var conditionValue))
            {
                conditionValue = condition.Invoke(_validatableObject);
                _conditionValuesCache.TryAdd(condition, conditionValue);
            }

            return conditionValue;
        }
    }
}