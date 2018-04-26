using System;
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


        public static ILanguageManager LanguageManager {
            get => _languageManager;
            set => _languageManager = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static Func<Type, PropertyInfo, LambdaExpression, IStringSource> DisplayNameResolver {
            get => _displayNameResolver;
            set => _displayNameResolver = value ?? DefaultDisplayNameResolver;
        }


        private static IStringSource DefaultDisplayNameResolver(Type type, PropertyInfo propertyInfo, LambdaExpression expression)
        {
            if (propertyInfo != null) {
                var displayNameAttribute = propertyInfo.GetCustomAttribute<DisplayNameAttribute>();

                return displayNameAttribute == null
                    ? null
                    : new DisplayNamePropertySource(displayNameAttribute);
            }

            if (type != null && expression != null) {
                var propertyName = ReactiveValidationHelper.GetPropertyName(type, expression);
                if (string.IsNullOrEmpty(propertyName) == true)
                    return null;

                var displayNameAttribute = type.GetProperty(propertyName)?.GetCustomAttribute<DisplayNameAttribute>();
                return displayNameAttribute == null
                    ? null
                    : new DisplayNamePropertySource(displayNameAttribute);
            }

            return null;
        }
    }
}