using System.Resources;

namespace ReactiveValidation
{
    public class LanguageStringSource : IStringSource
    {
        private readonly string _key;
        private readonly ResourceManager _resourceManager;

        public LanguageStringSource(string key)
        {
            _key = key;
        }

        public LanguageStringSource(ResourceManager resourceManager, string key)
        {
            _key = key;
            _resourceManager = resourceManager;
        }


        public string GetString()
        {
            if (_resourceManager != null)
                return _resourceManager.GetString(_key, ValidationOptions.LanguageManager.CurrentCulture);

            return ValidationOptions.LanguageManager.GetString(_key);
        }
    }
}
