using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

using ReactiveValidation.Validators;

namespace ReactiveValidation.Extensions
{
    /// <summary>
    /// Extensions for creating validators for collections.
    /// </summary>
    public static class CollectionValidatorsExtensions
    {
        /// <summary>
        /// Count of items in collection should be inclusive between <paramref name="minCount" /> and <paramref name="maxCount" /> values.
        /// </summary>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TCollection">The type of collection.</typeparam>
        /// <typeparam name="TItem">The type of element of collection.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="minCount">Minimum count of items in collection (inclusive).</param>
        /// <param name="maxCount">Maximum count of items in collection (inclusive).</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static ICollectionRuleBuilderInitial<TObject, TCollection, TItem> Count<TObject, TCollection, TItem>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TItem> ruleBuilder,
            int minCount,
            int maxCount,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TItem>
        {
            return ruleBuilder.SetValidator(new CountValidator<TObject, TCollection, TItem>(_ => minCount, _ => maxCount, validationMessageType));
        }

        /// <summary>
        /// Count of items in collection should be inclusive between <paramref name="minCount" /> and <paramref name="maxCountExpression" /> values.
        /// </summary>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TCollection">The type of collection.</typeparam>
        /// <typeparam name="TItem">The type of element of collection.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="minCount">Minimum count of items in collection (inclusive).</param>
        /// <param name="maxCountExpression">Maximum of count of items in collection (inclusive).</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static ICollectionRuleBuilderInitial<TObject, TCollection, TItem> Count<TObject, TCollection, TItem>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TItem> ruleBuilder,
            int minCount,
            Expression<Func<TObject, int>> maxCountExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TItem>
        {
            return ruleBuilder.SetValidator(new CountValidator<TObject, TCollection, TItem>(_ => minCount, maxCountExpression, validationMessageType));
        }

        /// <summary>
        /// Count of items in collection should be inclusive between <paramref name="minCountExpression" /> and <paramref name="maxCount" /> values.
        /// </summary>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TCollection">The type of collection.</typeparam>
        /// <typeparam name="TItem">The type of element of collection.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="minCountExpression">Minimum count of items in collection (inclusive).</param>
        /// <param name="maxCount">Maximum count of items in collection (inclusive).</param>
        public static ICollectionRuleBuilderInitial<TObject, TCollection, TItem> Count<TObject, TCollection, TItem>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TItem> ruleBuilder,
            Expression<Func<TObject, int>> minCountExpression,
            int maxCount,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TItem>
        {
            return ruleBuilder.SetValidator(new CountValidator<TObject, TCollection, TItem>(minCountExpression, _ => maxCount, validationMessageType));
        }

        /// <summary>
        /// Count of items in collection should be inclusive between <paramref name="minCountExpression" /> and <paramref name="maxCountExpression" /> values.
        /// </summary>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TCollection">The type of collection.</typeparam>
        /// <typeparam name="TItem">The type of element of collection.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="minCountExpression">Minimum count of items in collection (inclusive).</param>
        /// <param name="maxCountExpression">Maximum count of items in collection (inclusive).</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static ICollectionRuleBuilderInitial<TObject, TCollection, TItem> Count<TObject, TCollection, TItem>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TItem> ruleBuilder,
            Expression<Func<TObject, int>> minCountExpression,
            Expression<Func<TObject, int>> maxCountExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TItem>
        {
            return ruleBuilder.SetValidator(new CountValidator<TObject, TCollection, TItem>(minCountExpression, maxCountExpression, validationMessageType));
        }

        /// <summary>
        /// Count of items in collection should be not less than <paramref name="minCount" />.
        /// </summary>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TCollection">The type of collection.</typeparam>
        /// <typeparam name="TItem">The type of element of collection.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="minCount">Minimum count of items in collection (inclusive).</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static ICollectionRuleBuilderInitial<TObject, TCollection, TItem> MinCount<TObject, TCollection, TItem>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TItem> ruleBuilder,
            int minCount,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TItem>
        {
            return ruleBuilder.SetValidator(new MinCountValidator<TObject, TCollection, TItem>(_ => minCount, validationMessageType));
        }

        /// <summary>
        /// Count of items in collection should be not less than <paramref name="minCountExpression" />.
        /// </summary>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TCollection">The type of collection.</typeparam>
        /// <typeparam name="TItem">The type of element of collection.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="minCountExpression">Minimum count of items in collection (inclusive).</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static ICollectionRuleBuilderInitial<TObject, TCollection, TItem> MinCount<TObject, TCollection, TItem>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TItem> ruleBuilder,
            Expression<Func<TObject, int>> minCountExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TItem>
        {
            return ruleBuilder.SetValidator(new MinCountValidator<TObject, TCollection, TItem>(minCountExpression, validationMessageType));
        }

