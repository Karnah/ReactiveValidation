using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FluentAssertions;
using Xunit;
using ReactiveValidation.Tests.Helpers;
using ReactiveValidation.Tests.TestModels;

namespace ReactiveValidation.Tests.ObjectValidator;

/// <summary>
/// Tests for class <see cref="ObjectValidator{TObject}" />.
/// </summary>
public partial class ObjectValidatorTests
{
    private const string FIRST_VALIDATION_MESSAGE = "foo";
    private const string SECOND_VALIDATION_MESSAGE = "bar";

    public ObjectValidatorTests()
    {
        // Set default state before each test.
        ValidationOptions.PropertyCascadeMode = CascadeMode.Continue;
    }

    /// <summary>
    /// Check simple validation for one validator with different <see cref="ValidationMessageType" />.
    /// </summary>
    [Theory]
    [InlineData(ValidationMessageType.Error, false, false)]
    [InlineData(ValidationMessageType.SimpleError, false, false)]
    [InlineData(ValidationMessageType.Warning, true, true)]
    [InlineData(ValidationMessageType.SimpleWarning, true, true)]
    public void Validate_OneValidator_EventRaised(ValidationMessageType validationMessageType, bool isValid, bool hasWarnings)
    {
        // ARRANGE.
        const string propertyName = nameof(TestValidatableObject.Number);
        
        var instance = new TestValidatableObject();
        var events = new List<DataErrorsChangedEventArgs>();
        instance.ErrorsChanged += (_, args) => { events.Add(args); };

        var propertyValidator = PropertyValidatorExtensions
            .CreateSyncPropertyValidator()
            .WithMessages(new ValidationMessage(FIRST_VALIDATION_MESSAGE, validationMessageType));
        var ruleBuilder = RuleBuilderExtensions.CreateRuleBuilder(propertyName, propertyValidator);
        var objectValidator = new ObjectValidator<TestValidatableObject>(instance, new[] { ruleBuilder.Object });

        // ACT.
        instance.RaisePropertyChangedEvent(propertyName);

        // ASSERT.
        CheckEvents(events, propertyName);
        CheckSyncObjectValidator(objectValidator, isValid, hasWarnings,
            new ValidationMessage(FIRST_VALIDATION_MESSAGE, validationMessageType));
    }

    /// <summary>
    /// Check if validator return no messages.
    /// </summary>
    [Fact]
    public void Validate_OneValidatorNoError_EventNotRaised()
    {
        // ARRANGE.
        const string propertyName = nameof(TestValidatableObject.Number);
        
        var instance = new TestValidatableObject();
        var events = new List<DataErrorsChangedEventArgs>();
        instance.ErrorsChanged += (_, args) => { events.Add(args); };
        
        var propertyValidator = PropertyValidatorExtensions
            .CreateSyncPropertyValidator()
            .WithMessages(Array.Empty<ValidationMessage>());
        var ruleBuilder = RuleBuilderExtensions.CreateRuleBuilder(propertyName, propertyValidator);
        var objectValidator = new ObjectValidator<TestValidatableObject>(instance, new[] { ruleBuilder.Object });

        // ACT.
        instance.RaisePropertyChangedEvent(propertyName);

        // ASSERT.
        CheckEvents(events, Array.Empty<string>());
        CheckSyncObjectValidator(objectValidator, true, false);
    }
    
    /// <summary>
    /// Check validation for two validators for one property with different <see cref="ValidationMessageType" />.
    /// </summary>
    [Theory]
    [InlineData(ValidationMessageType.Error, ValidationMessageType.Error, false, false)]
    [InlineData(ValidationMessageType.Error, ValidationMessageType.Warning, false, true)]
    [InlineData(ValidationMessageType.Warning, ValidationMessageType.Error, false, true)]
    [InlineData(ValidationMessageType.Warning, ValidationMessageType.Warning, true, true)]
    public void Validate_TwoValidatorsForOneProperty_EventRaised(ValidationMessageType firstValidationMessageType, ValidationMessageType secondValidationMessageType, bool isValid, bool hasWarnings)
    {
        // ARRANGE.
        const string propertyName = nameof(TestValidatableObject.Number);
        
        var instance = new TestValidatableObject();
        var events = new List<DataErrorsChangedEventArgs>();
        instance.ErrorsChanged += (_, args) => { events.Add(args); };

        var firstPropertyValidator = PropertyValidatorExtensions
            .CreateSyncPropertyValidator()
            .WithMessages(new ValidationMessage(FIRST_VALIDATION_MESSAGE, firstValidationMessageType));
        var secondPropertyValidator = PropertyValidatorExtensions
            .CreateSyncPropertyValidator()
            .WithMessages(new ValidationMessage(SECOND_VALIDATION_MESSAGE, secondValidationMessageType));
        var ruleBuilder = RuleBuilderExtensions.CreateRuleBuilder(propertyName, firstPropertyValidator, secondPropertyValidator);
        var objectValidator = new ObjectValidator<TestValidatableObject>(instance, new[] { ruleBuilder.Object });

        // ACT.
        instance.RaisePropertyChangedEvent(propertyName);

        // ASSERT.
        CheckEvents(events, propertyName);
        CheckSyncObjectValidator(objectValidator, isValid, hasWarnings,
            new ValidationMessage(FIRST_VALIDATION_MESSAGE, firstValidationMessageType),
            new ValidationMessage(SECOND_VALIDATION_MESSAGE, secondValidationMessageType));
    }
    
