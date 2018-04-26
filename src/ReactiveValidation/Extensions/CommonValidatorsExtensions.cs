using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

using ReactiveValidation.Validators;

namespace ReactiveValidation.Extensions
{
    public static class CommonValidatorsExtensions
    {
        public static TNext Between<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            TProp from,
            TProp to,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable
        {
            if (Comparer<TProp>.Default.Compare(from, to) > 0)
                throw new ArgumentException("'To' should be larger than 'From' in BetweenValidator");

            return ruleBuilder.SetValidator(new BetweenValidator<TObject, TProp>(_ => from, _ => to, validationMessageType));
        }

        public static TNext Between<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            TProp from,
            Expression<Func<TObject, TProp>> toExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable
        {
            return ruleBuilder.SetValidator(new BetweenValidator<TObject, TProp>(_ => from, toExpression, validationMessageType));
        }

        public static TNext Between<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> fromExpression,
            TProp to,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable
        {
            return ruleBuilder.SetValidator(new BetweenValidator<TObject, TProp>(fromExpression, _ => to, validationMessageType));
        }

        public static TNext Between<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> fromExpression,
            Expression<Func<TObject, TProp>> toExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable
        {
            return ruleBuilder.SetValidator(new BetweenValidator<TObject, TProp>(fromExpression, toExpression, validationMessageType));
        }


        public static TNext Equal<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            TProp valueToCompare,
            ValidationMessageType validationMessageType = ValidationMessageType.SimpleError)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new EqualValidator<TObject, TProp>(_ => valueToCompare, validationMessageType));
        }

        public static TNext Equal<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.SimpleError)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new EqualValidator<TObject,TProp>(valueToCompareExpression, validationMessageType));
        }

        public static TNext GreaterThanOrEqual<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable
        {
            return ruleBuilder.SetValidator(new GreaterThanOrEqualValidator<TObject, TProp>(valueToCompareExpression, validationMessageType));
        }

        public static TNext GreaterThan<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable
        {
            return ruleBuilder.SetValidator(new GreaterThanValidator<TObject, TProp>(valueToCompareExpression, validationMessageType));
        }


        public static TNext Length<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            int minLength,
            int maxLength,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new LengthValidator<TObject>(_ => minLength, _ => maxLength, validationMessageType));
        }

        public static TNext Length<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            int minLength,
            Expression<Func<TObject, int>> maxLengthExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new LengthValidator<TObject>(_ => minLength, maxLengthExpression, validationMessageType));
        }

        public static TNext Length<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            Expression<Func<TObject, int>> minLengthExpression,
            int maxLength,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new LengthValidator<TObject>(minLengthExpression, _ => maxLength, validationMessageType));
        }

        public static TNext Length<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            Expression<Func<TObject, int>> minLengthExpression,
            Expression<Func<TObject, int>> maxLengthExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new LengthValidator<TObject>(minLengthExpression, maxLengthExpression, validationMessageType));
        }

        public static TNext MinLength<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            int minLength,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new MinLengthValidator<TObject>(_ => minLength, validationMessageType));
        }

        public static TNext MinLength<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            Expression<Func<TObject, int>> minLengthExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new MinLengthValidator<TObject>(minLengthExpression,  validationMessageType));
        }

        public static TNext MaxLength<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            int maxLength,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new MaxLengthValidator<TObject>(_ => maxLength, validationMessageType));
        }

        public static TNext MaxLength<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            Expression<Func<TObject, int>> maxLengthExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new MaxLengthValidator<TObject>(maxLengthExpression, validationMessageType));
        }

        public static TNext Length<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            int length,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new ExactLengthValidator<TObject>(_ => length, validationMessageType));
        }

        public static TNext Length<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            Expression<Func<TObject, int>> lengthExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new ExactLengthValidator<TObject>(lengthExpression, validationMessageType));
        }


        public static TNext LessThanOrEqual<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable
        {
            return ruleBuilder.SetValidator(new LessThanOrEqualValidator<TObject, TProp>(valueToCompareExpression, validationMessageType));
        }

        public static TNext LessThan<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable
        {
            return ruleBuilder.SetValidator(new LessThanValidator<TObject, TProp>(valueToCompareExpression, validationMessageType));
        }

        public static TNext ModelIsValid<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IValidatableObject
        {
            return ruleBuilder.SetValidator(new ModelIsValidValidator<TObject, TProp>(validationMessageType));
        }

        public static TNext NotEmpty<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            ValidationMessageType validationMessageType = ValidationMessageType.SimpleError)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new NotEmptyStringValidator<TObject>(validationMessageType));
        }

        public static TNext NotEqual<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            TProp valueToCompare,
            ValidationMessageType validationMessageType = ValidationMessageType.SimpleError)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new NotEqualValidator<TObject, TProp>(_ => valueToCompare, validationMessageType));
        }

        public static TNext NotEqual<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.SimpleError)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new NotEqualValidator<TObject, TProp>(valueToCompareExpression, validationMessageType));
        }

        public static TNext NotNull<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            ValidationMessageType validationMessageType = ValidationMessageType.SimpleError)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new NotNullValidator<TObject, TProp>(validationMessageType));
        }

        public static TNext Null<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            ValidationMessageType validationMessageType = ValidationMessageType.SimpleError)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : class
        {
            return ruleBuilder.SetValidator(new NullValidator<TObject, TProp>(validationMessageType));
        }


        public static TNext Matches<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            string regexPattern,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new RegularExpressionValidator<TObject>(_ => regexPattern, validationMessageType));
        }

        public static TNext Matches<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            string regexPattern,
            RegexOptions regexOptions,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new RegularExpressionValidator<TObject>(_ => regexPattern, regexOptions, validationMessageType));
        }

        public static TNext Matches<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            Expression<Func<TObject, string>> regexPatternExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new RegularExpressionValidator<TObject>(regexPatternExpression, validationMessageType));
        }

        public static TNext Matches<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            Expression<Func<TObject, string>> regexPatternExpression,
            RegexOptions regexOptions,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new RegularExpressionValidator<TObject>(regexPatternExpression, regexOptions, validationMessageType));
        }


        public static TNext Must<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Func<TProp, bool> predicate,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(
                new PredicateValidator<TObject, TProp>(context => predicate.Invoke(context.PropertyValue),
                                                       validationMessageType));
        }

        public static TNext Must<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Func<string, TProp, bool> predicate,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(
                new PredicateValidator<TObject, TProp>(context => predicate.Invoke(context.PropertyName, context.PropertyValue),
                                                       validationMessageType));
        }

        public static TNext Must<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Func<ValidationContext<TObject, TProp>, bool> predicate,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new PredicateValidator<TObject, TProp>(predicate, validationMessageType));
        }
    }
}
