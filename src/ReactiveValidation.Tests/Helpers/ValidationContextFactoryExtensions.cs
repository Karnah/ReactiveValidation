using System.Collections.Generic;
using ReactiveValidation.Tests.TestModels;
using ReactiveValidation.ValidatorFactory;
using ReactiveValidation.Validators;

namespace ReactiveValidation.Tests.Helpers
{
    /// <summary>
    /// Extensions for <see cref="ValidationContextFactory{TObject}" />.
    /// </summary>
    internal static class ValidationContextFactoryExtensions
    {
        /// <summary>
        /// Create test object of <see cref="ValidationContextFactory{TObject}" />.
        /// </summary>
        public static ValidationContextFactory<TestValidatableObject> CreateValidationContextFactory(
            string propertyName, object? value)
        {
            return new ValidationContextFactory<TestValidatableObject>(
                new TestValidatableObject(),
                new ValidationContextCache(),
                new Dictionary<string, PropertyChangedStopwatch>(),
                propertyName,
                null,
                value);
        }
    }
}
