using System;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation
{
    /// <summary>
    /// Cached information about validator parameter.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    /// <typeparam name="TParam">Type of parameter.</typeparam>
    public class ValidatorParameter<TObject, TParam>
        where TObject : IValidatableObject
    {
        /// <summary>
        /// Create new instance of validator parameter.
        /// </summary>
        /// <param name="name">Name of parameter.</param>
        /// <param name="displayNameSource">Source of display name of parameter.</param>
        /// <param name="funcValue">Function for getting value.</param>
        public ValidatorParameter(string name, IStringSource displayNameSource, Func<TObject, TParam> funcValue)
        {
            Name = name;
            DisplayNameSource = displayNameSource;
            FuncValue = funcValue;
        }

        /// <summary>
        /// Create new instance of validator parameter.
        /// </summary>
        /// <param name="paramExpression">Parameter expression.</param>
        public ValidatorParameter(Expression<Func<TObject, TParam>> paramExpression)
        {
            Name = ReactiveValidationHelper.GetPropertyName(typeof(TObject), paramExpression);
            FuncValue = paramExpression.Compile();

            if (!string.IsNullOrEmpty(Name))
            {
                DisplayNameSource = ValidationOptions
                    .DisplayNameResolver
                    .GetPropertyNameSource(typeof(TObject), null, paramExpression);;
            }
        }


        /// <summary>
        /// Name of parameter.
        /// <see langword="null" /> if parameter is constant value.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Source of display name of parameter.
        /// <see langword="null" /> if parameter is constant value.
        /// </summary>
        public IStringSource DisplayNameSource { get; }

        /// <summary>
        /// Function for getting value.
        /// </summary>
        public Func<TObject, TParam> FuncValue { get; }
    }
}
