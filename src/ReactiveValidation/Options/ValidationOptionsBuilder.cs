using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;

using ReactiveValidation.Attributes;
using ReactiveValidation.Exceptions;
using ReactiveValidation.Factory;
using ReactiveValidation.Internal;

namespace ReactiveValidation
{
    /// <summary>
    /// Class which allows setup validation options.
    /// </summary>
    public class ValidationOptionsBuilder
    {
        private bool _isUseDefaultResourceManagerCalled;
        private bool _isTrackCultureChangedCalled;
        private bool _isUseCustomDisplayNameResolverCalled;
        private bool _isUseCustomValidatorFactoryCalled;
        private bool _isUsedDefaultValidatorFactory;

        /// <summary>
        /// Set passed resource manager as default.
        /// It will allow create <see cref="DisplayNameAttribute" /> and <see cref="LanguageStringSource" /> without passing ResourceManager.
        /// </summary>
        /// <param name="resourceManager">Default resource manager.</param>
        public ValidationOptionsBuilder UseDefaultResourceManager(ResourceManager resourceManager)
        {
            if (resourceManager == null)
                throw new ArgumentNullException(nameof(resourceManager));

            if (_isUseDefaultResourceManagerCalled)
                throw new MethodAlreadyCalledException("Cannot change default resource manager twice");

            ValidationOptions.LanguageManager.DefaultResourceManager = resourceManager;

            _isUseDefaultResourceManagerCalled = true;
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
        public ValidationOptionsBuilder RegisterForValidatorFactory(Assembly assembly, Func<Type, IObjectValidatorBuilderCreator> factoryMethod = null)
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
        public ValidationOptionsBuilder RegisterForValidatorFactory(IEnumerable<Assembly> assemblies, Func<Type, IObjectValidatorBuilderCreator> factoryMethod = null)
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
        /// Add auto creation observers which can allows revalidate property by event.
        /// </summary>
        /// <param name="canObserve">
        /// Function which allows check if can create observer for property.
        /// First parameter - type of validatable object.
        /// Second parameter - type of property.
        /// Returns <see langword="true" /> if observer can be created.
        /// </param>
        /// <param name="createObserver">
        /// Function which allow create observer for property value.
        /// First parameter - value of validatable object.
        /// Second parameter - value of property.
        /// Third parameter - action which should be executed when event fired.
        /// Returns object which allows unsubscribe.
        /// </param>
        public ValidationOptionsBuilder AddPropertyObserver(Func<Type, Type, bool> canObserve,
            Func<object, object, Action, IDisposable> createObserver)
        {
            if (canObserve == null)
                throw new ArgumentNullException(nameof(canObserve));

            if (createObserver == null)
                throw new ArgumentNullException(nameof(createObserver));

            ValidationOptions.PropertyObservers.Add(new ObserverInfo(canObserve, createObserver));
            return this;
        }

        /// <summary>
        /// Add auto creation observers which can allows revalidate collection by event.
        /// </summary>
        /// <param name="canObserve">
        /// Function which allows check if can create observer for collection.
        /// First parameter - type of validatable object.
        /// Second parameter - type of collection.
        /// Returns <see langword="true" /> if observer can be created.
        /// </param>
        /// <param name="createObserver">
        /// Function which allow create observer for collection.
        /// First parameter - value of validatable object.
        /// Second parameter - value of collection.
        /// Third parameter - action which should be executed when event fired.
        /// Returns object which allows unsubscribe.
        /// </param>
        public ValidationOptionsBuilder AddCollectionObserver(Func<Type, Type, bool> canObserve,
            Func<object, object, Action, IDisposable> createObserver)
        {
            if (canObserve == null)
                throw new ArgumentNullException(nameof(canObserve));

            if (createObserver == null)
                throw new ArgumentNullException(nameof(createObserver));

            ValidationOptions.CollectionObservers.Add(new ObserverInfo(canObserve, createObserver));
            return this;
        }

        /// <summary>
        /// Get or create default validator factory.
        /// </summary>
        private ValidatorFactory GetDefaultValidatorFactory()
        {
            if (_isUseCustomValidatorFactoryCalled)
                throw new MethodAlreadyCalledException("Cannot set register creator for custom validation factory");

            if (ValidationOptions.ValidatorFactory == null)
                ValidationOptions.ValidatorFactory = new ValidatorFactory();

            return (ValidatorFactory) ValidationOptions.ValidatorFactory;
        }
    }
}
