using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ReactiveValidation
{
    public interface IValidationBuilder <TObject>
        where TObject : IValidatableObject
    {
        /// <summary>
        /// Create validator for single strongly typed property
        /// </summary>
        /// <typeparam name="TProp">The type of property</typeparam>
        /// <param name="property">Validatable property</param>
        /// <returns>Single property validator for <see cref="TProp"/> type</returns>
        ISinglePropertyRuleBuilderInitial<TObject, TProp> RuleFor<TProp>(Expression<Func<TObject, TProp>> property);

        /// <summary>
        /// Create validator for single property with object type by its name
        /// </summary>
        /// <param name="propertyName">Name of validatable property</param>
        /// <returns>Single property validator for <see cref="object"/> type</returns>
        ISinglePropertyRuleBuilderInitial<TObject, object> RuleFor(string propertyName);


        /// <summary>
        /// Create validator for collection of properties
        /// </summary>
        /// <param name="properties">Validatable properties</param>
        /// <returns>Properties collection validator</returns>
        IPropertiesRuleBuilderInitial<TObject> RuleFor(params Expression<Func<TObject, object>>[] properties);

        /// <summary>
        /// Create validator for collection of properties names
        /// </summary>
        /// <param name="propertiesNames">Names of validatable properties</param>
        /// <returns>Properties collection validator</returns>
        IPropertiesRuleBuilderInitial<TObject> RuleFor(params string[] propertiesNames);


        /// <summary>
        /// Create validator for property with collection type, i.e. <see cref="IEnumerable{T}"/> interface
        /// </summary>
        /// <typeparam name="TProp">The type of element of collection</typeparam>
        /// <param name="collection">Property with collection type</param>
        /// <returns>Validator for property with <see cref="IEnumerable{T}"/> type</returns>
        ICollectionRuleBuilderInitial<TObject, IEnumerable<TProp>, TProp> RuleForCollection<TProp>(
            Expression<Func<TObject, IEnumerable<TProp>>> collection);

        /// <summary>
        /// Created validator for strongly typed collection
        /// </summary>
        /// <typeparam name="TCollection">The type of collection</typeparam>
        /// <typeparam name="TProp">The type of element of collection</typeparam>
        /// <param name="collection">Property with collection type</param>
        /// <returns>Validator for property with <see cref="TCollection"/> type</returns>
        ICollectionRuleBuilderInitial<TObject, TCollection, TProp> RuleForStrongTypedCollection<TCollection, TProp>(
            Expression<Func<TObject, TCollection>> collection)
                where TCollection : IEnumerable<TProp>;
    }
}
