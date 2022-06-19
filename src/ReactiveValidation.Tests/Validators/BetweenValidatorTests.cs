using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Moq;
using Xunit;

using ReactiveValidation.Extensions;
using ReactiveValidation.Tests.Helpers;
using ReactiveValidation.Tests.TestModels;
using ReactiveValidation.Validators;

namespace ReactiveValidation.Tests.Validators
{
    public class BetweenValidatorTests
    {
        [Theory]
        [InlineData(0, 10, 20)]
        [InlineData(25, 10, 20)]
        public void BetweenValidator_NotValidTheory(int value, int from, int to)
        {
            var validationMessage = Between(value, from, to);

            AssertValidationMessage.NotEmptyMessage(validationMessage);
        }

        [Theory]
        [InlineData(10, 10, 20)]
        [InlineData(15, 10, 20)]
        [InlineData(20, 10, 20)]
        public void BetweenValidator_ValidTheory(int value, int from, int to)
        {
            var validationMessage = Between(value, from, to);

            AssertValidationMessage.EmptyMessage(validationMessage);
        }


        [Theory]
        [InlineData("b", "A", "C")]
        [InlineData("B", "A", "C")]
        public void BetweenValidatorWithoutComparer_ValidTheory(string value, string from, string to)
        {
            var validationMessage = Between(value, from, to);

            AssertValidationMessage.EmptyMessage(validationMessage);
        }

        [Theory]
        [InlineData("b", "A", "C")]
        [InlineData("B", "a", "c")]
        public void BetweenValidatorWithComparer_NotValidTheory(string value, string from, string to)
        {
            var validationMessage = Between(value, from, to, StringComparer.Ordinal);

            AssertValidationMessage.NotEmptyMessage(validationMessage);
        }

        [Theory]
        [InlineData("b", "a", "c")]
        [InlineData("b", "A", "c")]
        [InlineData("B", "A", "C")]
        public void BetweenValidatorWithComparer_ValidTheory(string value, string from, string to)
        {
            var validationMessage = Between(value, from, to, StringComparer.Ordinal);

            AssertValidationMessage.EmptyMessage(validationMessage);
        }

        private ValidationMessage Between<TProp>(
            TProp value,
            TProp from,
            TProp to,
            IComparer comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TProp : IComparable<TProp>
        {
            var betweenValidator = new BetweenValidator<TestValidatableObject, TProp>(_ => from, _ => to, comparer, validationMessageType);
            var factory = new ValidationContextFactory<TestValidatableObject>(null, new ValidationCache<TestValidatableObject>(null), nameof(TestValidatableObject.Number), null, value);
            var validationMessage = betweenValidator.ValidateProperty(factory).FirstOrDefault();

            return validationMessage;
        }




        [Fact]
        public void BetweenExtensions_Between20And10_Exception()
        {
            const int from = 20;
            const int to = 10;

            var ruleBuilder = Mock.Of<ISinglePropertyRuleBuilder<TestValidatableObject, int>>();
            Assert.Throws<ArgumentException>(() => ruleBuilder.Between(from, to));
        }
    }
}
