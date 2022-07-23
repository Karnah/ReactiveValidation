using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ReactiveValidation.Extensions
{
    /// <summary>
    /// Allow set settings for validatable properties.
    /// </summary>
    public static class SettingsExtensions
    {
        /// <summary>
        /// Specify how rules of property should cascade when one fails.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder which property(-ies) should be tracking.</param>
        /// <param name="propertyCascadeMode">Property cascade mode.</param>
        public static IRuleBuilderInitial<TObject, TProp, TNext> WithPropertyCascadeMode<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            CascadeMode propertyCascadeMode)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
        {
            var rb = (IRuleBuilder<TObject>) ruleBuilder;

            rb.PropertyCascadeMode.GuardNotCallTwice("CascadeMode is already assigned");
            rb.PropertyCascadeMode = propertyCascadeMode;

            return ruleBuilder;
        }
        
        /// <summary>
        /// Every time when property value will raise <see cref="INotifyPropertyChanged.PropertyChanged" /> event
        /// all rules which depend from this property will be revalidated.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder which property(-ies) should be tracking.</param>
        public static IRuleBuilderInitial<TObject, TProp, TNext> TrackValueChanged<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : INotifyPropertyChanged
        {
            var rb = (IRuleBuilder<TObject>) ruleBuilder;
            rb.ObservingPropertiesSettings.TrackValueChanged = true;

            return ruleBuilder;
        }

        /// <summary>
        /// Every time when property value will raise <see cref="INotifyDataErrorInfo.ErrorsChanged" /> event
        /// all rules which depend from this property will be revalidated.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder which property(-ies) should be tracking.</param>
        public static IRuleBuilderInitial<TObject, TProp, TNext> TrackErrorsChanged<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder)
            where TNext : IRuleBuilder<TObject, TProp, TNext>
            where TObject : IValidatableObject
            where TProp : INotifyDataErrorInfo
        {
            var rb = (IRuleBuilder<TObject>) ruleBuilder;
            rb.ObservingPropertiesSettings.TrackValueErrorsChanged = true;

            return ruleBuilder;
        }

        /// <summary>
        /// Automatically create and set validator for property value.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <param name="validatorFactoryMethod">Method which allows create object validator.</param>
        public static IRuleBuilderInitial<TObject, TProp, TNext> SetValueValidator<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Func<TProp, IObjectValidator>? validatorFactoryMethod = null)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IValidatableObject?
        {
            var rb = (IRuleBuilder<TObject>) ruleBuilder;
            if (validatorFactoryMethod == null)
                rb.ObservingPropertiesSettings.PropertyValueFactoryMethod = ValidationOptions.ValidatorFactory.GetValidator;
            else
                rb.ObservingPropertiesSettings.PropertyValueFactoryMethod = o => validatorFactoryMethod((TProp) o);

            return ruleBuilder;
        }

        /// <summary>
        /// Every time when collection will raise <see cref="INotifyCollectionChanged.CollectionChanged" /> event
        /// all rules which depend from this property will be revalidated.
        /// </summary>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TCollection">The type of collection.</typeparam>
        /// <typeparam name="TItem">The type of collection item.</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        public static ICollectionRuleBuilderInitial<TObject, TCollection, TItem> TrackCollectionChanged<TObject, TCollection, TItem>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TItem> ruleBuilder)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TItem>, INotifyCollectionChanged
        {
            var rb = (IRuleBuilder<TObject>) ruleBuilder;
            rb.ObservingPropertiesSettings.TrackCollectionChanged = true;

            return ruleBuilder;
        }

        /// <summary>
        /// Every time when collection item will raise <see cref="INotifyPropertyChanged.PropertyChanged" /> event
        /// all rules which depend from this property will be revalidated.
        /// </summary>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TCollection">The type of collection.</typeparam>
        /// <typeparam name="TItem">The type of collection item.</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        public static ICollectionRuleBuilderInitial<TObject, TCollection, TItem> TrackCollectionItemChanged<TObject, TCollection, TItem>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TItem> ruleBuilder)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TItem>
                where TItem : INotifyPropertyChanged
        {
            var rb = (IRuleBuilder<TObject>) ruleBuilder;
            rb.ObservingPropertiesSettings.TrackCollectionItemChanged = true;

            if (typeof(INotifyCollectionChanged).IsAssignableFrom(typeof(TCollection)))
                rb.ObservingPropertiesSettings.TrackCollectionChanged = true;

            return ruleBuilder;
        }

        /// <summary>
        /// Every time when collection item will raise <see cref="INotifyDataErrorInfo.ErrorsChanged" /> event
        /// all rules which depend from this property will be revalidated.
        /// </summary>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TCollection">The type of collection.</typeparam>
        /// <typeparam name="TItem">The type of collection item.</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        public static ICollectionRuleBuilderInitial<TObject, TCollection, TItem> TrackCollectionItemErrorsChanged<TObject, TCollection, TItem>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TItem> ruleBuilder)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TItem>
                where TItem : INotifyDataErrorInfo
        {
            var rb = (IRuleBuilder<TObject>) ruleBuilder;
            rb.ObservingPropertiesSettings.TrackCollectionItemErrorsChanged = true;

            if (typeof(INotifyCollectionChanged).IsAssignableFrom(typeof(TCollection)))
                rb.ObservingPropertiesSettings.TrackCollectionChanged = true;

            return ruleBuilder;
        }

        /// <summary>
        /// Automatically create and set validator for collection item.
        /// </summary>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TCollection">The type of collection.</typeparam>
        /// <typeparam name="TItem">The type of collection item.</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <param name="validatorFactoryMethod">Method which allows create object validator for items.</param>
        public static ICollectionRuleBuilderInitial<TObject, TCollection, TItem> SetCollectionItemValidator<TObject, TCollection, TItem>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TItem> ruleBuilder,
            Func<TItem, IObjectValidator>? validatorFactoryMethod = null)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TItem>
                where TItem : IValidatableObject
        {
            var rb = (IRuleBuilder<TObject>) ruleBuilder;
            if (validatorFactoryMethod == null)
                rb.ObservingPropertiesSettings.CollectionItemFactoryMethod = ValidationOptions.ValidatorFactory.GetValidator;
            else
                rb.ObservingPropertiesSettings.CollectionItemFactoryMethod = o => validatorFactoryMethod((TItem)o);

            return ruleBuilder;
        }
    }
}