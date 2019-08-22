using ReactiveValidation.Internal;

namespace ReactiveValidation
{
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
        /// Create new validation message with dynamic messages.
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
        public ValidationMessageType ValidationMessageType { get; protected set; }


        /// <inheritdoc />
        public override string ToString()
        {
            return Message;
        }

        /// <summary>
        /// Update message.
        /// </summary>
        internal void UpdateMessage()
        {
            Message = _stringSource.GetString();
            OnPropertyChanged(nameof(Message));

        }
    }
}
