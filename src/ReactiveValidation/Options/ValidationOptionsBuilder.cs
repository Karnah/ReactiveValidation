using System;
using System.Collections.Generic;
using System.Reflection;

using ReactiveValidation.Attributes;
using ReactiveValidation.Exceptions;
using ReactiveValidation.Resources.StringProviders;
using ReactiveValidation.Resources.StringSources;
using ReactiveValidation.ValidatorFactory;

namespace ReactiveValidation
{
    /// <summary>
    /// Class which allows setup validation options.
    /// </summary>
    public class ValidationOptionsBuilder
    {
        private bool _isUseStringProviderCalled;
        private bool _isTrackCultureChangedCalled;
        private bool _isUseCustomDisplayNameResolverCalled;
        private bool _isUseCustomValidatorFactoryCalled;
        private bool _isUsedDefaultValidatorFactory;

        /// <summary>
        /// Set string provider for text and localized strings.
        /// It will use by <see cref="DisplayNameAttribute" /> and <see cref="LanguageStringSource" />.
        /// </summary>
        /// <param name="stringProvider">String provider.</param>
        public ValidationOptionsBuilder UseStringProvider(IStringProvider stringProvider)
        {
            if (stringProvider == null)
                throw new ArgumentNullException(nameof(stringProvider));

            if (_isUseStringProviderCalled)
                throw new MethodAlreadyCalledException("Cannot change string provider twice");

            ValidationOptions.LanguageManager.StringProvider = stringProvider;

            _isUseStringProviderCalled = true;
            return this;
        }

        /// <summary>
        /// Allows update all messages immediately when the culture changed.
        /// Warning! Call to only if you need to change culture runtime.
        /// </summary>
        public ValidationOptionsBuilder TrackCultureChanged()
        {
            if (_isTrackCultureChangedCalled)
                throw new MethodAlreadyCalledException("Culture changed already tracked");

            ValidationOptions.LanguageManager.TrackCultureChanged = true;

            _isTrackCultureChangedCalled = true;
            return this;
        }

        /// <summary>
        /// Use custom class for resolving display name of properties.
        /// </summary>
        /// <param name="displayNameResolver">Custom display name resolver.</param>
        public ValidationOptionsBuilder UseCustomDisplayNameResolver(IDisplayNameResolver displayNameResolver)
        {
            if (displayNameResolver == null)
                throw new ArgumentNullException(nameof(displayNameResolver));

            if (_isUseCustomDisplayNameResolverCalled)
                throw new MethodAlreadyCalledException("Cannot change display name resolver twice");

            ValidationOptions.DisplayNameResolver = displayNameResolver;

            _isUseCustomDisplayNameResolverCalled = true;
            return this;
        }

        /// <summary>
        /// Use custom factory for creating object validators.
        /// </summary>
        /// <param name="validatorFactory">Custom validator factory.</param>
        public ValidationOptionsBuilder UseCustomValidatorFactory(IValidatorFactory validatorFactory)
        {
            if (validatorFactory == null)
                throw new ArgumentNullException(nameof(validatorFactory));

            if (_isUseCustomValidatorFactoryCalled)
                throw new MethodAlreadyCalledException("Cannot set validator factory twice");

            if (_isUsedDefaultValidatorFactory)
                throw new MethodAlreadyCalledException("Already was called methods for setup of default validation factory");

            ValidationOptions.ValidatorFactory = validatorFactory;

            _isUseCustomValidatorFactoryCalled = true;
            return this;
        }

        /// <summary>
        /// Registration of new object validator builder using its creator.
        /// </summary>
        /// <param name="creator">Creator of object validator builder.</param>
        public ValidationOptionsBuilder RegisterForValidatorFactory(IObjectValidatorBuilderCreator creator)
        {
            if (creator == null)
                throw new ArgumentNullException(nameof(creator));

            var factory = GetDefaultValidatorFactory();
            factory.Register(creator);

            _isUsedDefaultValidatorFactory = true;
            return this;
        }

        /// <summary>
        /// Registration of new object validator builders using its creators.
        /// </summary>
        /// <param name="creators">Creators of object validator builders.</param>
        public ValidationOptionsBuilder RegisterForValidatorFactory(IEnumerable<IObjectValidatorBuilderCreator> creators)
        {
            if (creators == null)
                throw new ArgumentNullException(nameof(creators));

            var factory = GetDefaultValidatorFactory();
            foreach (var creator in creators)
            {
                if (creator == null)
                    throw new NullReferenceException("One of creators is equal to null");

                factory.Register(creator);
            }

            _isUsedDefaultValidatorFactory = true;
            return this;
        }

        /// <summary>
        /// Registration of new object validator builders using its creators which searching in specified assembly.
        /// </summary>
        /// <param name="assembly">Assembly, which contains creators.</param>
        /// <param name="factoryMethod">
        /// Method, which allows get creator by its type.
        /// This can be DI method.
        /// </param>
        public ValidationOptionsBuilder RegisterForValidatorFactory(Assembly assembly, Func<Type, IObjectValidatorBuilderCreator>? factoryMethod = null)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            var factory = GetDefaultValidatorFactory();
            factory.Register(assembly, factoryMethod);

            _isUsedDefaultValidatorFactory = true;
            return this;
        }

        /// <summary>
        /// Registration of new object validator builders using its creators which searching in specified assembly.
        /// </summary>
        /// <param name="assemblies">Assemblies, which contains creators.</param>
        /// <param name="factoryMethod">
        /// Method, which allows get creator by its type.
        /// This can be DI method.
        /// </param>
        public ValidationOptionsBuilder RegisterForValidatorFactory(IEnumerable<Assembly> assemblies, Func<Type, IObjectValidatorBuilderCreator>? factoryMethod = null)
        {
            if (assemblies == null)
                throw new ArgumentNullException(nameof(assemblies));

            var factory = GetDefaultValidatorFactory();
            foreach (var assembly in assemblies)
            {
                if (assembly == null)
                    throw new NullReferenceException("One of assemblies is equal to null");

                factory.Register(assembly, factoryMethod);
            }

            _isUsedDefaultValidatorFactory = true;
            return this;
        }

        /// <summary>
        /// Get or create default validator factory.
        /// </summary>
        private DefaultValidatorFactory GetDefaultValidatorFactory()
        {
            if (_isUseCustomValidatorFactoryCalled)
                throw new MethodAlreadyCalledException("Cannot set register creator for custom validation factory");

            return (DefaultValidatorFactory) ValidationOptions.ValidatorFactory;
        }
    }
}
