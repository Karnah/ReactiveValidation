using System.Globalization;
using System.Resources;

namespace ReactiveValidation
{
    public interface ILanguageManager
    {
        CultureInfo Culture { get; set; }

        ResourceManager DefaultResourceManager { get; set; }


        string GetString(string key);
    }
}
