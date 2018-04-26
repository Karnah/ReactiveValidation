using System;
using System.Resources;

namespace ReactiveValidation.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DisplayNameAttribute : Attribute
    {
        public string DisplayName { get; set; }

        public string DisplayNameKey { get; set; }

        public ResourceManager ResourceManager { get; set; }


        internal string GetDisplayName()
        {
            if (string.IsNullOrEmpty(DisplayNameKey) == false) {
                if (ResourceManager != null)
                    return ResourceManager.GetString(DisplayNameKey, ValidationOptions.LanguageManager.Culture);

                return ValidationOptions.LanguageManager.GetString(DisplayNameKey);
            }

            return DisplayName;
        }
    }
}
