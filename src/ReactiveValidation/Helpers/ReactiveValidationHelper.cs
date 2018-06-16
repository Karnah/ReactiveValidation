using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ReactiveValidation.Helpers
{
    public static class ReactiveValidationHelper
    {
        /// <summary>
        /// Get property info by property name
        /// </summary>
        /// <param name="type">Type which contain property</param>
        /// <param name="propertyName">Property name</param>
        /// <returns>Property info or exception if property not exist</returns>
        internal static PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            const BindingFlags bindingAttributes = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var propertyInfo = type.GetProperty(propertyName, bindingAttributes);

            if (propertyInfo == null)
                throw new ArgumentException($"Property {propertyName} not found in {type} type", nameof(propertyName));

            return propertyInfo;
        }

        /// <summary>
        /// Get property info from lambda expression
        /// </summary>
        /// <param name="type">Type which contain property</param>
        /// <param name="expression">Lambda expression</param>
        /// <returns>Property info or exception if expression is not a property</returns>
        internal static PropertyInfo GetPropertyInfo(Type type, LambdaExpression expression)
        {
            var member = expression.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException($"Expression '{expression}' refers to a method, not a property.");

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException($"Expression '{expression}' refers to a field, not a property.");

            if (type != propInfo.ReflectedType && (propInfo.ReflectedType == null || type.IsSubclassOf(propInfo.ReflectedType) == false))
                throw new ArgumentException($"Expression '{expression}' refers to a property that is not from type {type}.");

            return propInfo;
        }

        /// <summary>
        /// Get property name from lambda expression
        /// </summary>
        /// <param name="type">Type which contain property</param>
        /// <param name="expression">Lambda expression</param>
        /// <returns>Property name or null if expression is not a property</returns>
        internal static string GetPropertyName(Type type, LambdaExpression expression)
        {
            var member = expression.Body as MemberExpression;
            if (member == null)
                return null;

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                return null;

            if (type != propInfo.ReflectedType && (propInfo.ReflectedType == null || type.IsSubclassOf(propInfo.ReflectedType) == false))
                return null;

            return propInfo.Name;
        }


        /// <summary>
        /// Get property type by property name
        /// </summary>
        /// <param name="type">Type which contain property</param>
        /// <param name="propertyName">Property name</param>
        /// <returns>Property type or exception if property not exist</returns>
        internal static Type GetPropertyType(Type type, string propertyName)
        {
            var propertyInfo = GetPropertyInfo(type, propertyName);

            return propertyInfo?.PropertyType;
        }

        /// <summary>
        /// Get property value from instance by property name
        /// </summary>
        /// <typeparam name="TProp">Property type</typeparam>
        /// <param name="instance">Instance</param>
        /// <param name="propertyName">Property name</param>
        /// <returns>Property type or exception if property not exist or cannot cast to type</returns>
        internal static TProp GetPropertyValue<TProp>(object instance, string propertyName)
        {
            var propertyInfo = GetPropertyInfo(instance.GetType(), propertyName);

            return (TProp)propertyInfo.GetValue(instance);
        }


        /// <summary>
        /// Get additional information about parameter for property validators
        /// </summary>
        /// <typeparam name="TObject">Type of instance</typeparam>
        /// <typeparam name="TParam">Type of parameter</typeparam>
        /// <param name="paramExpression">Expression which contain parameter</param>
        /// <returns>Property name (if parameter is property), display name source (if exists) and compiled function</returns>
        public static ParameterInfo<TObject, TParam> GetParameterInfo<TObject, TParam>(this Expression<Func<TObject, TParam>> paramExpression)
            where TObject : IValidatableObject
        {
            var paramName = GetPropertyName(typeof(TObject), paramExpression);
            var funcValue = paramExpression.Compile();

            if (string.IsNullOrEmpty(paramName) == true)
                return new ParameterInfo<TObject, TParam>(paramName, null, funcValue);

            var displayNameSource = ValidationOptions.DisplayNameResolver(typeof(TObject), null, paramExpression);
            return new ParameterInfo<TObject, TParam>(paramName, displayNameSource, funcValue);
        }
    }
}
