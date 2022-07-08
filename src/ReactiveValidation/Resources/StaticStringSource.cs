using System;

namespace ReactiveValidation
{
    /// <summary>
    /// String source which always return one string.
    /// </summary>
    public class StaticStringSource : IStringSource
    {
        private readonly string _message;

        /// <summary>
        /// Create new instance of static string source.
        /// </summary>
        /// <param name="message">Message.</param>
        public StaticStringSource(string message)
        {
            _message = message ?? throw new ArgumentNullException(nameof(message));
        }

        /// <inheritdoc />
        public string GetString()
        {
            return _message;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StaticStringSource) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _message.GetHashCode();
        }

        /// <summary>
        /// Check if two sources are equal.
        /// </summary>
        protected bool Equals(StaticStringSource other)
        {
            return string.Equals(_message, other._message);
        }
    }
}
