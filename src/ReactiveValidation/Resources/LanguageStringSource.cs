namespace ReactiveValidation
{
    /// <summary>
    /// Source which return localized message by its key and resource.
    /// </summary>
    public class LanguageStringSource : IStringSource
    {
        private readonly string _key;
        private readonly string _resource;

        /// <summary>
        /// Create new language source by its key.
        /// </summary>
        /// <param name="key">Key of string.</param>
        public LanguageStringSource(string key)
        {
            _key = key;
        }

        /// <summary>
        /// Create new language source by its resource and key.
        /// </summary>
        /// <param name="resource">Name of resource.</param>
        /// <param name="key">Key of string.</param>
        public LanguageStringSource(string resource, string key)
        {
            _key = key;
            _resource = resource;
        }


        /// <inheritdoc />
        public string GetString()
        {
            return ValidationOptions.LanguageManager.GetString(_key, _resource);
        }
    }
}
