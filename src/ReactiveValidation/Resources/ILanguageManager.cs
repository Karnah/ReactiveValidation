using System;
using System.Globalization;
using System.Resources;

namespace ReactiveValidation
{
    public interface ILanguageManager
    {
        CultureInfo Culture { get; set; }

        CultureInfo CurrentCulture { get; }

        ResourceManager DefaultResourceManager { get; set; }

        bool TrackCultureChanged { get; set; }


        event EventHandler<CultureChangedEventArgs> CultureChanged;


        void OnCultureChanged();

        string GetString(string key);
    }
}
