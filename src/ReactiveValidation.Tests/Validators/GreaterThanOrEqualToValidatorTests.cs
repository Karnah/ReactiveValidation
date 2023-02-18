using System;
using System.Collections;
using System.Linq;

using Xunit;

using ReactiveValidation.Tests.TestModels;
using ReactiveValidation.Validators;
using ReactiveValidation.Tests.Helpers;

namespace ReactiveValidation.Tests.Validators
{
    public class GreaterThanOrEqualToValidatorTests
    {
        [Theory]
        [InlineData(0, 10)]
        [InlineData(-20, 10)]
        public void GreaterThanOrEqualToValidator_NotValidTheory(int value, int valueToCompare)
        {
            var validationMessage = GreaterThanOrEqualTo(value, valueToCompare);

            AssertValidationMessage.NotEmptyMessage(validationMessage);
        }

        [Theory]
        [InlineData(10, 10)]
        [InlineData(20, 10)]
        public void GreaterThanOrEqualToValidator_ValidTheory(int value, int valueToCompare)
        {
            var validationMessage = GreaterThanOrEqualTo(value, valueToCompare);

            AssertValidationMessage.EmptyMessage(validationMessage);
        }


        [Theory]
        [InlineData("B", "b")]
        public void GreaterThanOrEqualToValidatorWithoutComparer_ValidTheory(string value, string valueToCompare)
        {
            var validationMessage = GreaterThanOrEqualTo(value, valueToCompare);

            AssertValidationMessage.EmptyMessage(validationMessage);
        }

        [Theory]
        [InlineData("b", "B")]
        public void GreaterThanOrEqualToValidatorWithoutComparer_NotValidTheory(string value, string valueToCompare)
        {
            var validationMessage = GreaterThanOrEqualTo(value, valueToCompare);

            AssertValidationMessage.NotEmptyMessage(validationMessage);
        }

        [Theory]
        [InlineData("B", "b")]
        [InlineData("b", "B")]
        public void GreaterThanOrEqualToValidatorWithComparer_ValidTheory(string value, string valueToCompare)
        {
            var validationMessage = GreaterThanOrEqualTo(value, valueToCompare, StringComparer.OrdinalIgnoreCase);

            AssertValidationMessage.EmptyMessage(validationMessage);
        }


        private static ValidationMessage? GreaterThanOrEqualTo<TProp>(
            TProp value,
            TProp valueToCompare,
            IComparer? comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TProp : IComparable<TProp>
        {
            var greaterThanOrEqualToValidator = new GreaterThanOrEqualValidator<TestValidatableObject, TProp>(_ => valueToCompare, comparer, validationMessageType);
            var factory = ValidationContextFactoryExtensions.CreateValidationContextFactory(nameof(TestValidatableObject.Number), value);
            var validationMessage = greaterThanOrEqualToValidator.ValidateProperty(factory).FirstOrDefault();

            return validationMessage;
        }
    }
}
