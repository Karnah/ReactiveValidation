using System;
using System.Collections;
using System.Linq;

using Xunit;

using ReactiveValidation.Tests.Helpers;
using ReactiveValidation.Tests.TestModels;
using ReactiveValidation.Validators;

namespace ReactiveValidation.Tests.Validators
{
    public class LessThanValidatorTests
    {
        [Theory]
        [InlineData(20, 10)]
        [InlineData(20, -10)]
        [InlineData(10, 10)]
        public void LessThanValidator_NotValidTheory(int value, int valueToCompare)
        {
            var validationMessage = LessThan(value, valueToCompare);

            AssertValidationMessage.NotEmptyMessage(validationMessage);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(-30, 0)]
        public void LessThanValidator_ValidTheory(int value, int valueToCompare)
        {
            var validationMessage = LessThan(value, valueToCompare);

            AssertValidationMessage.EmptyMessage(validationMessage);
        }


        [Theory]
        [InlineData("b", "B")]
        public void LessThanValidatorWithoutComparer_ValidTheory(string value, string valueToCompare)
        {
            var validationMessage = LessThan(value, valueToCompare);

            AssertValidationMessage.EmptyMessage(validationMessage);
        }

        [Theory]
        [InlineData("B", "b")]
        [InlineData("b", "B")]
        [InlineData("b", null)]
        public void LessThanValidatorWithComparer_NotValidTheory(string value, string valueToCompare)
        {
            var validationMessage = LessThan(value, valueToCompare, StringComparer.OrdinalIgnoreCase);

            AssertValidationMessage.NotEmptyMessage(validationMessage);
        }


        private ValidationMessage LessThan<TProp>(
            TProp value,
            TProp valueToCompare,
            IComparer comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TProp : IComparable<TProp>
        {
            var lessThanValidator = new LessThanValidator<TestValidatableObject, TProp>(_ => valueToCompare, comparer, validationMessageType);
            var factory = new ValidationContextFactory<TestValidatableObject>(null, new ValidationContextCache(), nameof(TestValidatableObject.Number), null, value);
            var validationMessage = lessThanValidator.ValidateProperty(factory).FirstOrDefault();

            return validationMessage;
        }
    }
}
