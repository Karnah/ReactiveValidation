using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
using ReactiveValidation.Exceptions;
using ReactiveValidation.Internal;

namespace ReactiveValidation.Factory
{
    /// <inheritdoc />
    internal class ValidatorFactory : IValidatorFactory
    {
        private readonly Dictionary<Type, IObjectValidatorBuilder> _validatorsBuilder;

        /// <summary>
        /// Create instance of <see cref="ValidatorFactory" /> class.
        /// </summary>
        public ValidatorFactory()
        {
            _validatorsBuilder = new Dictionary<Type, IObjectValidatorBuilder>();
        }

        /// <summary>
        /// Registration of new object validator builder using its creator.
        /// </summary>
        /// <param name="creator">Creator of object validator builder.</param>
        public void Register(IObjectValidatorBuilderCreator creator)
        {
            if (creator == null)
                throw new ArgumentNullException(nameof(creator));

            var builder = creator.Create();
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (_validatorsBuilder.ContainsKey(builder.SupportedType))
                throw new ObjectValidatorBuilderAlreadyRegistered(builder.SupportedType);

            _validatorsBuilder.Add(builder.SupportedType, builder);
        }

        /// <summary>
        /// Registration of new object validator builders using its creators which searching in specified assembly.
        /// </summary>
        /// <param name="assembly">Assembly, which contains creators.</param>
        /// <param name="factoryMethod">
        /// Method, which allows get creator by its type.
        /// This can be DI method.
        /// </param>
        public void Register(Assembly assembly, Func<Type, IObjectValidatorBuilderCreator>? factoryMethod = null)
        {
            var creatorTypes = assembly
                .GetTypes()
                .Where(p => typeof(IObjectValidatorBuilderCreator).IsAssignableFrom(p));
            foreach (var creatorType in creatorTypes)
            {
                IObjectValidatorBuilderCreator creator;
                if (factoryMethod != null)
                    creator = factoryMethod.Invoke(creatorType);
                else
                    creator = (IObjectValidatorBuilderCreator) Activator.CreateInstance(creatorType);

                Register(creator);
            }
        }

        /// <inheritdoc />
        public IObjectValidator GetValidator(IValidatableObject instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            if (!TryGetValidatorBuilder(instance.GetType(), out var builder))
                throw new ObjectValidatorBuilderNotFound(instance.GetType());

            return builder.Build(instance);
        }

        /// <inheritdoc />
        public bool TryGetValidator<TObject>(IValidatableObject instance, [NotNullWhen(true)]out IObjectValidator? objectValidator)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            objectValidator = null;
            if (!TryGetValidatorBuilder(instance.GetType(), out var builder))
                return false;

            objectValidator = builder.Build(instance);
            return true;
        }

        /// <summary>
        /// Try get validator builder for specified type.
        /// If cannot find - try find validator builder for base classes.
        /// </summary>
        /// <param name="validatableObjectType">Type of validatable object.</param>
        /// <param name="builder">Found builder.</param>
        /// <returns>
        /// <see langword="true" /> if builder founded for type or base types.
        /// <see langword="false" /> otherwise.
        /// </returns>
        private bool TryGetValidatorBuilder(Type validatableObjectType,
            [NotNullWhen(true)] out IObjectValidatorBuilder? builder)
        {
            builder = null;

            while (true)
            {
                if (_validatorsBuilder.TryGetValue(validatableObjectType, out builder))
                    return true;

                if (validatableObjectType.BaseType == null)
                    return false;
                
                validatableObjectType = validatableObjectType.BaseType;
            }
        }
    }
}
