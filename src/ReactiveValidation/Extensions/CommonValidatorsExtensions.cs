using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ReactiveValidation.Validators;

namespace ReactiveValidation.Extensions
{
    /// <summary>
    /// Extensions for creating common validators for properties.
    /// </summary>
    public static class CommonValidatorsExtensions
    {
        /// <summary>
        /// Defines an 'inclusive between' on the current rule builder, but only for properties of types that implement IComparable.
        /// Validation will fail if the value of the property is outside of the specified range. The range is inclusive.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="from">The lowest allowed value.</param>
        /// <param name="to">The highest allowed value.</param>
        /// <param name="comparer">Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext Between<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            TProp from,
            TProp to,
            IComparer? comparer = null,
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
        /// Validation will fail if the value of the property is outside of the specified range. The range is inclusive.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="from">The lowest allowed value.</param>
        /// <param name="to">The highest allowed value.</param>
        /// <param name="comparer">Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext Between<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp?, TNext> ruleBuilder,
            TProp from,
            TProp to,
            IComparer? comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp?, TNext>
                where TObject : IValidatableObject
                where TProp : struct, IComparable<TProp>
        {
            if ((comparer ?? Comparer<TProp>.Default).Compare(from, to) > 0)
                throw new ArgumentException("'To' should be larger than 'From' in BetweenValidator");

            return ruleBuilder.SetValidator(new BetweenValidator<TObject, TProp?, TProp>(_ => from, _ => to, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines an 'inclusive between' on the current rule builder, but only for properties of types that implement IComparable.
        /// Validation will fail if the value of the property is outside of the specified range. The range is inclusive.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="fromExpression">The expression from which will be calculated lowest allowed value.</param>
        /// <param name="to">The highest allowed value.</param>
        /// <param name="comparer">Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext Between<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> fromExpression,
            TProp to,
            IComparer? comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new BetweenValidator<TObject, TProp>(fromExpression, _ => to, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines an 'inclusive between' on the current rule builder, but only for properties of types that implement IComparable.
        /// Validation will fail if the value of the property is outside of the specified range. The range is inclusive.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="fromExpression">The expression from which will be calculated lowest allowed value.</param>
        /// <param name="to">The highest allowed value.</param>
        /// <param name="comparer">Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext Between<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp?, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> fromExpression,
            TProp to,
            IComparer? comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp?, TNext>
                where TObject : IValidatableObject
                where TProp : struct, IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new BetweenValidator<TObject, TProp?, TProp>(fromExpression, _ => to, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines an 'inclusive between' on the current rule builder, but only for properties of types that implement IComparable.
        /// Validation will fail if the value of the property is outside of the specified range. The range is inclusive.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <param name="from">The lowest allowed value.</param>
        /// <param name="toExpression">The expression from which will be calculated highest allowed value.</param>
        /// <param name="comparer">Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext Between<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            TProp from,
            Expression<Func<TObject, TProp>> toExpression,
            IComparer? comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new BetweenValidator<TObject, TProp>(_ => from, toExpression, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines an 'inclusive between' on the current rule builder, but only for properties of types that implement IComparable.
        /// Validation will fail if the value of the property is outside of the specified range. The range is inclusive.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="from">The lowest allowed value.</param>
        /// <param name="toExpression">The expression from which will be calculated highest allowed value.</param>
        /// <param name="comparer">Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext Between<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp?, TNext> ruleBuilder,
            TProp from,
            Expression<Func<TObject, TProp>> toExpression,
            IComparer? comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp?, TNext>
                where TObject : IValidatableObject
                where TProp : struct, IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new BetweenValidator<TObject, TProp?, TProp>(_ => from, toExpression, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines an 'inclusive between' on the current rule builder, but only for properties of types that implement IComparable.
        /// Validation will fail if the value of the property is outside of the specified range. The range is inclusive.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="fromExpression">The expression from which will be calculated lowest allowed value.</param>
        /// <param name="toExpression">The expression from which will be calculated highest allowed value.</param>
        /// <param name="comparer">Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext Between<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> fromExpression,
            Expression<Func<TObject, TProp>> toExpression,
            IComparer? comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new BetweenValidator<TObject, TProp>(fromExpression, toExpression, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines an 'inclusive between' on the current rule builder, but only for properties of types that implement IComparable.
        /// Validation will fail if the value of the property is outside of the specified range. The range is inclusive.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="fromExpression">The expression from which will be calculated lowest allowed value.</param>
        /// <param name="toExpression">The expression from which will be calculated highest allowed value.</param>
        /// <param name="comparer">Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext Between<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp?, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> fromExpression,
            Expression<Func<TObject, TProp>> toExpression,
            IComparer? comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp?, TNext>
                where TObject : IValidatableObject
                where TProp : struct, IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new BetweenValidator<TObject, TProp?, TProp>(fromExpression, toExpression, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines an 'equals' validator on the current rule builder.
        /// Validation will fail if the specified value is not equal to the value of the property.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="valueToCompare">The value to compare.</param>
        /// <param name="comparer">Equality Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext Equal<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            TProp valueToCompare,
            IEqualityComparer? comparer = null,
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
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="valueToCompare">The value to compare.</param>
        /// <param name="comparer">Equality Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext Equal<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp?, TNext> ruleBuilder,
            TProp valueToCompare,
            IEqualityComparer? comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.SimpleError)
                where TNext : IRuleBuilder<TObject, TProp?, TNext>
                where TObject : IValidatableObject
                where TProp : struct
        {
            return ruleBuilder.SetValidator(new EqualValidator<TObject, TProp?, TProp>(_ => valueToCompare, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines an 'equals' validator on the current rule builder.
        /// Validation will fail if the specified value is not equal to the value of the property.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="valueToCompareExpression">The expression from which will be calculated value to compare.</param>
        /// <param name="comparer">Equality Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext Equal<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            IEqualityComparer? comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.SimpleError)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new EqualValidator<TObject, TProp>(valueToCompareExpression, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines a 'greater than or equal' validator on the current rule builder.
        /// The validation will succeed if the property value is greater than or equal the specified value.
        /// The validation will fail if the property value is less than the specified value.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="valueToCompare">The value being compared.</param>
        /// <param name="comparer">Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext GreaterThanOrEqualTo<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            TProp valueToCompare,
            IComparer? comparer = null,
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
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="valueToCompare">The value being compared.</param>
        /// <param name="comparer">Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext GreaterThanOrEqualTo<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp?, TNext> ruleBuilder,
            TProp valueToCompare,
            IComparer? comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp?, TNext>
                where TObject : IValidatableObject
                where TProp : struct, IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new GreaterThanOrEqualValidator<TObject, TProp?, TProp>(_ => valueToCompare, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines a 'greater than or equal' validator on the current rule builder.
        /// The validation will succeed if the property value is greater than or equal the specified value.
        /// The validation will fail if the property value is less than the specified value.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="valueToCompareExpression">The expression from which will be calculated value being compared.</param>
        /// <param name="comparer">Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext GreaterThanOrEqualTo<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            IComparer? comparer = null,
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
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="valueToCompare">The value being compared.</param>
        /// <param name="comparer">Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext GreaterThan<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            TProp valueToCompare,
            IComparer? comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new GreaterThanValidator<TObject, TProp>(_ => valueToCompare, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines a 'greater than' validator on the current rule builder.
        /// The validation will succeed if the property value is greater than the specified value.
        /// The validation will fail if the property value is less than or equal the specified value.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="valueToCompare">The value being compared.</param>
        /// <param name="comparer">Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext GreaterThan<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp?, TNext> ruleBuilder,
            TProp valueToCompare,
            IComparer? comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp?, TNext>
                where TObject : IValidatableObject
                where TProp : struct, IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new GreaterThanValidator<TObject, TProp?, TProp>(_ => valueToCompare, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines a 'greater than or equal' validator on the current rule builder.
        /// The validation will succeed if the property value is greater than the specified value.
        /// The validation will fail if the property value is less than or equal the specified value.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="valueToCompareExpression">The expression from which will be calculated value being compared.</param>
        /// <param name="comparer">Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext GreaterThan<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            IComparer? comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new GreaterThanValidator<TObject, TProp> (valueToCompareExpression, comparer, validationMessageType));
        }



        /// <summary>
        /// Defines a 'less than or equal' validator on the current rule builder.
        /// The validation will succeed if the property value is less than or equal to the specified value.
        /// The validation will fail if the property value is greater than the specified value.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="valueToCompare">The value being compared.</param>
        /// <param name="comparer">Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext LessThanOrEqualTo<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            TProp valueToCompare,
            IComparer? comparer = null,
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
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="valueToCompare">The value being compared.</param>
        /// <param name="comparer">Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext LessThanOrEqualTo<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp?, TNext> ruleBuilder,
            TProp valueToCompare,
            IComparer? comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp?, TNext>
                where TObject : IValidatableObject
                where TProp : struct, IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new LessThanOrEqualValidator<TObject, TProp?, TProp>(_ => valueToCompare, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines a 'less than or equal' validator on the current rule builder.
        /// The validation will succeed if the property value is less than or equal to the specified value.
        /// The validation will fail if the property value is greater than the specified value.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="valueToCompareExpression">The expression from which will be calculated value being compared.</param>
        /// <param name="comparer">Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext LessThanOrEqualTo<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            IComparer? comparer = null,
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
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="valueToCompare">The value being compared.</param>
        /// <param name="comparer">Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext LessThan<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            TProp valueToCompare,
            IComparer? comparer = null,
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
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="valueToCompare">The value being compared.</param>
        /// <param name="comparer">Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext LessThan<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp?, TNext> ruleBuilder,
            TProp valueToCompare,
            IComparer? comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp?, TNext>
                where TObject : IValidatableObject
                where TProp : struct, IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new LessThanValidator<TObject, TProp?, TProp>(_ => valueToCompare, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines a 'less than' validator on the current rule builder.
        /// The validation will succeed if the property value is less than the specified value.
        /// The validation will fail if the property value is greater than or equal to the specified value.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="valueToCompareExpression">The expression from which will be calculated value being compared.</param>
        /// <param name="comparer">Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext LessThan<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            IComparer? comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IComparable<TProp>
        {
            return ruleBuilder.SetValidator(new LessThanValidator<TObject, TProp>(valueToCompareExpression, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines a 'model is valid' validator on the current rule builder. Only for IValidatableObject properties.
        /// The validation will fail if the property value is invalid.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext ModelIsValid<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
                where TProp : IValidatableObject
        {
            return ruleBuilder
                .TrackErrorsChanged()
                .SetValidator(new ModelIsValidValidator<TObject, TProp>(validationMessageType));
        }

        /// <summary>
        /// Defines an 'not equal' validator on the current rule builder.
        /// Validation will fail if the specified value is equal to the value of the property.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="valueToCompare">The value to compare.</param>
        /// <param name="comparer">Equality Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext NotEqual<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            TProp valueToCompare,
            IEqualityComparer? comparer = null,
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
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="valueToCompare">The value to compare.</param>
        /// <param name="comparer">Equality Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext NotEqual<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp?, TNext> ruleBuilder,
            TProp valueToCompare,
            IEqualityComparer? comparer = null,
            ValidationMessageType validationMessageType = ValidationMessageType.SimpleError)
                where TNext : IRuleBuilder<TObject, TProp?, TNext>
                where TObject : IValidatableObject
                where TProp : struct
        {
            return ruleBuilder.SetValidator(new NotEqualValidator<TObject, TProp?, TProp>(_ => valueToCompare, comparer, validationMessageType));
        }

        /// <summary>
        /// Defines an 'not equal' validator on the current rule builder.
        /// Validation will fail if the specified value is equal to the value of the property.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="valueToCompareExpression">The expression from which will be calculated value to compare.</param>
        /// <param name="comparer">Equality Comparer to use.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext NotEqual<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            IEqualityComparer? comparer = null,
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
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext NotNull<TObject, TProp, TNext>(
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
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext Null<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            ValidationMessageType validationMessageType = ValidationMessageType.SimpleError)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new NullValidator<TObject, TProp>(validationMessageType));
        }

        /// <summary>
        /// Defines a predicate validator on the current rule builder using a lambda expression to specify the predicate.
        /// Validation will fail if the specified lambda returns false.
        /// Validation will succeed if the specified lambda returns true.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="predicate">A lambda expression specifying the predicate.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext Must<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Func<TProp, bool> predicate,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(
                new PredicateValidator<TObject, TProp>(context => predicate.Invoke(context.PropertyValue!),
                                                       validationMessageType));
        }

        /// <summary>
        /// Defines a predicate validator on the current rule builder using a lambda expression to specify the predicate.
        /// Validation will fail if the specified lambda returns false.
        /// Validation will succeed if the specified lambda returns true.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="predicate">A lambda expression specifying the predicate.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext Must<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Func<string, TProp, bool> predicate,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(
                new PredicateValidator<TObject, TProp>(context => predicate.Invoke(context.PropertyName, context.PropertyValue!), validationMessageType));
        }

        /// <summary>
        /// Defines a predicate validator on the current rule builder using a lambda expression to specify the predicate.
        /// Validation will fail if the specified lambda returns false.
        /// Validation will succeed if the specified lambda returns true.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="predicate">A lambda expression specifying the predicate.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext Must<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Func<ValidationContext<TObject, TProp>, bool> predicate,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new PredicateValidator<TObject, TProp>(predicate, validationMessageType));
        }
        
        /// <summary>
        /// Defines an async predicate validator on the current rule builder using a lambda expression to specify the predicate.
        /// Validation will fail if the specified lambda returns false.
        /// Validation will succeed if the specified lambda returns true.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="predicate">A lambda expression specifying the predicate.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext Must<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Func<TProp, Task<bool>> predicate,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(
                new AsyncPredicateValidator<TObject, TProp>((context, _) => predicate.Invoke(context.PropertyValue!),
                                                       validationMessageType));
        }

        /// <summary>
        /// Defines an async predicate validator on the current rule builder using a lambda expression to specify the predicate.
        /// Validation will fail if the specified lambda returns false.
        /// Validation will succeed if the specified lambda returns true.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="predicate">A lambda expression specifying the predicate.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext Must<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Func<TProp, CancellationToken, Task<bool>> predicate,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
            where TNext : IRuleBuilder<TObject, TProp, TNext>
            where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(
                new AsyncPredicateValidator<TObject, TProp>((context, token) => predicate.Invoke(context.PropertyValue!, token),
                    validationMessageType));
        }
        
        /// <summary>
        /// Defines an async predicate validator on the current rule builder using a lambda expression to specify the predicate.
        /// Validation will fail if the specified lambda returns false.
        /// Validation will succeed if the specified lambda returns true.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="predicate">A lambda expression specifying the predicate.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext Must<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Func<string, TProp, Task<bool>> predicate,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(
                new AsyncPredicateValidator<TObject, TProp>((context, _) => predicate.Invoke(context.PropertyName, context.PropertyValue!),
                                                       validationMessageType));
        }

        /// <summary>
        /// Defines an async predicate validator on the current rule builder using a lambda expression to specify the predicate.
        /// Validation will fail if the specified lambda returns false.
        /// Validation will succeed if the specified lambda returns true.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="predicate">A lambda expression specifying the predicate.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext Must<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Func<string, TProp, CancellationToken, Task<bool>> predicate,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
            where TNext : IRuleBuilder<TObject, TProp, TNext>
            where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(
                new AsyncPredicateValidator<TObject, TProp>((context, token) => predicate.Invoke(context.PropertyName, context.PropertyValue!, token),
                    validationMessageType));
        }
        
        /// <summary>
        /// Defines an async predicate validator on the current rule builder using a lambda expression to specify the predicate.
        /// Validation will fail if the specified lambda returns false.
        /// Validation will succeed if the specified lambda returns true.
        /// </summary>
        /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
        /// <typeparam name="TObject">The type of validatable object.</typeparam>
        /// <typeparam name="TProp">The type of validatable property.</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
        /// <param name="predicate">A lambda expression specifying the predicate.</param>
        /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
        public static TNext Must<TObject, TProp, TNext>(
            this IRuleBuilderInitial<TObject, TProp, TNext> ruleBuilder,
            Func<ValidationContext<TObject, TProp>, CancellationToken, Task<bool>> predicate,
            ValidationMessageType validationMessageType = ValidationMessageType.Error)
                where TNext : IRuleBuilder<TObject, TProp, TNext>
                where TObject : IValidatableObject
        {
            return ruleBuilder.SetValidator(new AsyncPredicateValidator<TObject, TProp>(predicate, validationMessageType));
        }
    }
}
