using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

using ReactiveValidation.Validators;

namespace ReactiveValidation.Extensions
{
    public static class CommonValidatorsExtensions
    {
        /// <summary>
        /// Defines an 'inclusive between' on the current rule builder, but only for properties of types that implement IComparable.
        /// Validation will fail if the value of the property is outside of the specifed range. The range is inclusive.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <typeparam name="TProp">The type of validatable property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="from">The lowest allowed value</param>
        /// <param name="to">The highest allowed value</param>
        /// <param name="comparer">Comparer to use</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext Between<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            TProp from,
            TProp to,
            IComparer<TProp> comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable<TProp>
        {
            if ((comparer ?? Comparer<TProp>.Default).Compare(from, to) > 0)
                throw new ArgumentException("'To' should be larger than 'From' in BetweenValidator");

            return ruleBuilder.SetValidator(new BetweenValidator<TObject, TProp>(_ => from, _ => to, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines an 'inclusive between' on the current rule builder, but only for properties of types that implement IComparable.
        /// Validation will fail if the value of the property is outside of the specifed range. The range is inclusive.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <typeparam name="TProp">The type of validatable property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="fromExpression">The expression from which will be calculated lowest allowed value</param>
        /// <param name="to">The highest allowed value</param>
        /// <param name="comparer">Comparer to use</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext Between<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> fromExpression,
            TProp to,
            IComparer<TProp> comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new BetweenValidator<TObject, TProp>(fromExpression, _ => to, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines an 'inclusive between' on the current rule builder, but only for properties of types that implement IComparable.
        /// Validation will fail if the value of the property is outside of the specifed range. The range is inclusive.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <typeparam name="TProp">The type of validatable property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="from">The lowest allowed value</param>
        /// <param name="toExpression">The expression from which will be calculated highest allowed value</param>
        /// <param name="comparer">Comparer to use</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext Between<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            TProp from,
            Expression<Func<TObject, TProp>> toExpression,
            IComparer<TProp> comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new BetweenValidator<TObject, TProp>(_ => from, toExpression, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines an 'inclusive between' on the current rule builder, but only for properties of types that implement IComparable.
        /// Validation will fail if the value of the property is outside of the specifed range. The range is inclusive.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <typeparam name="TProp">The type of validatable property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="fromExpression">The expression from which will be calculated lowest allowed value</param>
        /// <param name="toExpression">The expression from which will be calculated highest allowed value</param>
        /// <param name="comparer">Comparer to use</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext Between<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> fromExpression,
            Expression<Func<TObject, TProp>> toExpression,
            IComparer<TProp> comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new BetweenValidator<TObject, TProp>(fromExpression, toExpression, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines an 'equals' validator on the current rule builder.
        /// Validation will fail if the specified value is not equal to the value of the property.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <typeparam name="TProp">The type of validatable property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="valueToCompare">The value to compare</param>
        /// <param name="comparer">Equality Comparer to use</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext Equal<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            TProp valueToCompare,
            IEqualityComparer<TProp> comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.SimpleError)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new EqualValidator<TObject, TProp>(_ => valueToCompare, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines an 'equals' validator on the current rule builder.
        /// Validation will fail if the specified value is not equal to the value of the property.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <typeparam name="TProp">The type of validatable property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="valueToCompareExpression">The expression from which will be calculated value to compare</param>
        /// <param name="comparer">Equality Comparer to use</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext Equal<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            IEqualityComparer<TProp> comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.SimpleError)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new EqualValidator<TObject,TProp>(valueToCompareExpression, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines a 'greater than or equal' validator on the current rule builder. 
        /// The validation will succeed if the property value is greater than or equal the specified value.
        /// The validation will fail if the property value is less than the specified value.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <typeparam name="TProp">The type of validatable property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="valueToCompare">The value being compared</param>
        /// <param name="comparer">Comparer to use</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext GreaterThanOrEqualTo<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            TProp valueToCompare,
            IComparer<TProp> comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new GreaterThanOrEqualValidator<TObject, TProp>(_ => valueToCompare, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines a 'greater than or equal' validator on the current rule builder. 
        /// The validation will succeed if the property value is greater than or equal the specified value.
        /// The validation will fail if the property value is less than the specified value.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <typeparam name="TProp">The type of validatable property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="valueToCompareExpression">The expression from which will be calculated value being compared</param>
        /// <param name="comparer">Comparer to use</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext GreaterThanOrEqualTo<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            IComparer<TProp> comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new GreaterThanOrEqualValidator<TObject, TProp>(valueToCompareExpression, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines a 'greater than' validator on the current rule builder. 
        /// The validation will succeed if the property value is greater than the specified value.
        /// The validation will fail if the property value is less than or equal the specified value.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <typeparam name="TProp">The type of validatable property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="valueToCompare">The value being compared</param>
        /// <param name="comparer">Comparer to use</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext GreaterThan<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            TProp valueToCompare,
            IComparer<TProp> comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new GreaterThanValidator<TObject, TProp>(_ => valueToCompare, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines a 'greater than or equal' validator on the current rule builder. 
        /// The validation will succeed if the property value is greater than the specified value.
        /// The validation will fail if the property value is less than or equal the specified value.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <typeparam name="TProp">The type of validatable property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="valueToCompareExpression">The expression from which will be calculated value being compared</param>
        /// <param name="comparer">Comparer to use</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext GreaterThan<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            IComparer<TProp> comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new GreaterThanValidator<TObject, TProp>(valueToCompareExpression, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines a length validator on the current rule builder, but only for string properties.
        /// Validation will fail if the length of the string is outside of the specifed range. The range is inclusive.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="minLength">The minimal length of string</param>
        /// <param name="maxLength">The maximum length of string</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext Length<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            int minLength,
            int maxLength,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            if (minLength < 0)
                throw new ArgumentException("MinLength should be not less 0", nameof(minLength));

            if (minLength > maxLength)
                throw new ArgumentException("MinLength should be not less MaxLength", nameof(minLength));


            return ruleBuilder.SetValidator(new LengthValidator<TObject>(_ => minLength, _ => maxLength, validationMessageType));
        }

        /// <summary>
        /// Defines a length validator on the current rule builder, but only for string properties.
        /// Validation will fail if the length of the string is outside of the specifed range. The range is inclusive.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="minLengthExpression">The expression from which will be calculated minimal length of string</param>
        /// <param name="maxLength">The maximum length of string</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext Length<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            Expression<Func<TObject, int>> minLengthExpression,
            int maxLength,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            if (maxLength < 0)
                throw new ArgumentException("MaxLength should be not less 0", nameof(maxLength));

            return ruleBuilder.SetValidator(new LengthValidator<TObject>(minLengthExpression, _ => maxLength, validationMessageType));
        }

        /// <summary>
        /// Defines a length validator on the current rule builder, but only for string properties.
        /// Validation will fail if the length of the string is outside of the specifed range. The range is inclusive.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="minLength">The minimal length of string</param>
        /// <param name="maxLengthExpression">The expression from which will be calculated maximum length of string</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext Length<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            int minLength,
            Expression<Func<TObject, int>> maxLengthExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            if (minLength < 0)
                throw new ArgumentException("MinLength should be not less 0", nameof(minLength));

            return ruleBuilder.SetValidator(new LengthValidator<TObject>(_ => minLength, maxLengthExpression, validationMessageType));
        }

        /// <summary>
        /// Defines a length validator on the current rule builder, but only for string properties.
        /// Validation will fail if the length of the string is outside of the specifed range. The range is inclusive.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="minLengthExpression">The expression from which will be calculated minimal length of string</param>
        /// <param name="maxLengthExpression">The expression from which will be calculated maximum length of string</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
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

        /// <summary>
        /// Defines a length validator on the current rule builder, but only for string properties.
        /// Validation will fail if the length of the string is less than the length specified.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="minLength">The minimal length of string</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext MinLength<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            int minLength,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            if (minLength < 0)
                throw new ArgumentException("MinLength should be not less 0", nameof(minLength));

            return ruleBuilder.SetValidator(new MinLengthValidator<TObject>(_ => minLength, validationMessageType));
        }

        /// <summary>
        /// Defines a length validator on the current rule builder, but only for string properties.
        /// Validation will fail if the length of the string is less than the length specified.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="minLengthExpression">The expression from which will be calculated minimal length of string</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext MinLength<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            Expression<Func<TObject, int>> minLengthExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new MinLengthValidator<TObject>(minLengthExpression,  validationMessageType));
        }

        /// <summary>
        /// Defines a length validator on the current rule builder, but only for string properties.
        /// Validation will fail if the length of the string is larger than the length specified.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="maxLength">The maximum length of string</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext MaxLength<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            int maxLength,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            if (maxLength < 0)
                throw new ArgumentException("MaxLength should be not less 0", nameof(maxLength));

            return ruleBuilder.SetValidator(new MaxLengthValidator<TObject>(_ => maxLength, validationMessageType));
        }

        /// <summary>
        /// Defines a length validator on the current rule builder, but only for string properties.
        /// Validation will fail if the length of the string is larger than the length specified.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="maxLengthExpression">The expression from which will be calculated maximum length of string</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext MaxLength<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            Expression<Func<TObject, int>> maxLengthExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new MaxLengthValidator<TObject>(maxLengthExpression, validationMessageType));
        }

        /// <summary>
        /// Defines a length validator on the current rule builder, but only for string properties.
        /// Validation will fail if the length of the string is not equal to the length specified.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="length">The exact length of string</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext Length<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            int length,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            if (length < 0)
                throw new ArgumentException("Length should be not less 0", nameof(length));

            return ruleBuilder.SetValidator(new ExactLengthValidator<TObject>(_ => length, validationMessageType));
        }

