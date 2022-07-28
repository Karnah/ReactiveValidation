using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xunit;
using ReactiveValidation.Tests.Helpers;
using ReactiveValidation.Tests.TestModels;

namespace ReactiveValidation.Tests.ObjectValidator;

/// <summary>
/// Tests for class <see cref="ObjectValidator{TObject}" /> with check <see cref="CascadeMode" />.
/// </summary>
public partial class ObjectValidatorTests
{
    /// <summary>
    /// Check validation for two validators for one property with different <see cref="ValidationMessageType" /> and different settings of <see cref="CascadeMode" />.
    /// </summary>
    [Theory]
    [InlineData(CascadeMode.Stop, null)]
    [InlineData(CascadeMode.Continue, CascadeMode.Stop)]
    
    public void ValidateCascadeMode_TwoValidatorsForOneProperty_StopCascadeMode(CascadeMode globalCascadeMode, CascadeMode? ruleBuilderCascadeMode)
    {
        // ARRANGE.
        const string propertyName = nameof(TestValidatableObject.Number);
        ValidationOptions.PropertyCascadeMode = globalCascadeMode;
        
        var instance = new TestValidatableObject();
        var events = new List<DataErrorsChangedEventArgs>();
        instance.ErrorsChanged += (_, args) => { events.Add(args); };

        var firstPropertyValidator = PropertyValidatorExtensions
            .CreateSyncPropertyValidator()
            .WithMessages(new ValidationMessage(FIRST_VALIDATION_MESSAGE));
        var secondPropertyValidator = PropertyValidatorExtensions
            .CreateSyncPropertyValidator()
            .WithMessages(new ValidationMessage(SECOND_VALIDATION_MESSAGE));
        var ruleBuilder = RuleBuilderExtensions
            .CreateRuleBuilder(propertyName, firstPropertyValidator, secondPropertyValidator)
            .WithCascadeMode(ruleBuilderCascadeMode);
        var objectValidator = new ObjectValidator<TestValidatableObject>(instance, new[] { ruleBuilder.Object });

        // ACT.
        instance.RaisePropertyChangedEvent(propertyName);

        // ASSERT.
        CheckEvents(events, propertyName);
        CheckSyncObjectValidator(objectValidator, false, false,
            new ValidationMessage(FIRST_VALIDATION_MESSAGE));
    }
    
    /// <summary>
    /// Check validation for two validators for one property with different <see cref="ValidationMessageType" /> and different settings of <see cref="CascadeMode" />.
    /// </summary>
    [Theory]
    [InlineData(CascadeMode.Continue, null)]
    [InlineData(CascadeMode.Stop, CascadeMode.Continue)]
    public void ValidateCascadeMode_TwoValidatorsForOneProperty_ContinueCascadeMode(CascadeMode globalCascadeMode, CascadeMode? ruleBuilderCascadeMode)
    {
        // ARRANGE.
        const string propertyName = nameof(TestValidatableObject.Number);
        ValidationOptions.PropertyCascadeMode = globalCascadeMode;
        
        var instance = new TestValidatableObject();
        var events = new List<DataErrorsChangedEventArgs>();
        instance.ErrorsChanged += (_, args) => { events.Add(args); };

        var firstPropertyValidator = PropertyValidatorExtensions
            .CreateSyncPropertyValidator()
            .WithMessages(new ValidationMessage(FIRST_VALIDATION_MESSAGE));
        var secondPropertyValidator = PropertyValidatorExtensions
            .CreateSyncPropertyValidator()
            .WithMessages(new ValidationMessage(SECOND_VALIDATION_MESSAGE));
        var ruleBuilder = RuleBuilderExtensions
            .CreateRuleBuilder(propertyName, firstPropertyValidator, secondPropertyValidator)
            .WithCascadeMode(ruleBuilderCascadeMode);
        var objectValidator = new ObjectValidator<TestValidatableObject>(instance, new[] { ruleBuilder.Object });

        // ACT.
        instance.RaisePropertyChangedEvent(propertyName);

        // ASSERT.
        CheckEvents(events, propertyName);
        CheckSyncObjectValidator(objectValidator, false, false,
            new ValidationMessage(FIRST_VALIDATION_MESSAGE),
            new ValidationMessage(SECOND_VALIDATION_MESSAGE));
    }
    
