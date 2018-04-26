using ReactiveValidation.Adapters;

namespace ReactiveValidation
{
    internal interface IAdapterBuilder<TObject>
        where TObject : IValidatableObject
    {
        IPropertiesAdapter Build(ObjectValidator<TObject> validator, params string[] properties);
    }
}
