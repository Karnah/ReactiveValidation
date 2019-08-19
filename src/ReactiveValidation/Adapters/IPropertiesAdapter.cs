namespace ReactiveValidation.Adapters
{
    /// <summary>
    /// Object which tracking properties and revalidating it of necessity.
    /// </summary>
    internal interface IPropertiesAdapter
    {
        /// <summary>
        /// Refresh validation messages for all target properties.
        /// </summary>
        void Revalidate();

        /// <summary>
        /// Refresh validation messages for specified property.
        /// </summary>
        /// <param name="propertyName">Name of property which should be revalidated.</param>
        void Revalidate(string propertyName);
    }
}
