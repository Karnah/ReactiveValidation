using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using ReactiveValidation.Helpers;
using ReactiveValidation.Languages;

namespace ReactiveValidation
{
    /// <inheritdoc />
    public class LanguageManager : ILanguageManager
    {
        /// <summary>
        /// Culture code which using when message in current culture not found.
        /// </summary>
        private const string DEFAULT_CULTURE_CODE = "en";

        /// <summary>
        /// Default culture.
        /// </summary>
        private readonly CultureInfo _defaultCulture = new CultureInfo(DEFAULT_CULTURE_CODE);
        /// <summary>
        /// List of supported languages by default.
        /// </summary>
        private readonly Dictionary<string, ILanguage> _languages;
        /// <summary>
        /// Weak collection of <see cref="CultureChanged" /> event subscribers.
        /// </summary>
        private readonly WeakCollection<EventHandler<CultureChangedEventArgs>> _cultureChangedCollection;

        /// <summary>
        /// Object for <see cref="_cultureChangedCollection" /> synchronization.
        /// </summary>
        private readonly object _cultureEventLock = new object();

        /// <summary>
        /// Culture which overriding by <see cref="ILanguageManager" />.
        /// </summary>
        private CultureInfo _overriddenCulture;

        /// <summary>
        /// Create new LanguageManager.
        /// </summary>
        public LanguageManager()
        {
            var languages = new ILanguage[]
            {
                new RussianLanguage(),
                new EnglishLanguage(),
                new GermanLanguage()
            };
            _languages = languages.ToDictionary(l => l.Name, l => l);
            _cultureChangedCollection = new WeakCollection<EventHandler<CultureChangedEventArgs>>();
        }


        /// <inheritdoc />
        public CultureInfo Culture
        {
            get => _overriddenCulture ?? CultureInfo.CurrentUICulture;
            set
            {
                if (Equals(_overriddenCulture, value))
                    return;

                _overriddenCulture = value;
                OnCultureChanged();
            }
        }

        /// <inheritdoc />
        public ResourceManager DefaultResourceManager { get; set; }

        /// <inheritdoc />
        public bool TrackCultureChanged { get; set; } = false;


        /// <inheritdoc />
        public event EventHandler<CultureChangedEventArgs> CultureChanged
        {
            add
            {
                lock (_cultureEventLock)
                {
                    _cultureChangedCollection.Add(value);
                }
            }
            remove
            {
                lock (_cultureEventLock)
                {
                    _cultureChangedCollection.Remove(value);
                }
            }
        }


        /// <inheritdoc />
        public void OnCultureChanged()
        {
            lock (_cultureEventLock)
            {
                var eventArgs = new CultureChangedEventArgs(Culture);
                foreach (var eventHandler in _cultureChangedCollection.GetLiveItems())
                {
                    eventHandler(this, eventArgs);
                }
            }
        }

        /// <inheritdoc />
        public string GetString(string key)
        {
            var code = Culture.Name;
            if (!Culture.IsNeutralCulture && !_languages.ContainsKey(code)) {
                code = Culture.Parent.Name;
            }

            // First try get string for current culture.
            var message = GetLocalizedString(key, code, Culture);

            // If empty, than try return string for default culture.
            if (string.IsNullOrEmpty(message))
                message = GetLocalizedString(key, DEFAULT_CULTURE_CODE, _defaultCulture);

            // Otherwise - just return key.
            if (string.IsNullOrEmpty(message))
                message = key;

            return message ?? string.Empty;
        }

        /// <summary>
        /// Get localized string from resource manager.
        /// </summary>
        /// <param name="key">Key of string.</param>
        /// <param name="languageCode">Code of language for static resources.</param>
        /// <param name="culture">Culture for resource manager.</param>
        private string GetLocalizedString(string key, string languageCode, CultureInfo culture)
        {
            var message = DefaultResourceManager?.GetString(key, culture);
            if (string.IsNullOrEmpty(message) && _languages.ContainsKey(languageCode)) {
                message = _languages[languageCode].GetTranslation(key);
            }

            return message;
        }
    }
}
