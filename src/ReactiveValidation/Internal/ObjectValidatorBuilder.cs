using System;
using System.Collections.Generic;

namespace ReactiveValidation.Internal
{
    /// <inheritdoc cref="IObjectValidatorBuilder" />
    internal class ObjectValidatorBuilder<TObject> : IObjectValidatorBuilder<TObject>, IObjectValidatorBuilder
        where TObject : IValidatableObject
    {
        private readonly IReadOnlyList<AdapterBuilderWrapper<TObject>> _adaptersBuilders;

        /// <summary>
        /// Create validation builder with specified adapters.
        /// </summary>
        public ObjectValidatorBuilder(IReadOnlyList<AdapterBuilderWrapper<TObject>> adaptersBuilders)
        {
            _adaptersBuilders = adaptersBuilders;
        }

        /// <inheritdoc />
        public Type SupportedType => typeof(TObject);

        /// <inheritdoc />
        public IObjectValidator Build(TObject instance)
        {
            var validator = new ObjectValidator<TObject>(instance);

            foreach (var adaptersBuilder in _adaptersBuilders)
            {
                var adapter = adaptersBuilder.Builder.Build(validator, adaptersBuilder.TargetProperties);
                validator.RegisterAdapter(adapter, adaptersBuilder.TargetProperties);
            }

            validator.Revalidate();

            return validator;
        }

        /// <inheritdoc />
        public IObjectValidator Build(IValidatableObject instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            if (instance is TObject i)
                return Build(i);

            throw new NotSupportedException($"Cannot create validator for type {instance.GetType()}, supported only {typeof(TObject)}");
        }
    }
}
