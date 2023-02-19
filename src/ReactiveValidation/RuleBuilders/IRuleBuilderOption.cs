using ReactiveValidation.Validators.Conditions;
using ReactiveValidation.Validators.Throttle;

namespace ReactiveValidation
{
    /// <summary>
    /// Interface for options of validation builder.
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TProp">The type of validatable property.</typeparam>
    public interface IRuleBuilderOption<TObject, TProp>
        where TObject : IValidatableObject
    {
        /// <summary>
        /// The validation of the rule will occur only if the condition is <see langword="true" />.
        /// </summary>
        IRuleBuilderOption<TObject, TProp> AllWhen(IValidationCondition<TObject> validationCondition);

        /// <summary>
        /// Allows to setup delay before property validation execution.
        /// If property changes value while this delay, previous value won't be validated.
        /// </summary>
        IRuleBuilderOption<TObject, TProp> CommonThrottle(IPropertiesThrottle propertiesThrottle);
    }
}
