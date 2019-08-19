using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;

using ReactiveValidation.Helpers;
using ReactiveValidation.Languages;

namespace ReactiveValidation
{
    /// <summary>
    /// Manager, which allows use different localization for validation messages.
    /// </summary>
    public sealed class LanguageManager
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
        /// Current culture.
        /// </summary>
        private CultureInfo _culture;

        /// <summary>
        /// Create new LanguageManager.
        /// </summary>
        internal LanguageManager()
        {
            var languages = new ILanguage[]
            {
                new RussianLanguage(),
                new EnglishLanguage(),
                new GermanLanguage()
            };
            _languages = languages.ToDictionary(l => l.Name, l => l);
            _cultureChangedCollection = new WeakCollection<EventHandler<CultureChangedEventArgs>>();

            Culture = CultureInfo.CurrentUICulture;
        }


        /// <summary>
        /// Current culture for validation messages.
        /// If not set - using <see cref="CultureInfo.CurrentUICulture" /> by default.
        /// </summary>
        public CultureInfo Culture
        {
            get => _culture;
            set
            {
                if (Equals(_culture, value))
                    return;

                _culture = value;
                OnCultureChanged();
            }
        }

        /// <summary>
        /// ResourceManager, which keeping strings for localization.
        /// </summary>
        public ResourceManager DefaultResourceManager { get; internal set; }

        /// <summary>
        /// If <see langword="true"/> all validation messages will change its message on <see cref="CultureChanged" /> event.
        /// </summary>
        public bool TrackCultureChanged { get; internal set; }


        /// <summary>
        /// Event, which fires when culture changed in application.
        /// </summary>
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


        /// <summary>
        /// Get localized string by its key.
        /// </summary>
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
            if (string.IsNullOrEmpty(message) && _languages.ContainsKey(languageCode))
            {
                message = _languages[languageCode].GetTranslation(key);
            }

            return message;
        }

        /// <summary>
        /// Raise <see cref="CultureChanged" /> event.
        /// </summary>
        private void OnCultureChanged()
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
    }
}
