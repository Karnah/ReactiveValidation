using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Resources;

namespace ReactiveValidation
{
    /// <summary>
    /// Core interface of validation.
    /// Allows use all of accessible validation methods and move to <see cref="IRuleBuilderOption{TObject,TProp}"/> interface
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object</typeparam>
    /// <typeparam name="TProp">The type of validatable property</typeparam>
    /// <typeparam name="TBuilder">Rule builder type</typeparam>
    public interface IRuleBuilder<TObject, TProp, out TBuilder> :
        IRuleBuilderInitial<TObject, TProp, TBuilder>,
        IRuleBuilderOption<TObject, TProp>
            where TObject : IValidatableObject
            where TBuilder : IRuleBuilder<TObject, TProp, TBuilder>
    {
        /// <summary>
        /// Last validator will check only if the condition is true
        /// </summary>
        TBuilder When(Func<bool> condition);

        /// <summary>
        /// Last validator will check only if the condition is true
        /// </summary>
        TBuilder When<TParam>(
            Expression<Func<TObject, bool>> conditionProperty);

        /// <summary>
        /// Last validator will check only if the condition is true
        /// </summary>
        TBuilder When<TParam>(
            Expression<Func<TObject, TParam>> property,
            Func<TParam, bool> condition);

        /// <summary>
        /// Last validator will check only if the condition is true
        /// </summary>
        TBuilder When<TParam1, TParam2>(
            Expression<Func<TObject, TParam1>> property1,
            Expression<Func<TObject, TParam2>> property2,
            Func<TParam1, TParam2, bool> condition);

        /// <summary>
        /// Last validator will check only if the condition is true
        /// </summary>
        TBuilder When<TParam1, TParam2, TParam3>(
            Expression<Func<TObject, TParam1>> property1,
            Expression<Func<TObject, TParam2>> property2,
            Expression<Func<TObject, TParam3>> property3,
            Func<TParam1, TParam2, TParam3, bool> condition);



        /// <summary>
        /// Allow replace default last validator's message with static text
        /// </summary>
        /// <param name="message">Validation message text</param>
        TBuilder WithMessage(string message);

        /// <summary>
        /// Allow replace default last validator's message with localized text
        /// </summary>
        /// <param name="messageKey">Message key in default resource</param>
        TBuilder WithLocalizedMessage(string messageKey);

        /// <summary>
        /// Allow replace default last validator's message with localized text
        /// </summary>
        /// <param name="resourceManager">Resource manager</param>
        /// <param name="messageKey">Message key in resource</param>
        TBuilder WithLocalizedMessage(ResourceManager resourceManager, string messageKey);
    }


    /// <summary>
    /// Core interface of validation single property per validator.
    /// Allows use some specific validation methods (for example, validate by regex or comparison operators)
    /// Allows use all of accessible validation methods and move to <see cref="IRuleBuilderOption{TObject,TProp}"/> interface
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object</typeparam>
    /// <typeparam name="TProp">The type of validatable property</typeparam>
    public interface ISinglePropertyRuleBuilder<TObject, TProp> :
        IRuleBuilder<TObject, TProp, ISinglePropertyRuleBuilder<TObject, TProp>>,
        ISinglePropertyRuleBuilderInitial<TObject, TProp>
            where TObject : IValidatableObject
    { }


    /// <summary>
    /// Core interface of validation collection properties per validator.
    /// Allows use only the simplest methods (for example, required)
    /// Allows use all of accessible validation methods and move to <see cref="IRuleBuilderOption{TObject,TProp}"/> interface
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object</typeparam>
    public interface IPropertiesRuleBuilder<TObject> :
        IRuleBuilder<TObject, object, IPropertiesRuleBuilder<TObject>>,
        IPropertiesRuleBuilderInitial<TObject>
            where TObject : IValidatableObject
    { }


    /// <summary>
    /// Core interface of validation of collection type property.
    /// Allows use some specific validation methods for each item
    /// Has additional logic for collection collection types(for examle,  <see cref="INotifyCollectionChanged"/>)
    /// Allows use all of accessible validation methods and move to <see cref="IRuleBuilderOption{TObject,TProp}"/> interface
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object</typeparam>
    /// <typeparam name="TCollection">The type of collection</typeparam>
    /// <typeparam name="TProp">The type of element of collection</typeparam>
    public interface ICollectionRuleBuilder<TObject, TCollection, TProp> :
        IRuleBuilder<TObject, TCollection, ICollectionRuleBuilder<TObject, TCollection, TProp>>,
        ICollectionRuleBuilderInitial<TObject, TCollection, TProp>
            where TObject : IValidatableObject
            where TCollection : IEnumerable<TProp>
    { }
}
