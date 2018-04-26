using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using ReactiveValidation.Validators;

namespace ReactiveValidation.Extensions
{
    public static class CollectionValidatorsExtensions
    {
        public static ICollectionRuleBuilderInitial<TObject, TCollection, TProp> Count<TObject, TCollection, TProp>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TProp> ruleBuilder,
            Expression<Func<TObject, int>> minCountExpression,
            Expression<Func<TObject, int>> maxCountExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TProp>
                where TProp : IValidatableObject
        {
            return ruleBuilder.SetValidator(new CountValidator<TObject, TCollection, TProp>(minCountExpression, maxCountExpression, validationMessageType));
        }

        public static ICollectionRuleBuilderInitial<TObject, TCollection, TProp> MinCount<TObject, TCollection, TProp>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TProp> ruleBuilder,
            Expression<Func<TObject, int>> minCountExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TProp>
                where TProp : IValidatableObject
        {
            return ruleBuilder.SetValidator(new MinCountValidator<TObject, TCollection, TProp>(minCountExpression, validationMessageType));
        }

        public static ICollectionRuleBuilderInitial<TObject, TCollection, TProp> MaxCount<TObject, TCollection, TProp>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TProp> ruleBuilder,
            Expression<Func<TObject, int>> maxCountExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TProp>
                where TProp : IValidatableObject
        {
            return ruleBuilder.SetValidator(new MaxCountValidator<TObject, TCollection, TProp>(maxCountExpression, validationMessageType));
        }

        public static ICollectionRuleBuilderInitial<TObject, TCollection, TProp> Count<TObject, TCollection, TProp>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TProp> ruleBuilder,
            Expression<Func<TObject, int>> countExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TProp>
                where TProp : IValidatableObject
        {
            return ruleBuilder.SetValidator(new ExactCountValidator<TObject, TCollection, TProp>(countExpression, validationMessageType));
        }


        public static ICollectionRuleBuilderInitial<TObject, TCollection, TProp> EachElement<TObject, TCollection, TProp>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TProp> ruleBuilder,
            Func<TProp, bool> validCondition,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TProp>
        {
            return ruleBuilder.SetValidator(new EachElementValidator<TObject, TCollection, TProp>(validCondition, validationMessageType));
        }

        public static ICollectionRuleBuilderInitial<TObject, TCollection, TProp> CollectionElementsAreValid<TObject, TCollection, TProp>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TProp> ruleBuilder,
            Func<TProp, bool> validCondition,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TProp>
                where TProp : IValidatableObject
        {
            return ruleBuilder.SetValidator(new CollectionElementsAreValidValidator<TObject, TCollection, TProp>(validationMessageType));
        }

        public static ICollectionRuleBuilderInitial<TObject, TCollection, TProp> NotEmpty<TObject, TCollection, TProp>(
            this ICollectionRuleBuilderInitial<TObject, TCollection, TProp> ruleBuilder,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TObject : IValidatableObject
                where TCollection : IEnumerable<TProp>
                where TProp : IValidatableObject
        {
            return ruleBuilder.SetValidator(new NotEmptyCollectionValidator<TObject, TCollection, TProp>(validationMessageType));
        }
    }
}
