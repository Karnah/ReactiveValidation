using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ReactiveValidation
{
    /// <summary>
    /// Class for resolving display name of properties by its metadata.
    /// </summary>
    public interface IDisplayNameResolver
    {
        /// <summary>
        /// Get string source for property display name.
        /// Property can by passed by its metadata (<paramref name="propertyInfo" /> is not null),
        /// Or by its expression (<paramref name="expression" /> is not null).
        /// </summary>
        /// <param name="type">Type of object, which contains property.</param>
        /// <param name="propertyInfo">Property metadata.</param>
        /// <param name="expression">Expression which call property.</param>
        /// <returns>
        /// <see langword="null"/> if cannot create display name for current property.
        /// Otherwise, string source which contains property display name.
        /// </returns>
        IStringSource GetPropertyNameSource(Type type, PropertyInfo propertyInfo, LambdaExpression expression);
    }
}
