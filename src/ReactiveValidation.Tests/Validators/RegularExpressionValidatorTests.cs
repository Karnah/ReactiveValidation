using System.Linq;

using Xunit;

using ReactiveValidation.Tests.Helpers;
using ReactiveValidation.Tests.TestModels;
using ReactiveValidation.Validators;

namespace ReactiveValidation.Tests.Validators
{
    public class RegularExpressionValidatorTests
    {
        [Theory]
        [InlineData("12345", @"^\d+$")]
        [InlineData("abacaba", @"^\w{7}$")]
        public void RegularExpressionValidator_ValidTheory(string s, string pattern)
        {
            var lessThanOrEqualToValidator = new RegularExpressionValidator<TestValidatableObject>(_ => pattern, ValidationMessageType.Error);
            var factory = new ValidationContextFactory<TestValidatableObject>(null, new ValidationCache<TestValidatableObject>(null), nameof(TestValidatableObject.Number), null, s);
            var validationMessage = lessThanOrEqualToValidator.ValidateProperty(factory).FirstOrDefault();

            AssertValidationMessage.EmptyMessage(validationMessage);
        }

        [Theory]
        [InlineData("12345abacaba", @"^\d+$")]
        [InlineData("abacab-", @"^\w{7}$")]
        public void RegularExpressionValidator_NotValidTheory(string s, string pattern)
        {
            var lessThanOrEqualToValidator = new RegularExpressionValidator<TestValidatableObject>(_ => pattern, ValidationMessageType.Error);
            var factory = new ValidationContextFactory<TestValidatableObject>(null, new ValidationCache<TestValidatableObject>(null), nameof(TestValidatableObject.Number), null, s);
            var validationMessage = lessThanOrEqualToValidator.ValidateProperty(factory).FirstOrDefault();

            AssertValidationMessage.NotEmptyMessage(validationMessage);
        }
    }
}
