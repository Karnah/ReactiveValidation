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
        private readonly Dictionary<string, ILanguage> _languages;

        private CultureInfo _overridedCulture;

        public LanguageManager()
        {
            var languages = new ILanguage[] {
                new RussianLanguage(),
                new EnglishLanguage(),
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

            var message = DefaultResourceManager?.GetString(key, CurrentCulture);
            if (string.IsNullOrEmpty(message) == true && _languages.ContainsKey(code)) {
                message = _languages[code].GetTranslation(key);
            }

            return message ?? string.Empty;
        }
    }
}
