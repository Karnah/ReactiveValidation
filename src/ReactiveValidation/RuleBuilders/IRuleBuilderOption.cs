using System;
using System.Linq.Expressions;

namespace ReactiveValidation
{
    /// <summary>
    /// Interface for options of validation builder.
    /// </summary>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TProp">The type of validatable property.</typeparam>
    public interface IRuleBuilderOption<TObject, TProp>
    {
        /// <summary>
        /// The validation of the rule will occur only if the condition is <see langword="true" />.
        /// </summary>
        IRuleBuilderOption<TObject, TProp> AllWhen(Func<bool> condition);

        /// <summary>
        /// The validation of the rule will occur only if the property value is <see langword="true" />.
        /// </summary>
        IRuleBuilderOption<TObject, TProp> AllWhen(Expression<Func<TObject, bool>> conditionProperty);

        /// <summary>
        /// The validation of the rule will occur only if the condition is <see langword="true" />.
        /// </summary>
        IRuleBuilderOption<TObject, TProp> AllWhen<TParam>(Expression<Func<TObject, TParam>> property, Func<TParam, bool> condition);

        /// <summary>
        /// The validation of the rule will occur only if the condition is <see langword="true" />.
        /// </summary>
        IRuleBuilderOption<TObject, TProp> AllWhen<TParam1, TParam2>(
            Expression<Func<TObject, TParam1>> property1,
            Expression<Func<TObject, TParam2>> property2,
            Func<TParam1, TParam2, bool> condition);

        /// <summary>
        /// The validation of the rule will occur only if the condition is <see langword="true" />.
        /// </summary>
        IRuleBuilderOption<TObject, TProp> AllWhen<TParam1, TParam2, TParam3>(
            Expression<Func<TObject, TParam1>> property1,
            Expression<Func<TObject, TParam2>> property2,
            Expression<Func<TObject, TParam3>> property3,
            Func<TParam1, TParam2, TParam3, bool> condition);
    }
}
