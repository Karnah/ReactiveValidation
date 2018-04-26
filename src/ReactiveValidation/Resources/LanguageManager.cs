using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;

using ReactiveValidation.Languages;

namespace ReactiveValidation
{
    public class LanguageManager : ILanguageManager
    {
        private readonly Dictionary<string, ILanguage> _languages;

        public LanguageManager()
        {
            var languages = new ILanguage[] {
                new RussianLanguage(),
                new EnglishLanguage(),
            };
            _languages = languages.ToDictionary(l => l.Name, l => l);
        }


        public CultureInfo Culture { get; set; }

        public ResourceManager DefaultResourceManager { get; set; }


        public string GetString(string key)
        {
            var culture = Culture ?? CultureInfo.CurrentUICulture;
            var code = culture.Name;

            if (culture.IsNeutralCulture == false && _languages.ContainsKey(code) == false) {
                code = culture.Parent.Name;
            }

            var message = DefaultResourceManager?.GetString(key, culture);
            if (string.IsNullOrEmpty(message) == true && _languages.ContainsKey(code)) {
                message = _languages[code].GetTranslation(key);
            }

            return message ?? string.Empty;
        }
    }
}