        /// <summary>
        /// Count of items in collection should be not greater than <paramref name="maxCount" />.
        /// </summary>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TCollection">The type of collection.</typeparam>
        /// <typeparam name="TItem">The type of element of collection.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="maxCount">Maximum count of items in collection (inclusive).</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static ICollectionRuleBuilderInitial<TObject, TCollection, TItem> MaxCount<TObject, TCollection, TItem>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TItem> ruleBuilder,
            int maxCount,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TItem>
        {
            return ruleBuilder.SetValidator(new MaxCountValidator<TObject, TCollection, TItem>(_ => maxCount, validationMessageType));
        }

        /// <summary>
        /// Count of items in collection should be not greater than <paramref name="maxCountExpression" />.
        /// </summary>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TCollection">The type of collection.</typeparam>
        /// <typeparam name="TItem">The type of element of collection.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="maxCountExpression">Maximum count of items in collection (inclusive).</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static ICollectionRuleBuilderInitial<TObject, TCollection, TItem> MaxCount<TObject, TCollection, TItem>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TItem> ruleBuilder,
            Expression<Func<TObject, int>> maxCountExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TItem>
        {
            return ruleBuilder.SetValidator(new MaxCountValidator<TObject, TCollection, TItem>(maxCountExpression, validationMessageType));
        }

        /// <summary>
        /// Count of items in collection should be exactly <paramref name="count" />.
        /// </summary>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TCollection">The type of collection.</typeparam>
        /// <typeparam name="TItem">The type of element of collection.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="count">Count of items in collection.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static ICollectionRuleBuilderInitial<TObject, TCollection, TItem> Count<TObject, TCollection, TItem>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TItem> ruleBuilder,
            int count,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TItem>
        {
            return ruleBuilder.SetValidator(new ExactCountValidator<TObject, TCollection, TItem>(_ => count, validationMessageType));
        }

        /// <summary>
        /// Count of items in collection should be exactly <paramref name="countExpression" />.
        /// </summary>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TCollection">The type of collection.</typeparam>
        /// <typeparam name="TItem">The type of element of collection.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="countExpression">Count of items in collection.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static ICollectionRuleBuilderInitial<TObject, TCollection, TItem> Count<TObject, TCollection, TItem>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TItem> ruleBuilder,
            Expression<Func<TObject, int>> countExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TItem>
        {
            return ruleBuilder.SetValidator(new ExactCountValidator<TObject, TCollection, TItem>(countExpression, validationMessageType));
        }

        /// <summary>
        /// Condition should be <see langwrod="true" /> for each element.
        /// </summary>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TCollection">The type of collection.</typeparam>
        /// <typeparam name="TItem">The type of element of collection.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="validCondition">Condition of validity.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static ICollectionRuleBuilderInitial<TObject, TCollection, TItem> EachElement<TObject, TCollection, TItem>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TItem> ruleBuilder,
            Func<TItem, bool> validCondition,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TItem>
        {
            return ruleBuilder.SetValidator(new EachElementValidator<TObject, TCollection, TItem>(validCondition, validationMessageType));
        }

        /// <summary>
        /// All items in collections should be valid.
        /// </summary>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TCollection">The type of collection.</typeparam>
        /// <typeparam name="TItem">The type of element of collection.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static ICollectionRuleBuilderInitial<TObject, TCollection, TItem> CollectionElementsAreValid<TObject, TCollection, TItem>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TItem> ruleBuilder,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TItem>
                where TItem : INotifyDataErrorInfo
        {
            return ruleBuilder
                .TrackCollectionItemErrorsChanged()
                .SetValidator(new CollectionElementsAreValidValidator<TObject, TCollection, TItem>(validationMessageType));
        }

        /// <summary>
        /// Collection should have at least one item.
        /// </summary>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TCollection">The type of collection.</typeparam>
        /// <typeparam name="TItem">The type of element of collection.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static ICollectionRuleBuilderInitial<TObject, TCollection, TItem> NotEmpty<TObject, TCollection, TItem>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TItem> ruleBuilder,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TItem>
        {
            return ruleBuilder.SetValidator(new NotEmptyCollectionValidator<TObject, TCollection, TItem>(validationMessageType));
        }
    }
}
