using System.Linq;
using Moq;
using ReactiveValidation.ObjectObserver;
using ReactiveValidation.Tests.TestModels;
using ReactiveValidation.Validators;

namespace ReactiveValidation.Tests.Helpers;

/// <summary>
/// Extensions to setup mock of <see cref="IRuleBuilder{TObject}" />
/// </summary>
internal static class RuleBuilderExtensions
{
    /// <summary>
    /// Create rule builder for one property with specified property validators.
    /// </summary>
    public static Mock<IRuleBuilder<TestValidatableObject>> CreateRuleBuilder(
        string propertyName,
        params Mock<IPropertyValidator<TestValidatableObject>>[] propertyValidators)
    {
        var ruleBuilder = new Mock<IRuleBuilder<TestValidatableObject>>();
        
        ruleBuilder
            .SetupGet(rb => rb.ValidatableProperties)
            .Returns(new[] { propertyName });
        ruleBuilder
            .SetupGet(rb => rb.ObservingPropertiesSettings)
            .Returns(() => new ObservingPropertySettings());
        ruleBuilder
            .Setup(rb => rb.GetValidators())
            .Returns(() => propertyValidators.Select(pv => pv.Object).ToList());

        return ruleBuilder;
    }

    /// <summary>
    /// Set cascade mode of rule builder.
    /// </summary>
    public static Mock<IRuleBuilder<TestValidatableObject>> WithCascadeMode(
        this Mock<IRuleBuilder<TestValidatableObject>> ruleBuilder,
        CascadeMode? cascadeMode)
    {
        ruleBuilder
            .SetupGet(rb => rb.PropertyCascadeMode)
            .Returns(() => cascadeMode);

        return ruleBuilder;
    }
    
    /// <summary>
    /// Set settings of rule builder.
    /// </summary>
    public static Mock<IRuleBuilder<TestValidatableObject>> WithSettings(
        this Mock<IRuleBuilder<TestValidatableObject>> ruleBuilder,
        ObservingPropertySettings settings)
    {
        ruleBuilder
            .SetupGet(rb => rb.ObservingPropertiesSettings)
            .Returns(() => settings);

        return ruleBuilder;
    }
}