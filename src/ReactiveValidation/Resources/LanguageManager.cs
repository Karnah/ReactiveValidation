using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;

using ReactiveValidation.Languages;

namespace ReactiveValidation
{
    public class LanguageManager : ILanguageManager
    {
        private const string DefaultCultureCode = "en";
        private readonly CultureInfo _defaultCulture = new CultureInfo(DefaultCultureCode);

        private readonly Dictionary<string, ILanguage> _languages;

        private CultureInfo _overridedCulture;

        public LanguageManager()
        {
            var languages = new ILanguage[] {
                new RussianLanguage(),
                new EnglishLanguage(),
                new GermanLanguage()
            };
            _languages = languages.ToDictionary(l => l.Name, l => l);

            CurrentCulture = Culture ?? CultureInfo.CurrentUICulture;
        }


        public CultureInfo Culture {
            get => _overridedCulture;
            set {
                if (Equals(_overridedCulture, value))
                    return;

                _overridedCulture = value;
                OnCultureChanged();
            }
        }

        public CultureInfo CurrentCulture { get; private set; }

        public ResourceManager DefaultResourceManager { get; set; }

        public bool TrackCultureChanged { get; set; } = false;


        public event EventHandler<CultureChangedEventArgs> CultureChanged;


        public void OnCultureChanged()
        {
            CurrentCulture = Culture ?? CultureInfo.CurrentUICulture;

            CultureChanged?.Invoke(this, new CultureChangedEventArgs(CurrentCulture));
        }

        public string GetString(string key)
        {
            var code = CurrentCulture.Name;

            if (CurrentCulture.IsNeutralCulture == false && _languages.ContainsKey(code) == false) {
                code = CurrentCulture.Parent.Name;
            }

            var message = GetLocalizedString(key, code, CurrentCulture);
            if (string.IsNullOrEmpty(message) == true) {
                message = GetLocalizedString(key, DefaultCultureCode, _defaultCulture);
            }
            if (string.IsNullOrEmpty(message) == true) {
                message = key;
            }

            return message ?? string.Empty;
        }

        private string GetLocalizedString(string key, string code, CultureInfo culture)
        {
            var message = DefaultResourceManager?.GetString(key, culture);
            if (string.IsNullOrEmpty(message) == true && _languages.ContainsKey(code)) {
                message = _languages[code].GetTranslation(key);
            }

            return message;
        }
    }
}
