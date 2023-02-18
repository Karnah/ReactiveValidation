using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ReactiveValidation.Extensions;
using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators.Throttle;

/// <summary>
/// Builder for <see cref="PropertiesThrottle" />.
/// </summary>
public class PropertiesThrottleBuilder<TObject>
    where TObject : IValidatableObject
{
    private TimeSpan? _validatingPropertyDueTo;
    private readonly Dictionary<string, TimeSpan> _relatedPropertiesDueTo = new();

    /// <summary>
    /// Property will begin validating after <paramref name="validatingPropertyMillisecondsDelay" />.
    /// If property changes value while this delay, previous value won't be validated.
    /// </summary>
    /// <param name="validatingPropertyMillisecondsDelay">The duration in milliseconds of the throttle period for validating property.</param>
    public PropertiesThrottleBuilder<TObject> AddValidatingPropertyThrottle(int validatingPropertyMillisecondsDelay)
    {
        return AddValidatingPropertyThrottle(TimeSpan.FromMilliseconds(validatingPropertyMillisecondsDelay));
    }

    /// <summary>
    /// Property will begin validating after <paramref name="validatingPropertyDueTime" />.
    /// If property changes value while this delay, previous value won't be validated.
    /// </summary>
    /// <param name="validatingPropertyDueTime">The duration of the throttle period for validating property.</param>
    public PropertiesThrottleBuilder<TObject> AddValidatingPropertyThrottle(TimeSpan validatingPropertyDueTime)
    {
        _validatingPropertyDueTo.GuardNotCallTwice("Validating property throttle is already specified");
        _validatingPropertyDueTo = validatingPropertyDueTime;

        return this;
    }

    /// <summary>
    /// Property will begin validating after <paramref name="relatedPropertyMillisecondsDelay" /> from changing <paramref name="property" />.
    /// If <paramref name="property" /> changes value while this delay, previous value won't be validated.
    /// </summary>
    /// <typeparam name="TProp">Type of related property.</typeparam>
    /// <param name="property">Related property.</param>
    /// <param name="relatedPropertyMillisecondsDelay">The duration of the throttle period for related property.</param>
    public PropertiesThrottleBuilder<TObject> AddRelatedPropertyThrottle<TProp>(Expression<Func<TObject, TProp>> property, int relatedPropertyMillisecondsDelay)
    {
        return AddRelatedPropertyThrottle(property, TimeSpan.FromMilliseconds(relatedPropertyMillisecondsDelay));
    }

    /// <summary>
    /// Property will begin validating after <paramref name="relatedPropertyDueTime" /> from changing <paramref name="property" />.
    /// If <paramref name="property" /> changes value while this delay, previous value won't be validated.
    /// </summary>
    /// <typeparam name="TProp">Type of related property.</typeparam>
    /// <param name="property">Related property.</param>
    /// <param name="relatedPropertyDueTime">The duration of the throttle period for related property.</param>
    public PropertiesThrottleBuilder<TObject> AddRelatedPropertyThrottle<TProp>(Expression<Func<TObject, TProp>> property, TimeSpan relatedPropertyDueTime)
    {
        var propertyName = ReactiveValidationHelper
            .GetPropertyInfo(typeof(TObject), property)
            .Name;
        return AddRelatedPropertyThrottle(propertyName, relatedPropertyDueTime);
    }

    /// <summary>
    /// Property will begin validating after <paramref name="relatedPropertyMillisecondsDelay" /> from changing <paramref name="propertyName" />.
    /// If <paramref name="propertyName" /> changes value while this delay, previous value won't be validated.
    /// </summary>
    /// <param name="propertyName">The name of related property.</param>
    /// <param name="relatedPropertyMillisecondsDelay">The duration of the throttle period for related property.</param>
    public PropertiesThrottleBuilder<TObject> AddRelatedPropertyThrottle(string propertyName, int relatedPropertyMillisecondsDelay)
    {
        return AddRelatedPropertyThrottle(propertyName, TimeSpan.FromMilliseconds(relatedPropertyMillisecondsDelay));
    }

    /// <summary>
    /// Property will begin validating after <paramref name="relatedPropertyDueTime" /> from changing <paramref name="propertyName" />.
    /// If <paramref name="propertyName" /> changes value while this delay, previous value won't be validated.
    /// </summary>
    /// <param name="propertyName">The name of related property.</param>
    /// <param name="relatedPropertyDueTime">The duration of the throttle period for related property.</param>
    public PropertiesThrottleBuilder<TObject> AddRelatedPropertyThrottle(string propertyName, TimeSpan relatedPropertyDueTime)
    {
        if (_relatedPropertiesDueTo.ContainsKey(propertyName))
            throw new ArgumentException($"Throttle for property {propertyName} is already added", nameof(propertyName));
        
        _relatedPropertiesDueTo.Add(propertyName, relatedPropertyDueTime);

        return this;
    }

    /// <summary>
    /// Create instance of <see cref="IPropertiesThrottle" /> with specified settings.
    /// </summary>
    public IPropertiesThrottle Build()
    {
        return new PropertiesThrottle(_validatingPropertyDueTo, _relatedPropertiesDueTo);
    }
}