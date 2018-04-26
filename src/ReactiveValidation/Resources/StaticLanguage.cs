using System.Collections.Generic;

namespace ReactiveValidation
{
    public abstract class StaticLanguage : ILanguage
    {
        private readonly Dictionary<string, string> _translations;

        protected StaticLanguage(string name)
        {
            Name = name;

            _translations = new Dictionary<string, string>();
        }


        protected void AddTranslations(string key, string message)
        {
            _translations[key] = message;
        }


        public string Name { get; }


        public string GetTranslation(string key)
        {
            if (_translations.TryGetValue(key, out var message) == true) {
                return message;
            }

            return null;
        }
    }
}
