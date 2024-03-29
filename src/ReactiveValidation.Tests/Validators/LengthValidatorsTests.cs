﻿using System.Linq;

using Xunit;

using ReactiveValidation.Tests.TestModels;
using ReactiveValidation.Validators;
using ReactiveValidation.Tests.Helpers;

namespace ReactiveValidation.Tests.Validators
{
    public class LengthValidatorsTests
    {
        #region BetweenLength

        [Theory]
        [InlineData("a", 1, 3)]
        [InlineData("aa", 1, 3)]
        [InlineData("aaa", 1, 3)]
        public void BetweenLengthValidator_ValidTheory(string value, int minLength, int maxLength)
        {
            var validationMessage = BetweenLength(value, minLength, maxLength);

            AssertValidationMessage.EmptyMessage(validationMessage);
        }

        [Theory]
        [InlineData("", 1, 3)]
        [InlineData("aaaa", 1, 3)]
        public void BetweenLengthValidator_NotValidTheory(string value, int minLength, int maxLength)
        {
            var validationMessage = BetweenLength(value, minLength, maxLength);

            AssertValidationMessage.NotEmptyMessage(validationMessage);
        }

        private static ValidationMessage? BetweenLength(
            string value,
            int minLength,
            int maxLength,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
        {
            var betweenLengthValidator = new LengthValidator<TestValidatableObject>(_ => minLength, _ => maxLength, validationMessageType);
            var factory = ValidationContextFactoryExtensions.CreateValidationContextFactory(nameof(TestValidatableObject.Number), value);
            var validationMessage = betweenLengthValidator.ValidateProperty(factory).FirstOrDefault();

            return validationMessage;
        }

        #endregion


        #region MinLength

        [Theory]
        [InlineData("aa", 2)]
        [InlineData("aaa", 2)]
        public void MinLengthValidator_ValidTheory(string value, int minLength)
        {
            var validationMessage = MinLength(value, minLength);

            AssertValidationMessage.EmptyMessage(validationMessage);
        }

        [Theory]
        [InlineData("", 2)]
        [InlineData("a", 2)]
        public void MinLengthValidator_NotValidTheory(string value, int minLength)
        {
            var validationMessage = MinLength(value, minLength);

            AssertValidationMessage.NotEmptyMessage(validationMessage);
        }

        private static ValidationMessage? MinLength(
            string value,
            int minLength,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
        {
            var betweenLengthValidator = new MinLengthValidator<TestValidatableObject>(_ => minLength, validationMessageType);
            var factory = ValidationContextFactoryExtensions.CreateValidationContextFactory(nameof(TestValidatableObject.Number), value);
            var validationMessage = betweenLengthValidator.ValidateProperty(factory).FirstOrDefault();

            return validationMessage;
        }

        #endregion


        #region MaxLength

        [Theory]
        [InlineData("a", 2)]
        [InlineData("aa", 2)]
        public void MaxLengthValidator_ValidTheory(string value, int maxLength)
        {
            var validationMessage = MaxLength(value, maxLength);

            AssertValidationMessage.EmptyMessage(validationMessage);
        }

        [Theory]
        [InlineData("aaa", 2)]
        [InlineData("aaaa", 2)]
        public void MaxLengthValidator_NotValidTheory(string value, int maxLength)
        {
            var validationMessage = MaxLength(value, maxLength);

            AssertValidationMessage.NotEmptyMessage(validationMessage);
        }

        private static ValidationMessage? MaxLength(
            string value,
            int maxLength,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
        {
            var betweenLengthValidator = new MaxLengthValidator<TestValidatableObject>(_ => maxLength, validationMessageType);
            var factory = ValidationContextFactoryExtensions.CreateValidationContextFactory(nameof(TestValidatableObject.Number), value);
            var validationMessage = betweenLengthValidator.ValidateProperty(factory).FirstOrDefault();

            return validationMessage;
        }

        #endregion


        #region ExactLength

        [Theory]
        [InlineData(null, 0)]
        [InlineData("", 0)]
        [InlineData("aa", 2)]
        public void ExactLengthValidator_ValidTheory(string value, int length)
        {
            var validationMessage = ExactLength(value, length);

            AssertValidationMessage.EmptyMessage(validationMessage);
        }

        [Theory]
        [InlineData(null, 2)]
        [InlineData("a", 2)]
        [InlineData("aaa", 2)]
        public void ExactLengthValidator_NotValidTheory(string value, int length)
        {
            var validationMessage = ExactLength(value, length);

            AssertValidationMessage.NotEmptyMessage(validationMessage);
        }

        private static ValidationMessage? ExactLength(
            string value,
            int length,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
        {
            var betweenLengthValidator = new ExactLengthValidator<TestValidatableObject>(_ => length, validationMessageType);
            var factory = ValidationContextFactoryExtensions.CreateValidationContextFactory(nameof(TestValidatableObject.Number), value);
            var validationMessage = betweenLengthValidator.ValidateProperty(factory).FirstOrDefault();

            return validationMessage;
        }

        #endregion
    }
}
