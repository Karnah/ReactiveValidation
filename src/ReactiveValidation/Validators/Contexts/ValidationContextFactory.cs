using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReactiveValidation.Resources.StringSources;
using ReactiveValidation.ValidatorFactory;
using ReactiveValidation.Validators.Conditions;
using ReactiveValidation.Validators.PropertyValueTransformers;
using ReactiveValidation.Validators.Throttle;

namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Factory for creating validation context.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable property.</typeparam>
    public class ValidationContextFactory<TObject>
        where TObject : IValidatableObject
    {
        private readonly List<IValidationCondition<TObject>> _conditions = new();
        private readonly List<IPropertiesThrottle> _throttles = new();

        /// <summary>
        /// Create new validation context factory.
        /// </summary>
        /// <param name="validatableObject">Object which being validating.</param>
        /// <param name="validationContextCache">Cache which store property values, result of functions and etc.</param>
        /// <param name="propertyChangedStopwatches">Stopwatches for property changed event.</param>
        /// <param name="propertyName">Name of property which being validating.</param>
        /// <param name="displayNameSource">Display name of validatable property.</param>
        /// <param name="propertyValue">Value of property  which being validating.</param>
        internal ValidationContextFactory(
            TObject validatableObject,
            ValidationContextCache validationContextCache,
            IReadOnlyDictionary<string, PropertyChangedStopwatch> propertyChangedStopwatches,
            string propertyName,
            IStringSource? displayNameSource,
            object? propertyValue)
        {
            ValidatableObject = validatableObject;
            PropertyName = propertyName;
            DisplayNameSource = displayNameSource;
            PropertyValue = propertyValue;
            ValidationContextCache = validationContextCache;
            PropertyChangedStopwatches = propertyChangedStopwatches;
        }

        /// <summary>
        /// Create new validation context factory.
        /// </summary>
        /// <param name="validatableObject">Object which being validating.</param>
        /// <param name="validationContextCache">Cache which store property values, result of functions and etc.</param>
        /// <param name="propertyChangedStopwatches">Stopwatches for property changed event.</param>
        /// <param name="conditions">Validations conditions.</param>
        /// <param name="throttles">Properties throttles.</param>
        /// <param name="propertyName">Name of property which being validating.</param>
        /// <param name="displayNameSource">Display name of validatable property.</param>
        /// <param name="propertyValue">Value of property  which being validating.</param>
        private ValidationContextFactory(
            TObject validatableObject,
            ValidationContextCache validationContextCache,
            IReadOnlyDictionary<string, PropertyChangedStopwatch> propertyChangedStopwatches,
            List<IValidationCondition<TObject>> conditions,
            List<IPropertiesThrottle> throttles,
            string propertyName,
            IStringSource? displayNameSource,
            object? propertyValue)
        {
            _conditions = conditions;
            _throttles = throttles;

            ValidatableObject = validatableObject;
            PropertyName = propertyName;
            DisplayNameSource = displayNameSource;
            PropertyValue = propertyValue;
            ValidationContextCache = validationContextCache;
            PropertyChangedStopwatches = propertyChangedStopwatches;
        }

        /// <summary>
        /// Object which being validating.
        /// </summary>
        public TObject ValidatableObject { get; }

        /// <summary>
        /// Name of property which being validating.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Display name of validatable property.
        /// </summary>
        public IStringSource? DisplayNameSource { get; }

        /// <summary>
        /// Value of property  which being validating.
        /// </summary>
        public object? PropertyValue { get; }
        
        /// <summary>
        /// Cache which store property values, result of functions and etc.
        /// </summary>
        public ValidationContextCache ValidationContextCache { get; }

        /// <summary>
        /// Stopwatches for property changed event.
        /// </summary>
        public IReadOnlyDictionary<string, PropertyChangedStopwatch> PropertyChangedStopwatches { get; }

        /// <summary>
        /// Create context for validating property.
        /// </summary>
        /// <typeparam name="TProp">Type of validatable property.</typeparam>
        public ValidationContext<TObject, TProp> CreateContext<TProp>()
        {
            return new ValidationContext<TObject, TProp>(ValidatableObject, ValidationContextCache, PropertyName, DisplayNameSource, (TProp) PropertyValue!);
        }

        /// <summary>
        /// Create new validation context factory with transformed property value.
        /// </summary>
        /// <typeparam name="TProp">The new type of property value.</typeparam>
        /// <param name="valueTransformer">Property value transformer.</param>
        /// <returns>New validation context factory with transformed property value.</returns>
        public ValidationContextFactory<TObject> GetTransformedContextFactory<TProp>(IValueTransformer<TObject, TProp> valueTransformer)
        {
            if (!ValidationContextCache.TryGetValue(valueTransformer, out var transformedPropertyValue))
            {
                transformedPropertyValue = valueTransformer.Transform(ValidatableObject, PropertyValue);
                ValidationContextCache.SetValue(valueTransformer, transformedPropertyValue);
            }

            return new ValidationContextFactory<TObject>(
                ValidatableObject,
                ValidationContextCache,
                PropertyChangedStopwatches,
                _conditions,
                _throttles,
                PropertyName,
                DisplayNameSource,
                transformedPropertyValue);
        }

        /// <summary>
        /// Register validation condition which must check before validation.
        /// </summary>
        public void RegisterValidationCondition(IValidationCondition<TObject> condition)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            _conditions.Add(condition);
        }

        /// <summary>
        /// Register properties throttle which must execute before validation.
        /// </summary>
        public void RegisterPropertiesThrottle(IPropertiesThrottle throttle)
        {
            if (throttle == null)
                throw new ArgumentNullException(nameof(throttle));

            _throttles.Add(throttle);
        }

        /// <summary>
        /// Check if property validator should not execute.
        /// </summary>
        public bool ShouldIgnoreValidation()
        {
            foreach (var condition in _conditions)
            {
                if (condition.ShouldIgnoreValidation(this))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Execute delay because of throttle.
        /// </summary>
        public async Task ThrottleAsync(CancellationToken cancellationToken)
        {
            foreach (var throttle in _throttles)
            {
                await throttle
                    .DelayAsync(this, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
    }
}