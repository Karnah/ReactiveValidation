using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ReactiveValidation.Helpers
{
    public static class ReactiveValidationHelper
    {
        internal static PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            const BindingFlags bindingAttributes = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var propertyInfo = type.GetProperty(propertyName, bindingAttributes);

            if (propertyInfo == null)
                throw new ArgumentException($"Property {propertyName} not found in {type} type", nameof(propertyName));

            return propertyInfo;
        }

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


        internal static string GetPropertyName(Type type, LambdaExpression expression)
        {
            try {
                var propertyInfo = GetPropertyInfo(type, expression);
                return propertyInfo.Name;
            }
            catch (ArgumentException) {
                return null;
            }
        }

        internal static Type GetPropertyType(Type type, string propertyName)
        {
            var propertyInfo = GetPropertyInfo(type, propertyName);

            return propertyInfo?.PropertyType;
        }

        internal static TProp GetPropertyValue<TProp>(object instance, string propertyName)
        {
            var propertyInfo = GetPropertyInfo(instance.GetType(), propertyName);

            return (TProp)propertyInfo.GetValue(instance);
        }

        internal static TParam GetParamFuncValue<TParam>(object instance, Func<object, TParam> paramFunc)
        {
            var paramValue = paramFunc.Invoke(instance);
            return paramValue;
        }


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