        /// <summary>
        /// Defines a length validator on the current rule builder, but only for string properties.
        /// Validation will fail if the length of the string is not equal to the length specified.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="lengthExpression">The expression from which will be calculated exact length of string</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext Length<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            Expression<Func<TObject, int>> lengthExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new ExactLengthValidator<TObject>(lengthExpression, validationMessageType));
        }

        /// <summary>
        /// Defines a 'less than or equal' validator on the current rule builder.
        /// The validation will succeed if the property value is less than or equal to the specified value.
        /// The validation will fail if the property value is greater than the specified value.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <typeparam name="TProp">The type of validatable property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="valueToCompare">The value being compared</param>
        /// <param name="comparer">Comparer to use</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext LessThanOrEqualTo<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            TProp valueToCompare,
            IComparer<TProp> comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new LessThanOrEqualValidator<TObject, TProp>(_ => valueToCompare, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines a 'less than or equal' validator on the current rule builder.
        /// The validation will succeed if the property value is less than or equal to the specified value.
        /// The validation will fail if the property value is greater than the specified value.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <typeparam name="TProp">The type of validatable property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="valueToCompareExpression">The expression from which will be calculated value being compared</param>
        /// <param name="comparer">Comparer to use</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext LessThanOrEqualTo<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            IComparer<TProp> comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new LessThanOrEqualValidator<TObject, TProp>(valueToCompareExpression, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines a 'less than' validator on the current rule builder.
        /// The validation will succeed if the property value is less than the specified value.
        /// The validation will fail if the property value is greater than or equal to the specified value.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <typeparam name="TProp">The type of validatable property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="valueToCompare">The value being compared</param>
        /// <param name="comparer">Comparer to use</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        public static TNext LessThan<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            TProp valueToCompare,
            IComparer<TProp> comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new LessThanValidator<TObject, TProp>(_ => valueToCompare, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines a 'less than' validator on the current rule builder.
        /// The validation will succeed if the property value is less than the specified value.
        /// The validation will fail if the property value is greater than or equal to the specified value.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <typeparam name="TProp">The type of validatable property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="valueToCompareExpression">The expression from which will be calculated value being compared</param>
        /// <param name="comparer">Comparer to use</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        public static TNext LessThan<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            IComparer<TProp> comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new LessThanValidator<TObject, TProp>(valueToCompareExpression, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines a 'model is valid' validator on the current rule builder. Only for IValidatableObject properties
        /// The validation will fail if the property value is invalid.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <typeparam name="TProp">The type of validatable property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        public static TNext ModelIsValid<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IValidatableObject
        {
            return ruleBuilder.SetValidator(new ModelIsValidValidator<TObject, TProp>(validationMessageType));
        }

        /// <summary>
        /// Defines a 'not empty' validator on the current rule builder, but only for strings
        /// Validation will fail if the string is null or empty
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext NotEmpty<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            ValidationMessageType validationMessageType = ValidationMessageType.SimpleError)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new NotEmptyStringValidator<TObject>(validationMessageType));
        }

        /// <summary>
        /// Defines an 'not equal' validator on the current rule builder.
        /// Validation will fail if the specified value is equal to the value of the property.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <typeparam name="TProp">The type of validatable property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="valueToCompare">The value to compare</param>
        /// <param name="comparer">Equality Comparer to use</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext NotEqual<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            TProp valueToCompare,
            IEqualityComparer<TProp> comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.SimpleError)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new NotEqualValidator<TObject, TProp>(_ => valueToCompare, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines an 'not equal' validator on the current rule builder.
        /// Validation will fail if the specified value is equal to the value of the property.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <typeparam name="TProp">The type of validatable property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="valueToCompareExpression">The expression from which will be calculated value to compare</param>
        /// <param name="comparer">Equality Comparer to use</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext NotEqual<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            IEqualityComparer<TProp> comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.SimpleError)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new NotEqualValidator<TObject, TProp>(valueToCompareExpression, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines an 'not null' validator on the current rule builder.
        /// Validation will fail if the specified value is not null.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <typeparam name="TProp">The type of validatable property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext NotNull<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            ValidationMessageType validationMessageType = ValidationMessageType.SimpleError)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new NotNullValidator<TObject, TProp>(validationMessageType));
        }

        /// <summary>
        /// Defines an 'null' validator on the current rule builder.
        /// Validation will fail if the specified value is null.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <typeparam name="TProp">The type of validatable property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext Null<TNext, TObject, TProp>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            ValidationMessageType validationMessageType = ValidationMessageType.SimpleError)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new NullValidator<TObject, TProp>(validationMessageType));
        }

        /// <summary>
        /// Defines a regular expression validator on the current rule builder, but only for string properties.
        /// The validation will succeed if the property value is null or empty.
        /// Validation will fail if the value returned by the lambda does not match the regular expression.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="regexPattern">The regular expression to check the value against.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
        public static TNext Matches<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            string regexPattern,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            if(string.IsNullOrEmpty(regexPattern) == true)
                throw new ArgumentException("RegexPattern should be not empty", nameof(regexPattern));

            return ruleBuilder.SetValidator(new RegularExpressionValidator<TObject>(_ => regexPattern, validationMessageType));
        }

        /// <summary>
        /// Defines a regular expression validator on the current rule builder, but only for string properties.
        /// The validation will succeed if the property value is null or empty.
        /// Validation will fail if the value returned by the lambda does not match the regular expression.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="regexPattern">The regular expression to check the value against.</param>
        /// <param name="regexOptions">Regex options</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        public static TNext Matches<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            string regexPattern,
            RegexOptions regexOptions,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            if (string.IsNullOrEmpty(regexPattern) == true)
                throw new ArgumentException("RegexPattern should be not empty", nameof(regexPattern));

            return ruleBuilder.SetValidator(new RegularExpressionValidator<TObject>(_ => regexPattern, regexOptions, validationMessageType));
        }

        /// <summary>
        /// Defines a regular expression validator on the current rule builder, but only for string properties.
        /// The validation will succeed if the property value is null or empty or calculated pattern is null or empty.
        /// Validation will fail if the value returned by the lambda does not match the regular expression.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="regexPatternExpression">The expression from which will be calculated regular expression to check the value against.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        public static TNext Matches<TNext, TObject>(
            this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
            Expression<Func<TObject, string>> regexPatternExpression,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, string, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new RegularExpressionValidator<TObject>(regexPatternExpression, validationMessageType));
        }

        /// <summary>
        /// Defines a regular expression validator on the current rule builder, but only for string properties.
        /// The validation will succeed if the property value is null or empty or calculated pattern is null or empty.
        /// Validation will fail if the value returned by the lambda does not match the regular expression.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="regexPatternExpression">The expression from which will be calculated regular expression to check the value against.</param>
        /// <param name="regexOptions">Regex options</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
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

        /// <summary>
        /// Defines a predicate validator on the current rule builder using a lambda expression to specify the predicate.
        /// Validation will fail if the specified lambda returns false.
        /// Validation will succeed if the specifed lambda returns true.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <typeparam name="TProp">The type of validatable property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="predicate">A lambda expression specifying the predicate</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
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

        /// <summary>
        /// Defines a predicate validator on the current rule builder using a lambda expression to specify the predicate.
        /// Validation will fail if the specified lambda returns false.
        /// Validation will succeed if the specifed lambda returns true.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <typeparam name="TProp">The type of validatable property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="predicate">A lambda expression specifying the predicate</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
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

        /// <summary>
        /// Defines a predicate validator on the current rule builder using a lambda expression to specify the predicate.
        /// Validation will fail if the specified lambda returns false.
        /// Validation will succeed if the specifed lambda returns true.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder</typeparam>
        /// <typeparam name="TObject">The type of validatable object</typeparam>
        /// <typeparam name="TProp">The type of validatable property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="predicate">A lambda expression specifying the predicate</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed</param>
        /// <returns></returns>
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