    /// <summary>
    /// Check validation for two validators for one property with different <see cref="ValidationMessageType" /> and <see cref="CascadeMode.Stop" />.
    /// Must return only first message.
    /// </summary>
    [Theory]
    [InlineData(ValidationMessageType.Error, ValidationMessageType.Warning, false, false)]
    [InlineData(ValidationMessageType.Warning, ValidationMessageType.Error, true, true)]
    public void ValidateCascadeModeStop_TwoValidatorsForOneProperty_OnlyFirstMessage(ValidationMessageType firstValidationMessageType, ValidationMessageType secondValidationMessageType, bool isValid, bool hasWarnings)
    {
        // ARRANGE.
        const string propertyName = nameof(TestValidatableObject.Number);
        ValidationOptions.PropertyCascadeMode = CascadeMode.Stop;
        
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
            new ValidationMessage(FIRST_VALIDATION_MESSAGE, firstValidationMessageType));
    }
    
    /// <summary>
    /// Check validation for two validators for one property with and <see cref="CascadeMode.Stop" />.
    /// If first message is empty then return second message.
    /// </summary>
    [Fact]
    public void ValidateCascadeModeStop_FirstValidatorValid_SecondMessage()
    {
        // ARRANGE.
        const string propertyName = nameof(TestValidatableObject.Number);
        ValidationOptions.PropertyCascadeMode = CascadeMode.Stop;
        
        var instance = new TestValidatableObject();
        var events = new List<DataErrorsChangedEventArgs>();
        instance.ErrorsChanged += (_, args) => { events.Add(args); };

        var firstPropertyValidator = PropertyValidatorExtensions
            .CreateSyncPropertyValidator()
            .WithMessages(Array.Empty<ValidationMessage>());
        var secondPropertyValidator = PropertyValidatorExtensions
            .CreateSyncPropertyValidator()
            .WithMessages(new ValidationMessage(SECOND_VALIDATION_MESSAGE));
        var ruleBuilder = RuleBuilderExtensions.CreateRuleBuilder(propertyName, firstPropertyValidator, secondPropertyValidator);
        var objectValidator = new ObjectValidator<TestValidatableObject>(instance, new[] { ruleBuilder.Object });

        // ACT + ASSERT.
        // Step 1. First validator is valid. Second is not.
        instance.RaisePropertyChangedEvent(propertyName);

        CheckEvents(events, propertyName);
        CheckSyncObjectValidator(objectValidator, false, false,
            new ValidationMessage(SECOND_VALIDATION_MESSAGE));
        
        
        // Step 2. First validator is not valid, second should reset messages.
        firstPropertyValidator
            .WithMessages(new ValidationMessage(FIRST_VALIDATION_MESSAGE));
        instance.RaisePropertyChangedEvent(propertyName);
        
        CheckEvents(events, propertyName);
        CheckSyncObjectValidator(objectValidator, false, false,
            new ValidationMessage(FIRST_VALIDATION_MESSAGE));
    }
    
    /// <summary>
    /// Check validation for two validators for one property with and <see cref="CascadeMode.Stop" />.
    /// First validator is not valid. Second changed because of related property.
    /// Then messages won't changed. 
    /// </summary>
    [Fact]
    public void ValidateCascadeModeStop_SecondValidatorRelatedProperty_SecondMessage()
    {
        // ARRANGE.
        const string firstPropertyName = nameof(TestValidatableObject.Number);
        const string secondPropertyName = nameof(TestValidatableObject.InnerValidatableObject);
        ValidationOptions.PropertyCascadeMode = CascadeMode.Stop;
        
        var instance = new TestValidatableObject();
        var events = new List<DataErrorsChangedEventArgs>();
        instance.ErrorsChanged += (_, args) => { events.Add(args); };

        var firstPropertyValidator = PropertyValidatorExtensions
            .CreateSyncPropertyValidator()
            .WithMessages(new ValidationMessage(FIRST_VALIDATION_MESSAGE));
        var secondPropertyValidator = PropertyValidatorExtensions
            .CreateSyncPropertyValidator()
            .WithRelatedProperties(secondPropertyName)
            .WithMessages(new ValidationMessage(SECOND_VALIDATION_MESSAGE));
        var ruleBuilder = RuleBuilderExtensions.CreateRuleBuilder(firstPropertyName, firstPropertyValidator, secondPropertyValidator);
        var objectValidator = new ObjectValidator<TestValidatableObject>(instance, new[] { ruleBuilder.Object });

        // ACT + ASSERT.
        // Step 1. First validator is not valid.
        instance.RaisePropertyChangedEvent(firstPropertyName);

        CheckEvents(events, firstPropertyName);
        CheckSyncObjectValidator(objectValidator, false, false,
            new ValidationMessage(FIRST_VALIDATION_MESSAGE));
        
        
        // Step 2. Messages doesn't changed.
        instance.RaisePropertyChangedEvent(secondPropertyName);
        
        CheckEvents(events);
        CheckSyncObjectValidator(objectValidator, false, false,
            new ValidationMessage(FIRST_VALIDATION_MESSAGE));
    }
}