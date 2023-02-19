using System.Threading;
using System.Threading.Tasks;

namespace ReactiveValidation.Validators.Throttle;

/// <summary>
/// Allows to setup delay before property validation execution.
/// If property changes value while this delay, previous value won't be validated.
/// </summary>
public interface IPropertiesThrottle
{
    /// <summary>
    /// Execute delay after property value changed.
    /// </summary>
    public Task DelayAsync<TObject>(ValidationContextFactory<TObject> validationContextFactory, CancellationToken cancellationToken)
        where TObject : IValidatableObject;
}