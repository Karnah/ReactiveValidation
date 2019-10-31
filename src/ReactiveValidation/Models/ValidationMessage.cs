using ReactiveValidation.Internal;

namespace ReactiveValidation
{
    /// <summary>
    /// Message of validation result.
    /// </summary>
    public class ValidationMessage : BaseNotifyPropertyChanged
    {
        /// <summary>
        /// Empty validation message which doesn't display or affect on <see cref="IObjectValidator.IsValid" /> property.
        /// </summary>
        public static ValidationMessage Empty = null;

        /// <summary>
        /// Source which allow dynamic get messages.
        /// </summary>
        private readonly IStringSource _stringSource;

        /// <summary>
        /// Create new validation message with static message.
        /// </summary>
        public ValidationMessage(string message, ValidationMessageType validationMessageType = ValidationMessageType.Error)
            : this(new StaticStringSource(message), validationMessageType)
        { }

        /// <summary>
        /// Create new validation message with localized message.
        /// </summary>
        public ValidationMessage(IStringSource stringSource, ValidationMessageType validationMessageType = ValidationMessageType.Error)
        {
            _stringSource = stringSource;

            ValidationMessageType = validationMessageType;
            Message = _stringSource.GetString();
        }

        /// <summary>
        /// Message.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Validation message type.
        /// </summary>
        public ValidationMessageType ValidationMessageType { get; }


        /// <inheritdoc />
        public override string ToString()
        {
            return Message;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ValidationMessage) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return ((_stringSource != null ? _stringSource.GetHashCode() : 0) * 397) ^ (int) ValidationMessageType;
            }
        }

        /// <summary>
        /// Update message.
        /// </summary>
        internal void UpdateMessage()
        {
            Message = _stringSource.GetString();
            OnPropertyChanged(nameof(Message));
        }

        /// <summary>
        /// Check if two validation messages are equal.
        /// </summary>
        protected bool Equals(ValidationMessage other)
        {
            return Equals(_stringSource, other._stringSource) && ValidationMessageType == other.ValidationMessageType;
        }
    }
}
