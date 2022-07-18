using System;
using System.Collections.Generic;

namespace ReactiveValidation
{
    /// <inheritdoc cref="IObjectValidatorBuilder" />
    internal class ObjectValidatorBuilder<TObject> : IObjectValidatorBuilder
        where TObject : IValidatableObject
    {
        private readonly IReadOnlyList<IRuleBuilder<TObject>> _rulesBuilders;

        /// <summary>
        /// Create validation builder with specified adapters.
        /// </summary>
        public ObjectValidatorBuilder(IReadOnlyList<IRuleBuilder<TObject>> rulesBuilders)
        {
            _rulesBuilders = rulesBuilders;
        }

        /// <inheritdoc />
        public Type SupportedType => typeof(TObject);

        /// <inheritdoc />
        public IObjectValidator Build(IValidatableObject instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            if (instance is TObject i)
                return new ObjectValidator<TObject>(i, _rulesBuilders);

            throw new NotSupportedException($"Cannot create validator for type {instance.GetType()}, supported only {typeof(TObject)}");
        }
    }
}
