﻿namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Factory for creating validation context.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable property.</typeparam>
    public class ValidationContextFactory<TObject>
        where TObject : IValidatableObject
    {
        private readonly string _propertyName;
        private readonly IStringSource _displayNameSource;
        private readonly object _propertyValue;

        /// <summary>
        /// Create new validation context factory.
        /// </summary>
        /// <param name="validatableObject">Object which being validating.</param>
        /// <param name="validationCache">Cache which store property values, result of functions and etc.</param>
        /// <param name="propertyName">Name of property which being validating.</param>
        /// <param name="displayNameSource">Display name of validatable property.</param>
        /// <param name="propertyValue">Value of property  which being validating.</param>
        internal ValidationContextFactory(TObject validatableObject, ValidationCache<TObject> validationCache, string propertyName, IStringSource displayNameSource, object propertyValue)
        {
            _propertyName = propertyName;
            _displayNameSource = displayNameSource;
            _propertyValue = propertyValue;

            ValidatableObject = validatableObject;
            ValidationCache = validationCache;
        }

        /// <summary>
        /// Object which being validating.
        /// </summary>
        public TObject ValidatableObject { get; }

        /// <summary>
        /// Cache which store property values, result of functions and etc.
        /// </summary>
        public ValidationCache<TObject> ValidationCache { get; }
        
        /// <summary>
        /// Create context for validating property.
        /// </summary>
        /// <typeparam name="TProp">Type of validatable property.</typeparam>
        public ValidationContext<TObject, TProp> CreateContext<TProp>()
        {
            return new ValidationContext<TObject, TProp>(ValidatableObject, ValidationCache, _propertyName, _displayNameSource, (TProp) _propertyValue);
        }
    }
}