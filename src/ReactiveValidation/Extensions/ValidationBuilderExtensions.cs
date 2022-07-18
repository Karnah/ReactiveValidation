using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace ReactiveValidation.Extensions
{
    /// <summary>
    /// Extensions for <see cref="IValidationBuilder{TObject}" />.
    /// </summary>
    public static class ValidationBuilderExtensions
    {
        /// <summary>
        /// Create validator for property with collection type, i.e. <see cref="IEnumerable{T}" /> interface.
        /// </summary>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of element of collection.</typeparam>
        /// <param name="validationBuilder">The validation builder.</param>
        /// <param name="collection">Property with collection type.</param>
        /// <returns>Validator for property with <see cref="IEnumerable{T}" /> type.</returns>
        public static ICollectionRuleBuilderInitial<TObject, IEnumerable<TProp>, TProp> RuleForCollection<TObject, TProp>(
            this IValidationBuilder<TObject> validationBuilder,
            Expression<Func<TObject, IEnumerable<TProp>>> collection)
            where TObject : IValidatableObject
        {
            return validationBuilder.RuleForCollection<IEnumerable<TProp>, TProp>(collection);
        }

        /// <summary>
        /// Create validator for property with <see cref="ObservableCollection{T}" /> type.
        /// </summary>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of element of collection.</typeparam>
        /// <param name="collection">Property with collection type.</param>
        /// <param name="validationBuilder">The validation builder.</param>
        /// <returns>Validator for property with <see cref="IEnumerable{T}" /> type.</returns>
        public static ICollectionRuleBuilderInitial<TObject, ObservableCollection<TProp>, TProp> RuleForCollection<TObject, TProp>(
            this IValidationBuilder<TObject> validationBuilder,
            Expression<Func<TObject, ObservableCollection<TProp>>> collection)
            where TObject : IValidatableObject
        {
            return validationBuilder.RuleForCollection<ObservableCollection<TProp>, TProp>(collection);
        }
    }
}
