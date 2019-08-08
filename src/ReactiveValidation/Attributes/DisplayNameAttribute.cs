using System;
using System.Resources;

namespace ReactiveValidation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DisplayNameAttribute : Attribute
    {
        private Type _resourceType;
        private ResourceManager _resourceManager;

        public string DisplayName { get; set; }

        public string DisplayNameKey { get; set; }

        public Type ResourceType {
            get => _resourceType;
            set {
                if (_resourceType == value)
                    return;

                _resourceType = value;

                _resourceManager = new ResourceManager(_resourceType);
                try {
                    _resourceManager.GetString(string.Empty);
                }
                catch (MissingManifestResourceException) {
                    throw new ArgumentException($"Can not create ResourceManager from {_resourceType}");
                }
            }
        }


        internal string GetDisplayName()
        {
            if (string.IsNullOrEmpty(DisplayNameKey) == false) {
                if (_resourceManager != null) {
                    return _resourceManager.GetString(DisplayNameKey, ValidationOptions.LanguageManager.Culture);
                }

                return ValidationOptions.LanguageManager.GetString(DisplayNameKey);
            }

            return DisplayName;
        }
    }
}
