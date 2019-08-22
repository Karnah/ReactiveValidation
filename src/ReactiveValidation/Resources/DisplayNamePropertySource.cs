using ReactiveValidation.Attributes;

namespace ReactiveValidation
{
    /// <summary>
    /// String source which returns display name of property.
    /// </summary>
    public class DisplayNamePropertySource : IStringSource
    {
        private readonly DisplayNameAttribute _displayNameAttribute;

        /// <summary>
        /// Create new instance of display name property source.
        /// </summary>
        /// <param name="displayNameAttribute">Attribute for display name.</param>
        public DisplayNamePropertySource(DisplayNameAttribute displayNameAttribute)
        {
            _displayNameAttribute = displayNameAttribute;
        }

        /// <inheritdoc />
        public string GetString()
        {
            return _displayNameAttribute.GetDisplayName();
        }
    }
}
