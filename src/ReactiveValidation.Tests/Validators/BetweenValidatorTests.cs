using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using ReactiveValidation.Extensions;
using ReactiveValidation.Validators;


namespace ReactiveValidation.Tests.Validators
{
    [TestClass]
    public class BetweenValidatorTests
    {
        public class TestValidatableObject : ValidatableObject
        {
            public int Number { get; set; }
        }


        [TestMethod]
        public void ValidateProperty_0Between10And20_ErrorMessage()
        {
            const int num = 0;
            const int from = 10;
            const int to = 20;

            var betweenValidator = new BetweenValidator<TestValidatableObject, int>(_ => from, _ => to, ValidationMessageType.Error);
            var context = new ValidationContext<TestValidatableObject, int>(null, nameof(TestValidatableObject.Number), null, num);
            var validationMessage = betweenValidator.ValidateProperty(context).FirstOrDefault();

            Assert.AreNotEqual(null, validationMessage);
            Assert.AreEqual(ValidationMessageType.Error, validationMessage?.ValidationMessageType);
            Assert.IsNotNull(validationMessage?.Message);
            Assert.AreNotEqual(string.Empty, validationMessage?.Message);
        }

        [TestMethod]
        public void ValidateProperty_15Between10And20_EmptyMessage()
        {
            const int num = 15;
            const int from = 10;
            const int to = 20;

            var betweenValidator = new BetweenValidator<TestValidatableObject, int>(_ => from, _ => to, ValidationMessageType.Error);
            var context = new ValidationContext<TestValidatableObject, int>(null, nameof(TestValidatableObject.Number), null, num);
            var validationMessage = betweenValidator.ValidateProperty(context).FirstOrDefault();

            Assert.AreEqual(ValidationMessage.Empty, validationMessage);
        }

        [TestMethod]
        public void ValidateProperty_25Between10And20_ErrorMessage()
        {
            const int num = 25;
            const int from = 10;
            const int to = 20;

            var betweenValidator = new BetweenValidator<TestValidatableObject, int>(_ => from, _ => to, ValidationMessageType.Error);
            var context = new ValidationContext<TestValidatableObject, int>(null, nameof(TestValidatableObject.Number), null, num);
            var validationMessage = betweenValidator.ValidateProperty(context).FirstOrDefault();

            Assert.AreNotEqual(null, validationMessage);
            Assert.AreEqual(ValidationMessageType.Error, validationMessage?.ValidationMessageType);
            Assert.IsNotNull(validationMessage?.Message);
            Assert.AreNotEqual(string.Empty, validationMessage?.Message);
        }

        [TestMethod]
        public void Between_Between20And10_Exception()
        {
            const int from = 20;
            const int to = 10;

            var ruleBuilder = Mock.Of<ISinglePropertyRuleBuilder<TestValidatableObject, int>>();
            Assert.ThrowsException<ArgumentException>(() => ruleBuilder.Between(from, to));
        }
    }
}
