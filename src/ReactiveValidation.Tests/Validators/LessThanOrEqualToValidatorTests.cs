using System;
using System.Collections;
using System.Linq;

using Xunit;

using ReactiveValidation.Tests.Helpers;
using ReactiveValidation.Tests.TestModels;
using ReactiveValidation.Validators;

namespace ReactiveValidation.Tests.Validators
{
    public class LessThanOrEqualToValidatorTests
    {
        [Theory]
        [InlineData(20, 10)]
        [InlineData(20, -10)]
        public void LessThanOrEqualToValidator_NotValidTheory(int value, int valueToCompare)
        {
            var validationMessage = LessThanOrEqualTo(value, valueToCompare);

            AssertValidationMessage.NotEmptyMessage(validationMessage);
        }

        [Theory]
        [InlineData(10, 10)]
        [InlineData(0, 10)]
        public void LessThanOrEqualToValidator_ValidTheory(int value, int valueToCompare)
        {
            var validationMessage = LessThanOrEqualTo(value, valueToCompare);

            AssertValidationMessage.EmptyMessage(validationMessage);
        }


        [Theory]
        [InlineData("b", "B")]
        public void LessThanOrEqualToValidatorWithoutComparer_ValidTheory(string value, string valueToCompare)
        {
            var validationMessage = LessThanOrEqualTo(value, valueToCompare);

            AssertValidationMessage.EmptyMessage(validationMessage);
        }

        [Theory]
        [InlineData("B", "b")]
        public void LessThanOrEqualToValidatorWithoutComparer_NotValidTheory(string value, string valueToCompare)
        {
            var validationMessage = LessThanOrEqualTo(value, valueToCompare);

            AssertValidationMessage.NotEmptyMessage(validationMessage);
        }

        [Theory]
        [InlineData("B", "b")]
        [InlineData("b", "B")]
        public void LessThanOrEqualToValidatorWithComparer_ValidTheory(string value, string valueToCompare)
        {
            var validationMessage = LessThanOrEqualTo(value, valueToCompare, StringComparer.OrdinalIgnoreCase);

            AssertValidationMessage.EmptyMessage(validationMessage);
        }


        private ValidationMessage LessThanOrEqualTo<TProp>(
            TProp value,
            TProp valueToCompare,
            IComparer comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TProp : IComparable<TProp>
        {
            var lessThanOrEqualToValidator = new LessThanOrEqualValidator<TestValidatableObject, TProp>(_ => valueToCompare, comparer, validationMessageType);
            var factory = new ValidationContextFactory<TestValidatableObject>(null, nameof(TestValidatableObject.Number), null, value);
            var validationMessage = lessThanOrEqualToValidator.ValidateProperty(factory).FirstOrDefault();

            return validationMessage;
        }
    }
}
