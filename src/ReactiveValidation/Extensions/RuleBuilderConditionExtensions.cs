using System;
using System.Linq.Expressions;
using ReactiveValidation.Validators;

namespace ReactiveValidation.Extensions
{
    /// <summary>
    /// Extensions method of <see cref="IRuleBuilder{TObject,TProp,TBuilder}.When" /> and <see cref="IRuleBuilderOption{TObject,TProp}.AllWhen" />.
    /// </summary>
    public static class RuleBuilderConditionExtensions
    {
        #region When

        /// <summary>
        /// Last validator will check only if the condition is <see langword="true" />.
        /// </summary>
        public static IRuleBuilder<TObject, TProp, TNext> When<TNext, TObject, TProp>(
            this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
            Func<bool> condition)
                where TObject : IValidatableObject
                where TNext : IRuleBuilder<TObject, TProp, TNext>
        {
            return ruleBuilder.When(WrapCondition<TObject>(condition));
        }

        /// <summary>
        /// Last validator will check only if the condition is <see langword="true" />.
        /// </summary>
        public static IRuleBuilder<TObject, TProp, TNext> When<TNext, TObject, TProp>(
            this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, bool>> conditionProperty)
                where TObject : IValidatableObject
                where TNext : IRuleBuilder<TObject, TProp, TNext>
        {
            return ruleBuilder.When(WrapCondition(conditionProperty));
        }

        /// <summary>
        /// Last validator will check only if the condition is <see langword="true" />.
        /// </summary>
        public static IRuleBuilder<TObject, TProp, TNext> When<TNext, TObject, TProp, TParam>(
            this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TParam>> property,
            Func<TParam, bool> condition)
                where TObject : IValidatableObject
                where TNext : IRuleBuilder<TObject, TProp, TNext>
        {
            return ruleBuilder.When(WrapCondition(condition, property));
        }

        /// <summary>
        /// Last validator will check only if the condition is <see langword="true" />.
        /// </summary>
        public static IRuleBuilder<TObject, TProp, TNext> When<TNext, TObject, TProp, TParam1, TParam2>(
            this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TParam1>> property1,
            Expression<Func<TObject, TParam2>> property2,
            Func<TParam1, TParam2, bool> condition)
                where TObject : IValidatableObject
                where TNext : IRuleBuilder<TObject, TProp, TNext>
        {
            return ruleBuilder.When(WrapCondition(condition, property1, property2));
        }

        /// <summary>
        /// Last validator will check only if the condition is <see langword="true" />.
        /// </summary>
        public static IRuleBuilder<TObject, TProp, TNext> When<TNext, TObject, TProp, TParam1, TParam2, TParam3>(
            this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TParam1>> property1,
            Expression<Func<TObject, TParam2>> property2,
            Expression<Func<TObject, TParam3>> property3,
            Func<TParam1, TParam2, TParam3, bool> condition)
                where TObject : IValidatableObject
                where TNext : IRuleBuilder<TObject, TProp, TNext>
        {
            return ruleBuilder.When(WrapCondition(condition, property1, property2, property3));
        }

        #endregion

        #region AllWhen

        /// <summary>
        /// The validation of the rule will occur only if the condition is <see langword="true" />.
        /// </summary>
        public static IRuleBuilderOption<TObject, TProp> AllWhen<TNext, TObject, TProp>(
            this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
            Func<bool> condition)
                where TObject : IValidatableObject
                where TNext : IRuleBuilder<TObject, TProp, TNext>
        {
            return ruleBuilder.AllWhen(WrapCondition<TObject>(condition));
        }

        /// <summary>
        /// The validation of the rule will occur only if the property value is <see langword="true" />.
        /// </summary>
        public static IRuleBuilderOption<TObject, TProp> AllWhen<TNext, TObject, TProp>(
            this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, bool>> conditionProperty)
                where TObject : IValidatableObject
                where TNext : IRuleBuilder<TObject, TProp, TNext>
        {
            return ruleBuilder.AllWhen(WrapCondition(conditionProperty));
        }

