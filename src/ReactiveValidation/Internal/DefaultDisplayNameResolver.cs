using System;
using System.Linq.Expressions;
using System.Reflection;

using ReactiveValidation.Attributes;
using ReactiveValidation.Helpers;

namespace ReactiveValidation.Internal
{
    /// <summary>
    /// Display name resolver, which uses <see cref="DisplayNameAttribute" />.
    /// </summary>
    internal class DefaultDisplayNameResolver : IDisplayNameResolver
    {
        /// <inheritdoc />
        public IStringSource GetPropertyNameSource(Type type, PropertyInfo propertyInfo, LambdaExpression expression)
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
    }
}
