using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using ReactiveValidation.ObjectObserver;
using ReactiveValidation.Resources.StringSources;
using ReactiveValidation.Validators;
using ReactiveValidation.Validators.Conditions;

namespace ReactiveValidation
{
    /// <summary>
    /// Core interface of validation.
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    internal interface IRuleBuilder<TObject>
        where TObject : IValidatableObject
    {
        /// <summary>
        /// Specifies how rules of property should cascade when one fails.
        /// </summary>
        public CascadeMode? PropertyCascadeMode { get; set; }
        
        /// <summary>
        /// List of properties names which validating by this rules.
        /// </summary>
        IReadOnlyList<string> ValidatableProperties { get; }

        /// <summary>
        /// Settings of validatable properties.
        /// </summary>
        ObservingPropertySettings ObservingPropertiesSettings { get; }

        /// <summary>
        /// Get all registered validators.
        /// </summary>
        IReadOnlyList<IPropertyValidator<TObject>> GetValidators();
    }

    /// <summary>
    /// Core interface of validation.
    /// Allows use all of accessible validation methods and move to <see cref="IRuleBuilderOption{TObject,TProp}" /> interface.
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TProp">The type of validatable property.</typeparam>
    /// <typeparam name="TBuilder">Rule builder type.</typeparam>
    public interface IRuleBuilder<TObject, TProp, out TBuilder> :
        IRuleBuilderInitial<TObject, TProp, TBuilder>,
        IRuleBuilderOption<TObject, TProp>
            where TObject : IValidatableObject
            where TBuilder : IRuleBuilder<TObject, TProp, TBuilder>
    {
        /// <summary>
        /// Last validator will check according to condition.
        /// </summary>
        TBuilder When(IValidationCondition<TObject> condition);

        /// <summary>
        /// Allow replace default last validator's message with string from source.
        /// </summary>
        /// <param name="stringSource">Validation message source.</param>
        TBuilder WithMessageSource(IStringSource stringSource);
    }


    /// <summary>
    /// Core interface of validation single property per validator.
    /// Allows use some specific validation methods (for example, validate by regex or comparison operators).
    /// Allows use all of accessible validation methods and move to <see cref="IRuleBuilderOption{TObject,TProp}" /> interface.
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TProp">The type of validatable property.</typeparam>
    public interface ISinglePropertyRuleBuilder<TObject, TProp> :
        IRuleBuilder<TObject, TProp, ISinglePropertyRuleBuilder<TObject, TProp>>,
        ISinglePropertyRuleBuilderInitial<TObject, TProp>
            where TObject : IValidatableObject
    { }


    /// <summary>
    /// Core interface of validation collection properties per validator.
    /// Allows use only the simplest methods (for example, required).
    /// Allows use all of accessible validation methods and move to <see cref="IRuleBuilderOption{TObject,TProp}" /> interface.
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    public interface IPropertiesRuleBuilder<TObject> :
        IRuleBuilder<TObject, object?, IPropertiesRuleBuilder<TObject>>,
        IPropertiesRuleBuilderInitial<TObject>
            where TObject : IValidatableObject
    { }


    /// <summary>
    /// Core interface of validation of collection type property.
    /// Allows use some specific validation methods for each item.
    /// Has additional logic for collection collection types(for example,  <see cref="INotifyCollectionChanged" />).
    /// Allows use all of accessible validation methods and move to <see cref="IRuleBuilderOption{TObject,TProp}" /> interface.
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TCollection">The type of collection.</typeparam>
    /// <typeparam name="TItem">The type of element of collection.</typeparam>
    public interface ICollectionRuleBuilder<TObject, TCollection, TItem> :
        IRuleBuilder<TObject, TCollection, ICollectionRuleBuilder<TObject, TCollection, TItem>>,
        ICollectionRuleBuilderInitial<TObject, TCollection, TItem>
            where TObject : IValidatableObject
            where TCollection : IEnumerable<TItem>
    { }
}
