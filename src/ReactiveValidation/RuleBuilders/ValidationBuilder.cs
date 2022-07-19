namespace ReactiveValidation
{
    /// <inheritdoc cref="IValidationBuilder{TObject}" />
    public class ValidationBuilder<TObject> : ValidationRuleBuilder<TObject>
        where TObject : IValidatableObject
    {
    }
}