    /// <summary>
    /// Check validation for two validators for two properties with different <see cref="ValidationMessageType" />.
    /// </summary>
    [Theory]
    [InlineData(ValidationMessageType.Error, ValidationMessageType.Error, false, false)]
    [InlineData(ValidationMessageType.Error, ValidationMessageType.Warning, false, true)]
    [InlineData(ValidationMessageType.Warning, ValidationMessageType.Error, false, true)]
    [InlineData(ValidationMessageType.Warning, ValidationMessageType.Warning, true, true)]
    public void Validate_TwoValidatorsForTwoProperties_EventRaised(ValidationMessageType firstValidationMessageType, ValidationMessageType secondValidationMessageType, bool isValid, bool hasWarnings)
    {
        // ARRANGE.
        const string firstPropertyName = nameof(TestValidatableObject.Number);
        const string secondPropertyName = nameof(TestValidatableObject.InnerValidatableObject);
        
        var instance = new TestValidatableObject();
        var events = new List<DataErrorsChangedEventArgs>();
        instance.ErrorsChanged += (_, args) => { events.Add(args); };

        var firstPropertyValidator = PropertyValidatorExtensions
            .CreateSyncPropertyValidator()
            .WithMessages(new ValidationMessage(FIRST_VALIDATION_MESSAGE, firstValidationMessageType));
        var firstRuleBuilder = RuleBuilderExtensions.CreateRuleBuilder(firstPropertyName, firstPropertyValidator);
        var secondPropertyValidator = PropertyValidatorExtensions
            .CreateSyncPropertyValidator()
            .WithMessages(new ValidationMessage(SECOND_VALIDATION_MESSAGE, secondValidationMessageType));
        var secondRuleBuilder = RuleBuilderExtensions.CreateRuleBuilder(secondPropertyName, secondPropertyValidator);
        var objectValidator = new ObjectValidator<TestValidatableObject>(instance, new[] { firstRuleBuilder.Object, secondRuleBuilder.Object });

        // ACT.
        instance.RaisePropertyChangedEvent(firstPropertyName);
        instance.RaisePropertyChangedEvent(secondPropertyName);

        // ASSERT.
        CheckEvents(events, firstPropertyName, secondPropertyName);
        CheckSyncObjectValidator(objectValidator, isValid, hasWarnings,
            new ValidationMessage(FIRST_VALIDATION_MESSAGE, firstValidationMessageType),
            new ValidationMessage(SECOND_VALIDATION_MESSAGE, secondValidationMessageType));
    }
    
    /// <summary>
    /// Check validation for one validator with related property.
    /// If related property is changed - validator should be revalidated.
    /// </summary>
    [Fact]
    public void Validate_RelatedValidator_EventRaised()
    {
        // ARRANGE.
        const string propertyName = nameof(TestValidatableObject.Number);
        
        var instance = new TestValidatableObject();
        var events = new List<DataErrorsChangedEventArgs>();
        instance.ErrorsChanged += (_, args) => { events.Add(args); };

        var propertyValidator = PropertyValidatorExtensions
            .CreateSyncPropertyValidator()
            .WithRelatedProperties(nameof(instance.InnerValidatableObject))
            .WithMessages(new ValidationMessage(FIRST_VALIDATION_MESSAGE));
        var ruleBuilder = RuleBuilderExtensions.CreateRuleBuilder(propertyName, propertyValidator);
        var objectValidator = new ObjectValidator<TestValidatableObject>(instance, new[] { ruleBuilder.Object });

        // ACT.
        instance.RaisePropertyChangedEvent(nameof(instance.InnerValidatableObject));

        // ASSERT.
        CheckEvents(events, propertyName);
        CheckSyncObjectValidator(objectValidator, false, false,
            new ValidationMessage(FIRST_VALIDATION_MESSAGE));
    }

    /// <summary>
    /// Check properties of <see cref="ObjectValidator{TObject}" />.
    /// </summary>
    private static void CheckSyncObjectValidator(
        ObjectValidator<TestValidatableObject> objectValidator,
        bool isValid,
        bool hasWarnings,
        params ValidationMessage[] validationMessages)
    {
        objectValidator.IsValid.Should().Be(isValid);
        objectValidator.HasWarnings.Should().Be(hasWarnings);
        objectValidator.IsAsyncValidating.Should().BeFalse();

        objectValidator
            .WaitValidatingCompletedAsync()
            .IsCompleted
            .Should()
            .BeTrue();
        
        objectValidator.ValidationMessages
            .Should()
            .BeEquivalentTo(validationMessages, options => options.WithStrictOrdering());
    }

    /// <summary>
    /// Check raised error events.
    /// </summary>
    private static void CheckEvents(List<DataErrorsChangedEventArgs> events, params string[] properties)
    {
        events
            .Select(e => e.PropertyName)
            .Should()
            .BeEquivalentTo(properties, options => options.WithStrictOrdering());
        
        events.Clear();
    }
}