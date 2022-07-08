using System;

namespace ReactiveValidation
{
    /// <summary>
    /// Source which return localized message by its key and resource.
    /// </summary>
    public class LanguageStringSource : IStringSource
    {
        private readonly string _key;
        private readonly string? _resource;

        /// <summary>
        /// Create new language source by its key.
        /// </summary>
        /// <param name="key">Key of string.</param>
        public LanguageStringSource(string key)
        {
            _key = key ?? throw new ArgumentNullException(nameof(key));
        }

        /// <summary>
        /// Create new language source by its resource and key.
        /// </summary>
        /// <param name="resource">Name of resource.</param>
        /// <param name="key">Key of string.</param>
        public LanguageStringSource(string resource, string key)
        {
            _key = key ?? throw new ArgumentNullException(nameof(key));
            _resource = resource ?? throw new ArgumentNullException(nameof(resource));
        }


        /// <inheritdoc />
        public string GetString()
        {
            return ValidationOptions.LanguageManager.GetString(_key, _resource);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LanguageStringSource) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (_key.GetHashCode() * 397) ^ (_resource != null ? _resource.GetHashCode() : 0);
            }
        }

        /// <summary>
        /// Check if two sources are equal.
        /// </summary>
        protected bool Equals(LanguageStringSource other)
        {
            return string.Equals(_key, other._key) && string.Equals(_resource, other._resource);
        }
    }
}
