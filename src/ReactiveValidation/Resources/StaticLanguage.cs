using System.Collections.Generic;

namespace ReactiveValidation
{
    /// <summary>
    /// Resource which store localized strings for language.
    /// </summary>
    public abstract class StaticLanguage : ILanguage
    {
        private readonly Dictionary<string, string> _translations;

        /// <summary>
        /// Create new static resource of language.
        /// </summary>
        /// <param name="name">The name of language.</param>
        protected StaticLanguage(string name)
        {
            Name = name;

            _translations = new Dictionary<string, string>();
        }


        /// <inheritdoc />
        public string Name { get; }


        /// <inheritdoc />
        public string GetTranslation(string key)
        {
            if (_translations.TryGetValue(key, out var message))
                return message;

            return null;
        }

        /// <summary>
        /// Add localized string by its key.
        /// </summary>
        /// <param name="key">Key of message.</param>
        /// <param name="message">Localized message.</param>
        protected void AddTranslations(string key, string message)
        {
            _translations.Add(key, message);
        }
    }
}
