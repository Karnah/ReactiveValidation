namespace ReactiveValidation.Validators
{
    /// <summary>
    /// Factory for creating validation context.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable property.</typeparam>
    public class ValidationContextFactory<TObject>
        where TObject : IValidatableObject
    {
        /// <summary>
        /// Create new validation context factory.
        /// </summary>
        /// <param name="validatableObject">Object which being validating.</param>
        /// <param name="validationContextCache">Cache which store property values, result of functions and etc.</param>
        /// <param name="propertyName">Name of property which being validating.</param>
        /// <param name="displayNameSource">Display name of validatable property.</param>
        /// <param name="propertyValue">Value of property  which being validating.</param>
        internal ValidationContextFactory(TObject validatableObject, ValidationContextCache validationContextCache, string propertyName, IStringSource displayNameSource, object propertyValue)
        {
            PropertyName = propertyName;
            DisplayNameSource = displayNameSource;
            PropertyValue = propertyValue;

            ValidatableObject = validatableObject;
            ValidationContextCache = validationContextCache;
        }

        /// <summary>
        /// Object which being validating.
        /// </summary>
        public TObject ValidatableObject { get; }

        /// <summary>
        /// Cache which store property values, result of functions and etc.
        /// </summary>
        public ValidationContextCache ValidationContextCache { get; }
        
        /// <summary>
        /// Name of property which being validating.
        /// </summary>
        internal string PropertyName { get; }
        
        /// <summary>
        /// Display name of validatable property.
        /// </summary>
        internal IStringSource DisplayNameSource { get; }
        
        /// <summary>
        /// Value of property  which being validating.
        /// </summary>
        internal object PropertyValue { get; }
        
        /// <summary>
        /// Create context for validating property.
        /// </summary>
        /// <typeparam name="TProp">Type of validatable property.</typeparam>
        public ValidationContext<TObject, TProp> CreateContext<TProp>()
        {
            return new ValidationContext<TObject, TProp>(ValidatableObject, ValidationContextCache, PropertyName, DisplayNameSource, (TProp) PropertyValue);
        }
    }
}