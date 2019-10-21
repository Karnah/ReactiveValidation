using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;
using ReactiveValidation.Internal;

namespace ReactiveValidation
{
    /// <summary>
    /// Defines a validation rules for an object.
    /// Derive from this class to create validation rules for object.
    /// </summary>
    /// <typeparam name="TObject">Type of validatable object.</typeparam>
    public abstract class ValidationRuleBuilder<TObject> : IObjectValidatorBuilderCreator
        where TObject : IValidatableObject
    {
        private readonly List<IRuleBuilder<TObject>> _rulesBuilders;

        /// <inheritdoc />
        protected ValidationRuleBuilder()
        {
            _rulesBuilders = new List<IRuleBuilder<TObject>>();
        }


        /// <inheritdoc />
        IObjectValidatorBuilder IObjectValidatorBuilderCreator.Create()
        {
            return CreateBuilder();
        }


        /// <summary>
        /// Create validator for single strongly typed property.
        /// </summary>
        /// <typeparam name="TProp">The type of property.</typeparam>
        /// <param name="property">Validatable property.</param>
        /// <returns>Single property validator for <see cref="TProp"/> type.</returns>
        protected ISinglePropertyRuleBuilderInitial<TObject, TProp> RuleFor<TProp>(Expression<Func<TObject, TProp>> property)
        {
            var propertyName = GetPropertyNameForValidator(property);
            var ruleBuilder = new SinglePropertyRuleBuilder<TObject, TProp>(propertyName);

            _rulesBuilders.Add(ruleBuilder);

            return ruleBuilder;
        }

        /// <summary>
        /// Create validator for single property with object type by its name.
        /// </summary>
        /// <param name="propertyName">Name of validatable property.</param>
        /// <returns>Single property validator for <see cref="object"/> type.</returns>
        protected ISinglePropertyRuleBuilderInitial<TObject, object> RuleFor(string propertyName)
        {
            var ruleBuilder = new SinglePropertyRuleBuilder<TObject, object>(propertyName);
            _rulesBuilders.Add(ruleBuilder);

            return ruleBuilder;
        }


        /// <summary>
        /// Create validator for collection of properties.
        /// </summary>
        /// <param name="properties">Validatable properties.</param>
        /// <returns>Properties collection validator.</returns>
        protected IPropertiesRuleBuilderInitial<TObject> RuleFor(params Expression<Func<TObject, object>>[] properties)
        {
            var propertiesNames = properties.Select(GetPropertyNameForValidator).ToArray();

            return RuleFor(propertiesNames);
        }

        /// <summary>
        /// Create validator for collection of properties names.
        /// </summary>
        /// <param name="propertiesNames">Names of validatable properties.</param>
        /// <returns>Properties collection validator.</returns>
        protected IPropertiesRuleBuilderInitial<TObject> RuleFor(params string[] propertiesNames)
        {
            var ruleBuilder = new PropertiesRuleBuilder<TObject>(propertiesNames);
            _rulesBuilders.Add(ruleBuilder);

            return ruleBuilder;
        }


        /// <summary>
        /// Create validator for property with collection type, i.e. <see cref="IEnumerable{T}"/> interface.
        /// </summary>
        /// <typeparam name="TProp">The type of element of collection.</typeparam>
        /// <param name="collection">Property with collection type.</param>
        /// <returns>Validator for property with <see cref="IEnumerable{T}"/> type.</returns>
        protected ICollectionRuleBuilderInitial<TObject, IEnumerable<TProp>, TProp> RuleForCollection<TProp>(
            Expression<Func<TObject, IEnumerable<TProp>>> collection)
        {
            return RuleForStrongTypedCollection<IEnumerable<TProp>, TProp>(collection);
        }

        /// <summary>
        /// Created validator for strongly typed collection.
        /// </summary>
        /// <typeparam name="TCollection">The type of collection.</typeparam>
        /// <typeparam name="TProp">The type of element of collection.</typeparam>
        /// <param name="collection">Property with collection type.</param>
        /// <returns>Validator for property with <see cref="TCollection"/> type.</returns>
        protected ICollectionRuleBuilderInitial<TObject, TCollection, TProp> RuleForStrongTypedCollection<TCollection, TProp>(
            Expression<Func<TObject, TCollection>> collection)
            where TCollection : IEnumerable<TProp>
        {
            var propertyName = GetPropertyNameForValidator(collection);
            var ruleBuilder = new CollectionPropertyRuleBuilder<TObject, TCollection, TProp>(propertyName);

            _rulesBuilders.Add(ruleBuilder);

            return ruleBuilder;
        }


        /// <summary>
        /// Create prepared builder.
        /// </summary>
        internal IObjectValidatorBuilder CreateBuilder()
        {
            return new ObjectValidatorBuilder<TObject>(_rulesBuilders);
        }

        /// <summary>
        /// Get property name by expression for validator. Throws ArgumentException, if couldn't validate this property.
        /// </summary>
        /// <typeparam name="TProp">The type of element of collection.</typeparam>
        /// <param name="property">Expression which contains property.</param>
        /// <returns>Property name.</returns>
        private static string GetPropertyNameForValidator<TProp>(Expression<Func<TObject, TProp>> property)
        {
            var propertyInfo = ReactiveValidationHelper.GetPropertyInfo(typeof(TObject), property);
            return propertyInfo.Name;
        }
    }
}