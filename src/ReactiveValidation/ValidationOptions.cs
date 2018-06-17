using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using ReactiveValidation.Attributes;
using ReactiveValidation.Helpers;

namespace ReactiveValidation
{
    public static class ValidationOptions
    {
        private static ILanguageManager _languageManager = new LanguageManager();
        private static Func<Type, PropertyInfo, LambdaExpression, IStringSource> _displayNameResolver = DefaultDisplayNameResolver;


        public static ILanguageManager LanguageManager
        {
            get => _languageManager;
            set => _languageManager = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static Func<Type, PropertyInfo, LambdaExpression, IStringSource> DisplayNameResolver
        {
            get => _displayNameResolver;
            set => _displayNameResolver = value ?? DefaultDisplayNameResolver;
        }


        internal static List<ObserverInfo> PropertyObservers { get; } = new List<ObserverInfo>();

        internal static List<ObserverInfo> CollectionObservers { get; } = new List<ObserverInfo>();


        public static void AddPropertyObserver(Func<Type, Type, bool> canObserve, Func<object, object, Action, IDisposable> createObserver)
        {
            PropertyObservers.Add(new ObserverInfo(canObserve, createObserver));
        }

        public static void AddCollectionObserver(Func<Type, Type, bool> canObserve, Func<object, object, Action, IDisposable> createObserver)
        {
            CollectionObservers.Add(new ObserverInfo(canObserve, createObserver));
        }


        private static IStringSource DefaultDisplayNameResolver(Type type, PropertyInfo propertyInfo, LambdaExpression expression)
        {
            if (propertyInfo != null)
            {
                var displayNameAttribute = propertyInfo.GetCustomAttribute<DisplayNameAttribute>();

                return displayNameAttribute == null
                    ? null
                    : new DisplayNamePropertySource(displayNameAttribute);
            }

            if (type != null && expression != null)
            {
                var propertyName = ReactiveValidationHelper.GetPropertyName(type, expression);
                if (string.IsNullOrEmpty(propertyName))
                    return null;

                var displayNameAttribute = type.GetProperty(propertyName)?.GetCustomAttribute<DisplayNameAttribute>();
                return displayNameAttribute == null
                    ? null
                    : new DisplayNamePropertySource(displayNameAttribute);
            }

            return null;
        }


        internal class ObserverInfo
        {
            private readonly Func<Type, Type, bool> _canObserve;
            private readonly Func<object, object, Action, IDisposable> _createObserver;

            public ObserverInfo(Func<Type, Type, bool> canObserve, Func<object, object, Action, IDisposable> createObserver)
            {
                _canObserve = canObserve;
                _createObserver = createObserver;
            }


            public bool CanObserve(Type instanceType, Type propertyType)
            {
                return _canObserve.Invoke(instanceType, propertyType);
            }

            public bool CanObserve<TObject, TProp>()
                where TObject : IValidatableObject
            {
                return CanObserve(typeof(TObject), typeof(TProp));
            }


            public IDisposable CreateObserver<TObject, TProp>(TObject instance, TProp property, Action action)
            {
                return _createObserver.Invoke(instance, property, action);
            }
        }
    }
}