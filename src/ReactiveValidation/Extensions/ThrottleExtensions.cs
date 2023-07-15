using System;
using ReactiveValidation.Validators.Throttle;

namespace ReactiveValidation.Extensions;

/// <summary>
/// Extensions for throttle settings.
/// </summary>
public static class ThrottleExtensions
{
    /// <summary>
    /// Property will begin validating after <paramref name="validatingPropertyMillisecondsDelay" />.
    /// If property changes value while this delay, previous value won't be validated.
    /// </summary>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="validatingPropertyMillisecondsDelay">The duration in milliseconds of the throttle period for validating property.</param>
    /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TProp">The type of validatable property.</typeparam>
    public static TNext Throttle<TObject, TProp, TNext>(
        this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
        int validatingPropertyMillisecondsDelay)
        where TNext : IRuleBuilder<TObject, TProp, TNext>
        where TObject : IValidatableObject
    {
        return ruleBuilder.Throttle(builder => builder.AddValidatingPropertyThrottle(validatingPropertyMillisecondsDelay));
    }
    
    /// <summary>
    /// Property will begin validating after <paramref name="validatingPropertyMillisecondsDelay" />.
    /// If property changes value while this delay, previous value won't be validated.
    /// </summary>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="validatingPropertyMillisecondsDelay">The duration in milliseconds of the throttle period for validating property.</param>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TProp">The type of validatable property.</typeparam>
    public static IRuleBuilderOption<TObject, TProp> CommonThrottle<TObject, TProp>(
        this IRuleBuilderOption<TObject, TProp> ruleBuilder,
        int validatingPropertyMillisecondsDelay)
        where TObject : IValidatableObject
    {
        return ruleBuilder.CommonThrottle(builder => builder.AddValidatingPropertyThrottle(validatingPropertyMillisecondsDelay));
    }

    /// <summary>
    /// Property will begin validating after <paramref name="validatingPropertyDueTime" />.
    /// If property changes value while this delay, previous value won't be validated.
    /// </summary>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="validatingPropertyDueTime">The duration of the throttle period for validating property.</param>
    /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TProp">The type of validatable property.</typeparam>
    public static TNext Throttle<TObject, TProp, TNext>(
        this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
        TimeSpan validatingPropertyDueTime)
        where TNext : IRuleBuilder<TObject, TProp, TNext>
        where TObject : IValidatableObject
    {
        return ruleBuilder.Throttle(builder => builder.AddValidatingPropertyThrottle(validatingPropertyDueTime));
    }

    /// <summary>
    /// Property will begin validating after <paramref name="validatingPropertyDueTime" />.
    /// If property changes value while this delay, previous value won't be validated.
    /// </summary>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="validatingPropertyDueTime">The duration of the throttle period for validating property.</param>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TProp">The type of validatable property.</typeparam>
    public static IRuleBuilderOption<TObject, TProp> CommonThrottle<TObject, TProp>(
        this IRuleBuilderOption<TObject, TProp> ruleBuilder,
        TimeSpan validatingPropertyDueTime)
        where TObject : IValidatableObject
    {
        return ruleBuilder.CommonThrottle(builder => builder.AddValidatingPropertyThrottle(validatingPropertyDueTime));
    }

    /// <summary>
    /// Property will begin validating after specified by builder delay.
    /// If property changes value while this delay, previous value won't be validated.
    /// </summary>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="propertiesThrottleBuilderAction">Action for building properties' throttle.</param>
    /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TProp">The type of validatable property.</typeparam>
    public static TNext Throttle<TObject, TProp, TNext>(
        this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
        Action<PropertiesThrottleBuilder<TObject>> propertiesThrottleBuilderAction)
        where TNext : IRuleBuilder<TObject, TProp, TNext>
        where TObject : IValidatableObject
    {
        var builder = new PropertiesThrottleBuilder<TObject>();
        propertiesThrottleBuilderAction.Invoke(builder);
        return ruleBuilder.Throttle(builder.Build());
    }

    /// <summary>
    /// Property will begin validating after specified by builder delay.
    /// If property changes value while this delay, previous value won't be validated.
    /// </summary>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="propertiesThrottleBuilderAction">Action for building properties' throttle.</param>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TProp">The type of validatable property.</typeparam>
    public static IRuleBuilderOption<TObject, TProp> CommonThrottle<TObject, TProp>(
        this IRuleBuilderOption<TObject, TProp> ruleBuilder,
        Action<PropertiesThrottleBuilder<TObject>> propertiesThrottleBuilderAction)
        where TObject : IValidatableObject
    {
        var builder = new PropertiesThrottleBuilder<TObject>();
        propertiesThrottleBuilderAction.Invoke(builder);
        return ruleBuilder.CommonThrottle(builder.Build());
    }
}