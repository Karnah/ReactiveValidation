namespace ReactiveValidation.Internal
{
    internal class AdapterBuilderWrapper<TObject>
        where TObject : IValidatableObject
    {
        public AdapterBuilderWrapper(IAdapterBuilder<TObject> builder, params string[] targetProperties)
        {
            Builder = builder;
            TargetProperties = targetProperties;
        }


        public IAdapterBuilder<TObject> Builder { get; }

        public string[] TargetProperties { get; }
    }
}
