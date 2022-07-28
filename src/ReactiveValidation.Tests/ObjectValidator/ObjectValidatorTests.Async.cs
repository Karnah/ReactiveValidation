using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using ReactiveValidation.Helpers.Nito.AsyncEx;
using ReactiveValidation.Tests.Helpers;
using ReactiveValidation.Tests.TestModels;
using ReactiveValidation.Validators;

namespace ReactiveValidation.Tests.ObjectValidator;

/// <summary>
/// Tests for class <see cref="ObjectValidator{TObject}" /> with check async validators.
/// </summary>
public partial class ObjectValidatorTests
{
    /// <summary>
    /// Check if validator return no messages.
    /// </summary>
    [Fact]
    public async Task AsyncValidate_OneValidatorNoError_EventNotRaised()
    {
        // ARRANGE.
        const string propertyName = nameof(TestValidatableObject.Number);
        
        var instance = new TestValidatableObject();
        var events = new List<DataErrorsChangedEventArgs>();
        instance.ErrorsChanged += (_, args) => { events.Add(args); };

        var propertyValidatorWaiter = new AsyncManualResetEvent(false);
        var propertyValidator = PropertyValidatorExtensions
            .CreateAsyncPropertyValidator()
            .WithMessages(propertyValidatorWaiter, Array.Empty<ValidationMessage>());
        var ruleBuilder = RuleBuilderExtensions.CreateRuleBuilder(propertyName, propertyValidator);
        var objectValidator = new ObjectValidator<TestValidatableObject>(instance, new[] { ruleBuilder.Object });

        // ACT + ASSERT.
        // STEP 1: Raise property change and check if async validation is running.
        instance.RaisePropertyChangedEvent(propertyName);

        CheckEvents(events, Array.Empty<string>());
        CheckAsyncObjectValidator(objectValidator, true, false,
            Array.Empty<ValidationMessage>());
        
        // STEP 2: complete async validation.
        propertyValidatorWaiter.Set();
        await objectValidator.WaitValidatingCompletedAsync();
        
        CheckEvents(events, Array.Empty<string>());
        CheckSyncObjectValidator(objectValidator, true, false,
            Array.Empty<ValidationMessage>());
    }
    
    /// <summary>
    /// Check if validator had validation messages and run again then it reset messages on new running.
    /// </summary>
    [Fact]
    public async Task AsyncValidate_OneValidator_ValidationMessagesResetWhileRunning()
    {
        // ARRANGE.
        const string propertyName = nameof(TestValidatableObject.Number);
        
        var instance = new TestValidatableObject();
        var events = new List<DataErrorsChangedEventArgs>();
        instance.ErrorsChanged += (_, args) => { events.Add(args); };

        var propertyValidatorWaiter = new AsyncManualResetEvent(true);
        var propertyValidator = PropertyValidatorExtensions
            .CreateAsyncPropertyValidator()
            .WithMessages(propertyValidatorWaiter, new ValidationMessage(FIRST_VALIDATION_MESSAGE));
        var ruleBuilder = RuleBuilderExtensions.CreateRuleBuilder(propertyName, propertyValidator);
        var objectValidator = new ObjectValidator<TestValidatableObject>(instance, new[] { ruleBuilder.Object });

        // ACT + ASSERT.
        // STEP 1: Validate once for messages.
        instance.RaisePropertyChangedEvent(propertyName);

        CheckEvents(events, propertyName);
        CheckSyncObjectValidator(objectValidator, false, false,
            new ValidationMessage(FIRST_VALIDATION_MESSAGE));
        
        // STEP 2: run validation again and check that messages are empty.
        propertyValidatorWaiter.Reset();
        instance.RaisePropertyChangedEvent(propertyName);
        
        CheckEvents(events, propertyName);
        CheckAsyncObjectValidator(objectValidator, true, false,
            Array.Empty<ValidationMessage>());
        
        // STEP 3: complete async validation.
        propertyValidatorWaiter.Set();
        await objectValidator.WaitValidatingCompletedAsync();
        
        CheckEvents(events, propertyName);
        CheckSyncObjectValidator(objectValidator, false, false,
            new ValidationMessage(FIRST_VALIDATION_MESSAGE));
    }
    
    /// <summary>
    /// Check if property change value before previous validation is completed, it will be cancelled.
    /// </summary>
    [Fact]
    public async Task AsyncValidate_OneValidatorPropertyChangedTwice_FirstValidationCancelled()
    {
        // ARRANGE.
        const string propertyName = nameof(TestValidatableObject.Number);
        
        var instance = new TestValidatableObject();
        var events = new List<DataErrorsChangedEventArgs>();
        instance.ErrorsChanged += (_, args) => { events.Add(args); };

        var propertyValidatorWaiter = new AsyncManualResetEvent(false);
        var propertyValidator = PropertyValidatorExtensions
            .CreateAsyncPropertyValidator()
            .WithMessages(propertyValidatorWaiter, new ValidationMessage(FIRST_VALIDATION_MESSAGE));
        var ruleBuilder = RuleBuilderExtensions.CreateRuleBuilder(propertyName, propertyValidator);
        var objectValidator = new ObjectValidator<TestValidatableObject>(instance, new[] { ruleBuilder.Object });

        // ACT + ASSERT.
        // STEP 1: Run and check first validation.
        instance.RaisePropertyChangedEvent(propertyName);

        CheckEvents(events);
        CheckAsyncObjectValidator(objectValidator, true, false,
            Array.Empty<ValidationMessage>());
        
        // STEP 2: Change property again and it will begin new validation.
        instance.RaisePropertyChangedEvent(propertyName);
        
        CheckEvents(events);
        CheckAsyncObjectValidator(objectValidator, true, false,
            Array.Empty<ValidationMessage>());
        
        // Check that for first validation IsCancellationRequested, and for second are not.
        propertyValidator.Verify(v => v.ValidatePropertyAsync(
            It.IsAny<ValidationContextFactory<TestValidatableObject>>(),
            It.Is<CancellationToken>(ct => ct.IsCancellationRequested)),
            Times.Once);
        propertyValidator.Verify(v => v.ValidatePropertyAsync(
                It.IsAny<ValidationContextFactory<TestValidatableObject>>(),
                It.Is<CancellationToken>(ct => !ct.IsCancellationRequested)),
            Times.Once);
        
        // STEP 3: complete async validation.
        propertyValidatorWaiter.Set();
        await objectValidator.WaitValidatingCompletedAsync();
        
        CheckEvents(events, propertyName);
        CheckSyncObjectValidator(objectValidator, false, false,
            new ValidationMessage(FIRST_VALIDATION_MESSAGE));
    }
    
    /// <summary>
    /// Check properties of <see cref="ObjectValidator{TObject}" /> when async running.
    /// </summary>
    private static void CheckAsyncObjectValidator(
        ObjectValidator<TestValidatableObject> objectValidator,
        bool isValid,
        bool hasWarnings,
        params ValidationMessage[] validationMessages)
    {
        objectValidator.IsValid.Should().Be(isValid);
        objectValidator.HasWarnings.Should().Be(hasWarnings);
        objectValidator.IsAsyncValidating.Should().BeTrue();

        objectValidator
            .WaitValidatingCompletedAsync()
            .IsCompleted
            .Should()
            .BeFalse();
        
        objectValidator.ValidationMessages
            .Should()
            .BeEquivalentTo(validationMessages, options => options.WithStrictOrdering());
    }
}