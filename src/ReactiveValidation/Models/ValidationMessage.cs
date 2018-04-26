namespace ReactiveValidation
{
    public class ValidationMessage
    {
        public static ValidationMessage Empty = null;


        public ValidationMessage(string message) : this(message, ValidationMessageType.Error)
        { }

        public ValidationMessage(string message, ValidationMessageType validationMessageType)
        {
            Message = message;
            ValidationMessageType = validationMessageType;
        }


        public string Message { get; private set; }

        public ValidationMessageType ValidationMessageType { get; protected set; }


        public override string ToString()
        {
            return Message;
        }
    }
}
