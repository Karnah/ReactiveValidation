namespace ReactiveValidation.Adapters
{
    internal interface IPropertiesAdapter
    {
        /// <summary>
        /// Refresh validation messages for target properties
        /// </summary>
        void Revalidate();

        void Revalidate(string propertyName);
    }
}
