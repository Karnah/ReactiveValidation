using Xunit;

namespace ReactiveValidation.Tests.Helpers
{
    public static class AssertValidationMessage
    {
        public static void EmptyMessage(ValidationMessage message)
        {
            Assert.Equal(ValidationMessage.Empty, message);
        }

        public static void NotEmptyMessage(ValidationMessage message, ValidationMessageType validationMessageType = ValidationMessageType.Error)
        {
            Assert.NotEqual(ValidationMessage.Empty, message);
            Assert.Equal(validationMessageType, message?.ValidationMessageType);
            Assert.NotNull(message?.Message);
            Assert.NotEqual(string.Empty, message?.Message);
        }
    }
}
