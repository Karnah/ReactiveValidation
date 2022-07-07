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
        /// </summary>
        /// <param name="propertyInfo">Property metadata.</param>
        /// <returns>
        /// <see langword="null"/> if cannot create display name for current property.
        /// Otherwise, string source which contains property display name.
        /// </returns>
        IStringSource? GetPropertyNameSource(PropertyInfo propertyInfo);
        
        /// <summary>
        /// Get string source for property display name.
        /// </summary>
        /// <param name="objectType">Type of object, which contains property.</param>
        /// <param name="expression">Expression which call property.</param>
        /// <returns>
        /// <see langword="null"/> if cannot create display name for current property.
        /// Otherwise, string source which contains property display name.
        /// </returns>
        IStringSource? GetPropertyNameSource(Type objectType, LambdaExpression expression);
    }
}
