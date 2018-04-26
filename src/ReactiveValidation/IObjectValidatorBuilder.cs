namespace ReactiveValidation
{
    public interface IObjectValidatorBuilder<in TObject>
        where TObject : IValidatableObject
    {
        IObjectValidator Build(TObject instance);
    }
}