        /// <summary>
        /// The validation of the rule will occur only if the condition is <see langword="true" />.
        /// </summary>
        public static IRuleBuilderOption<TObject, TProp> AllWhen<TNext, TObject, TProp, TParam>(
            this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TParam>> property, Func<TParam, bool> condition)
                where TObject : IValidatableObject
                where TNext : IRuleBuilder<TObject, TProp, TNext>
        {
            return ruleBuilder.AllWhen(WrapCondition(condition, property));
        }

        /// <summary>
        /// The validation of the rule will occur only if the condition is <see langword="true" />.
        /// </summary>
        public static IRuleBuilderOption<TObject, TProp> AllWhen<TNext, TObject, TProp, TParam1, TParam2>(
            this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TParam1>> property1,
            Expression<Func<TObject, TParam2>> property2,
            Func<TParam1, TParam2, bool> condition)
                where TObject : IValidatableObject
                where TNext : IRuleBuilder<TObject, TProp, TNext>
        {
            return ruleBuilder.AllWhen(WrapCondition(condition, property1, property2));
        }

        /// <summary>
        /// The validation of the rule will occur only if the condition is <see langword="true" />.
        /// </summary>
        public static IRuleBuilderOption<TObject, TProp> AllWhen<TNext, TObject, TProp, TParam1, TParam2, TParam3>(
            this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TParam1>> property1,
            Expression<Func<TObject, TParam2>> property2,
            Expression<Func<TObject, TParam3>> property3,
            Func<TParam1, TParam2, TParam3, bool> condition)
                where TObject : IValidatableObject
                where TNext : IRuleBuilder<TObject, TProp, TNext>
        {
            return ruleBuilder.AllWhen(WrapCondition(condition, property1, property2, property3));
        }

        #endregion
        
        private static ValidationCondition<TObject> WrapCondition<TObject>(Func<bool> condition)
            where TObject : IValidatableObject
        {
            return new ValidationCondition<TObject>(_ => condition());
        }

        private static ValidationCondition<TObject> WrapCondition<TObject>(Expression<Func<TObject, bool>> conditionProperty)
            where TObject : IValidatableObject
        {
            return new ValidationCondition<TObject>(conditionProperty.Compile(), conditionProperty);
        }
        
        private static ValidationCondition<TObject> WrapCondition<TObject, TParam>(
            Func<TParam, bool> condition,
            Expression<Func<TObject, TParam>> property)
            where TObject : IValidatableObject
        {
            var paramFunc = property.Compile();
            
            return new ValidationCondition<TObject>(instance =>
            {
                var param = paramFunc.Invoke(instance);
                return condition.Invoke(param);
            }, property);
        }

        private static ValidationCondition<TObject> WrapCondition<TObject, TParam1, TParam2>(
            Func<TParam1, TParam2, bool> condition,
            Expression<Func<TObject, TParam1>> property1,
            Expression<Func<TObject, TParam2>> property2)
            where TObject : IValidatableObject
        {
            var param1Func = property1.Compile();
            var param2Func = property2.Compile();
            
            return new ValidationCondition<TObject>(instance =>
            {
                var param1 = param1Func.Invoke(instance);
                var param2 = param2Func.Invoke(instance);

                return condition.Invoke(param1, param2);
            }, property1, property2);
        }

        private static ValidationCondition<TObject> WrapCondition<TObject, TParam1, TParam2, TParam3>(
            Func<TParam1, TParam2, TParam3, bool> condition,
            Expression<Func<TObject, TParam1>> property1,
            Expression<Func<TObject, TParam2>> property2,
            Expression<Func<TObject, TParam3>> property3)
            where TObject : IValidatableObject
        {
            var param1Func = property1.Compile();
            var param2Func = property2.Compile();
            var param3Func = property3.Compile();
            
            return new ValidationCondition<TObject>(instance =>
            {
                var param1 = param1Func.Invoke(instance);
                var param2 = param2Func.Invoke(instance);
                var param3 = param3Func.Invoke(instance);

                return condition.Invoke(param1, param2, param3);
            }, property1, property2, property3);
        }
    }
}