using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReactiveValidation.ValidatorFactory;

namespace ReactiveValidation.Validators.Throttle;

/// <inheritdoc />
public class PropertiesThrottle : IPropertiesThrottle
{
    private readonly TimeSpan? _validatingPropertyDueTo;
    private readonly Dictionary<string, TimeSpan> _relatedPropertiesDueTo;

    /// <summary>
    /// Create instance of <see cref="PropertiesThrottle" /> class.
    /// </summary>
    /// <param name="validatingPropertyDueTo">Delay for target validating property.</param>
    /// <param name="relatedPropertiesDueTo">Delays for other related properties.</param>
    public PropertiesThrottle(TimeSpan? validatingPropertyDueTo, Dictionary<string, TimeSpan> relatedPropertiesDueTo)
    {
        _validatingPropertyDueTo = validatingPropertyDueTo;
        _relatedPropertiesDueTo = relatedPropertiesDueTo;
    }

    /// <inheritdoc />
    public async Task DelayAsync<TObject>(
        ValidationContextFactory<TObject> validationContextFactory,
        CancellationToken cancellationToken)
        where TObject : IValidatableObject
    {
        if (_validatingPropertyDueTo != null)
        {
            await DelayPropertyAsync(
                validationContextFactory.PropertyChangedStopwatches,
                validationContextFactory.PropertyName,
                _validatingPropertyDueTo.Value,
                cancellationToken).ConfigureAwait(false);
        }

        foreach (var relatedProperty in _relatedPropertiesDueTo)
        {
            await DelayPropertyAsync(
                validationContextFactory.PropertyChangedStopwatches,
                relatedProperty.Key,
                relatedProperty.Value,
                cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Execute delay after property value changed.
    /// </summary>
    private static Task DelayPropertyAsync(
        IReadOnlyDictionary<string, PropertyChangedStopwatch> propertyChangedStopwatches,
        string propertyName,
        TimeSpan propertyDueTo,
        CancellationToken cancellationToken)
    {
        if (!propertyChangedStopwatches.TryGetValue(propertyName, out var propertyChangedStopwatch))
            return Task.CompletedTask;

        return propertyChangedStopwatch.WaitUntilAsync(propertyDueTo, cancellationToken);
    }
}