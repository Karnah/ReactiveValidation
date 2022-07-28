using System;
using System.Threading;
using Moq;
using ReactiveValidation.Helpers.Nito.AsyncEx;
using ReactiveValidation.Tests.TestModels;
using ReactiveValidation.Validators;

namespace ReactiveValidation.Tests.Helpers;

/// <summary>
/// Extensions to setup mock of <see cref="IPropertyValidator{TObject}" />
/// </summary>
internal static class PropertyValidatorExtensions
{
    /// <summary>
    /// Create an empty sync property validator.
    /// </summary>
    public static Mock<IPropertyValidator<TestValidatableObject>> CreateSyncPropertyValidator()
    {
        var propertyValidator = new Mock<IPropertyValidator<TestValidatableObject>>();

        propertyValidator
            .SetupGet(pv => pv.IsAsync)
            .Returns(false);
        
        propertyValidator
            .SetupGet(pv => pv.RelatedProperties)
            .Returns(Array.Empty<string>());

        return propertyValidator;
    }
    
    /// <summary>
    /// Create an empty sync property validator.
    /// </summary>
    public static Mock<IPropertyValidator<TestValidatableObject>> CreateAsyncPropertyValidator()
    {
        var propertyValidator = new Mock<IPropertyValidator<TestValidatableObject>>();

        propertyValidator
            .SetupGet(pv => pv.IsAsync)
            .Returns(true);
        
        propertyValidator
            .SetupGet(pv => pv.RelatedProperties)
            .Returns(Array.Empty<string>());

        return propertyValidator;
    }
    
    /// <summary>
    /// Setup list of related properties.
    /// </summary>
    public static Mock<IPropertyValidator<TestValidatableObject>> WithRelatedProperties(
        this Mock<IPropertyValidator<TestValidatableObject>> propertyValidator,
        params string[] relatedProperties)
    {
        propertyValidator
            .SetupGet(pv => pv.RelatedProperties)
            .Returns(relatedProperties);

        return propertyValidator;
    }
    
    /// <summary>
    /// Setup list of validation messages.
    /// </summary>
    public static Mock<IPropertyValidator<TestValidatableObject>> WithMessages(
        this Mock<IPropertyValidator<TestValidatableObject>> propertyValidator,
        params ValidationMessage[] validationMessages)
    {
        propertyValidator
            .Setup(pv => pv.ValidateProperty(It.IsAny<ValidationContextFactory<TestValidatableObject>>()))
            .Returns(validationMessages);
        
        return propertyValidator;
    }
    
    /// <summary>
    /// Setup list of validation messages.
    /// </summary>
    public static Mock<IPropertyValidator<TestValidatableObject>> WithMessages(
        this Mock<IPropertyValidator<TestValidatableObject>> propertyValidator,
        AsyncManualResetEvent waitingEvent,
        params ValidationMessage[] validationMessages)
    {
        propertyValidator
            .Setup(pv => pv.ValidatePropertyAsync(It.IsAny<ValidationContextFactory<TestValidatableObject>>(), It.IsAny<CancellationToken>()))
            .Returns<ValidationContextFactory<TestValidatableObject>, CancellationToken>(async (_, cancellationToken) =>
            {
                await waitingEvent.WaitAsync(cancellationToken);
                return validationMessages;
            });
        
        return propertyValidator;
    }
}