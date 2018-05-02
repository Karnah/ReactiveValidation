using System.Collections.Generic;
using System.Collections.Specialized;

using ReactiveValidation.Validators;

namespace ReactiveValidation
{
    /// <summary>
    /// Base interface for the first step of validation.
    /// It is necessary for it to be impossible to immediately call methods
    /// from <see cref="IRuleBuilder{TObject,TProp, TNext}"/> or <see cref="IRuleBuilderOption{TObject,TProp}"/>
    /// interfaces
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object</typeparam>
    /// <typeparam name="TProp">The type of validatable property</typeparam>
    /// <typeparam name="TBuilder">The next type from which can move to condtion or message step</typeparam>
    public interface IRuleBuilderInitial<TObject, TProp, out TBuilder>
        where TBuilder : IRuleBuilder<TObject, TProp, TBuilder>
        where TObject : IValidatableObject
    {
        TBuilder SetValidator(IPropertyValidator<TObject, TProp> validator);
    }


    /// <summary>
    /// Interface for the first step of validation single property per validator.
    /// Allows use some specific validation methods (for example, validate by regex or comparison operators)
    /// Interface for first step is necessary for it to be impossible to immediately call methods
    /// from <see cref="IRuleBuilder{TObject,TProp, TNext}"/> or <see cref="IRuleBuilderOption{TObject,TProp}"/>
    /// interfaces
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object</typeparam>
    /// <typeparam name="TProp">The type of validatable property</typeparam>
    public interface ISinglePropertyRuleBuilderInitial<TObject, TProp> :
        IRuleBuilderInitial<TObject, TProp, ISinglePropertyRuleBuilder<TObject, TProp>>
            where TObject : IValidatableObject
    { }


    /// <summary>
    /// Interface for the first step of validation collection properties per validator.
    /// Allows use only the simplest methods (for example, required)
    /// Interface for first step is necessary for it to be impossible to immediately call methods
    /// from <see cref="IRuleBuilder{TObject,TProp, TNext}"/> or <see cref="IRuleBuilderOption{TObject,TProp}"/>
    /// interfaces
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object</typeparam>
    public interface IPropertiesRuleBuilderInitial<TObject> :
        IRuleBuilderInitial<TObject, object, IPropertiesRuleBuilder<TObject>>
            where TObject : IValidatableObject
    { }


    /// <summary>
    /// Interface for the first step of validation of collection type property.
    /// Allows use some specific validation methods (for example, NotEmpty or check condtion for each element)
    /// Has additional logic for collection collection types(for examle,  <see cref="INotifyCollectionChanged"/>)
    /// Interface for first step is necessary for it to be impossible to immediately call methods
    /// from <see cref="IRuleBuilder{TObject,TProp, TNext}"/> or <see cref="IRuleBuilderOption{TObject,TProp}"/>
    /// interfaces
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object</typeparam>
    /// <typeparam name="TCollection">The type of collection</typeparam>
    /// <typeparam name="TProp">The type of element of collection</typeparam>
    public interface ICollectionRuleBuilderInitial<TObject, TCollection, TProp> :
        IRuleBuilderInitial<TObject, TCollection, ICollectionRuleBuilder<TObject, TCollection, TProp>>
            where TObject : IValidatableObject
            where TCollection : IEnumerable<TProp>
    { }
}
