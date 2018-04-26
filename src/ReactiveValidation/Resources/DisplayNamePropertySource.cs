using ReactiveValidation.Attributes;

namespace ReactiveValidation
{
    public class DisplayNamePropertySource : IStringSource
    {
        private readonly DisplayNameAttribute _displayNameAttribute;

        public DisplayNamePropertySource(DisplayNameAttribute displayNameAttribute)
        {
            _displayNameAttribute = displayNameAttribute;
        }


        public string GetString()
        {
            return _displayNameAttribute.GetDisplayName();
        }
    }
}
