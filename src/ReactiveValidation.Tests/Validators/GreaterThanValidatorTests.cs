using System;
using System.Collections;
using System.Linq;

using Xunit;

using ReactiveValidation.Tests.TestModels;
using ReactiveValidation.Validators;
using ReactiveValidation.Tests.Helpers;

namespace ReactiveValidation.Tests.Validators
{
    public class GreaterThanValidatorTests
    {
        [Theory]
        [InlineData(0, 10)]
        [InlineData(-20, 10)]
        [InlineData(10, 10)]
        public void GreaterThanValidator_NotValidTheory(int value, int valueToCompare)
        {
            var validationMessage = GreaterThan(value, valueToCompare);

            AssertValidationMessage.NotEmptyMessage(validationMessage);
        }

        [Theory]
        [InlineData(20, 10)]
        [InlineData(20, -30)]
        public void GreaterThanValidator_ValidTheory(int value, int valueToCompare)
        {
            var validationMessage = GreaterThan(value, valueToCompare);

            AssertValidationMessage.EmptyMessage(validationMessage);
        }


        [Theory]
        [InlineData("B", "b")]
        [InlineData("b", null)]
        public void GreaterThanValidatorWithoutComparer_ValidTheory(string value, string valueToCompare)
        {
            var validationMessage = GreaterThan(value, valueToCompare);

            AssertValidationMessage.EmptyMessage(validationMessage);
        }

        [Theory]
        [InlineData("B", "b")]
        [InlineData("b", "B")]
        public void GreaterThanValidatorWithComparer_NotValidTheory(string value, string valueToCompare)
        {
            var validationMessage = GreaterThan(value, valueToCompare, StringComparer.OrdinalIgnoreCase);

            AssertValidationMessage.NotEmptyMessage(validationMessage);
        }


        private ValidationMessage GreaterThan<TProp>(
            TProp value,
            TProp valueToCompare,
            IComparer comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TProp : IComparable<TProp>
        {
            var greaterThanValidator = new GreaterThanValidator<TestValidatableObject, TProp>(_ => valueToCompare, comparer, validationMessageType);
            var factory = new ValidationContextFactory<TestValidatableObject>(null, nameof(TestValidatableObject.Number), null, value);
            var validationMessage = greaterThanValidator.ValidateProperty(factory).FirstOrDefault();

            return validationMessage;
        }
    }
}
