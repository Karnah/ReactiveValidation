// There is no opportunity to keep type (string/string?) of property.
// If use extensions with 'string' parameter, then it will be warnings for 'string?' properties.
// If use extension with 'string?' parameter, then return builder type will be IRuleBuilderInitial<TObject, string?, TNext> for 'string' property.
// Because of that, nullable disabled for this file.
#nullable disable

using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using ReactiveValidation.Validators;

namespace ReactiveValidation.Extensions;

/// <summary>
/// Extensions for rule builders with <see cref="string" /> property type. 
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Defines a length validator on the current rule builder, but only for string properties.
    /// Validation will fail if the length of the string is outside of the specified range. The range is inclusive.
    /// </summary>
    /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="minLength">The minimal length of string.</param>
    /// <param name="maxLength">The maximum length of string.</param>
    /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
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
    /// Validation will fail if the length of the string is outside of the specified range. The range is inclusive.
    /// </summary>
    /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="minLengthExpression">The expression from which will be calculated minimal length of string.</param>
    /// <param name="maxLength">The maximum length of string.</param>
    /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
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
    /// Validation will fail if the length of the string is outside of the specified range. The range is inclusive.
    /// </summary>
    /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="minLength">The minimal length of string.</param>
    /// <param name="maxLengthExpression">The expression from which will be calculated maximum length of string.</param>
    /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
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
    /// Validation will fail if the length of the string is outside of the specified range. The range is inclusive.
    /// </summary>
    /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="minLengthExpression">The expression from which will be calculated minimal length of string.</param>
    /// <param name="maxLengthExpression">The expression from which will be calculated maximum length of string.</param>
    /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
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
    /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="minLength">The minimal length of string.</param>
    /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
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
    /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="minLengthExpression">The expression from which will be calculated minimal length of string.</param>
    /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
    public static TNext MinLength<TNext, TObject>(
        this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
        Expression<Func<TObject, int>> minLengthExpression,
        ValidationMessageType validationMessageType = ValidationMessageType.Error)
        where TNext : IRuleBuilder<TObject, string, TNext>
        where TObject : IValidatableObject
    {
        return ruleBuilder.SetValidator(new MinLengthValidator<TObject>(minLengthExpression, validationMessageType));
    }

    /// <summary>
    /// Defines a length validator on the current rule builder, but only for string properties.
    /// Validation will fail if the length of the string is larger than the length specified.
    /// </summary>
    /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="maxLength">The maximum length of string.</param>
    /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
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
    /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="maxLengthExpression">The expression from which will be calculated maximum length of string.</param>
    /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
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
    /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="length">The exact length of string.</param>
    /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
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
    /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="lengthExpression">The expression from which will be calculated exact length of string.</param>
    /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
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
    /// Defines a 'not empty' validator on the current rule builder, but only for strings.
    /// Validation will fail if the string is null or empty.
    /// </summary>
    /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
    public static TNext NotEmpty<TNext, TObject>(
        this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
        ValidationMessageType validationMessageType = ValidationMessageType.SimpleError)
        where TNext : IRuleBuilder<TObject, string, TNext>
        where TObject : IValidatableObject
    {
        return ruleBuilder.SetValidator(new NotEmptyStringValidator<TObject>(validationMessageType));
    }
    
    /// <summary>
    /// Defines a regular expression validator on the current rule builder, but only for string properties.
    /// The validation will succeed if the property value is null or empty.
    /// Validation will fail if the value returned by the lambda does not match the regular expression.
    /// </summary>
    /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="regexPattern">The regular expression to check the value against.</param>
    /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
    public static TNext Matches<TNext, TObject>(
        this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
        string regexPattern,
        ValidationMessageType validationMessageType = ValidationMessageType.Error)
            where TNext : IRuleBuilder<TObject, string, TNext>
            where TObject : IValidatableObject
    {
        if(string.IsNullOrEmpty(regexPattern))
            throw new ArgumentException("RegexPattern should be not empty", nameof(regexPattern));

        return ruleBuilder.SetValidator(new RegularExpressionValidator<TObject>(_ => regexPattern, validationMessageType));
    }

    /// <summary>
    /// Defines a regular expression validator on the current rule builder, but only for string properties.
    /// The validation will succeed if the property value is null or empty.
    /// Validation will fail if the value returned by the lambda does not match the regular expression.
    /// </summary>
    /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="regexPattern">The regular expression to check the value against.</param>
    /// <param name="regexOptions">Regex options.</param>
    /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
    public static TNext Matches<TNext, TObject>(
        this IRuleBuilderInitial<TObject, string, TNext> ruleBuilder,
        string regexPattern,
        RegexOptions regexOptions,
        ValidationMessageType validationMessageType = ValidationMessageType.Error)
            where TNext : IRuleBuilder<TObject, string, TNext>
            where TObject : IValidatableObject
    {
        if (string.IsNullOrEmpty(regexPattern))
            throw new ArgumentException("RegexPattern should be not empty", nameof(regexPattern));

        return ruleBuilder.SetValidator(new RegularExpressionValidator<TObject>(_ => regexPattern, regexOptions, validationMessageType));
    }

    /// <summary>
    /// Defines a regular expression validator on the current rule builder, but only for string properties.
    /// The validation will succeed if the property value is null or empty or calculated pattern is null or empty.
    /// Validation will fail if the value returned by the lambda does not match the regular expression.
    /// </summary>
    /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="regexPatternExpression">The expression from which will be calculated regular expression to check the value against.</param>
    /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
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
    /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="regexPatternExpression">The expression from which will be calculated regular expression to check the value against.</param>
    /// <param name="regexOptions">Regex options.</param>
    /// <param name="validationMessageType">The message type that will be shown if validation failed.</param>
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
}