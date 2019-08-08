using System;
using System.Globalization;
using System.Resources;

namespace ReactiveValidation
{
    /// <summary>
    /// Manager, which allow change language of validation messages.
    /// </summary>
    public interface ILanguageManager
    {
        /// <summary>
        /// Current culture for validation messages.
        /// If not set - using <see cref="CultureInfo.CurrentUICulture" /> by default.
        /// </summary>
        CultureInfo Culture { get; set; }

        /// <summary>
        /// ResourceManager, which keeping strings for localization.
        /// </summary>
        ResourceManager DefaultResourceManager { get; set; }

        /// <summary>
        /// If <see langword="true"/> all validation messages will change its message on <see cref="CultureChanged" /> event.
        /// Set to <see langword="true"/> only if you need change culture runtime.
        /// </summary>
        bool TrackCultureChanged { get; set; }


        /// <summary>
        /// Event, which fires when culture changed in application.
        /// </summary>
        /// <remarks>
        /// Carefully! If you inherit this interface on your own, you should use WeakReference!
        /// Please, see default realization.
        /// </remarks>
        event EventHandler<CultureChangedEventArgs> CultureChanged;


        /// <summary>
        /// Notify that application culture changed.
        /// Using when changing only <see cref="CultureInfo.CurrentUICulture" />.
        /// </summary>
        void OnCultureChanged();

        /// <summary>
        /// Get localized string by its key.
        /// </summary>
        string GetString(string key);
    }
}
