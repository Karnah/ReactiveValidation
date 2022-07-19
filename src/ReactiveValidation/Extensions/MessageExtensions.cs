using ReactiveValidation.Resources.StringSources;

namespace ReactiveValidation.Extensions;

/// <summary>
/// Extensions to set message source to validators.
/// </summary>
public static class MessageExtensions
{
    /// <summary>
    /// Allow replace default last validator's message with static text.
    /// </summary>
    /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TProp">The type of validatable property.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="message">Validation message text.</param>
    public static TNext WithMessage<TObject, TProp, TNext>(
        this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
        string message)
            where TNext : IRuleBuilder<TObject, TProp, TNext>
            where TObject : IValidatableObject
    {
        return ruleBuilder.WithMessageSource(new StaticStringSource(message));
    }
    
    /// <summary>
    /// Allow replace default last validator's message with localized text.
    /// </summary>
    /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TProp">The type of validatable property.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="messageKey">Message key in default resource.</param>
    public static TNext WithLocalizedMessage<TObject, TProp, TNext>(
        this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
        string messageKey)
            where TNext : IRuleBuilder<TObject, TProp, TNext>
            where TObject : IValidatableObject
    {
        return ruleBuilder.WithMessageSource(new LanguageStringSource(messageKey));
    }
    
    /// <summary>
    /// Allow replace default last validator's message with localized text.
    /// </summary>
    /// <typeparam name="TNext">The type of the next rule builder.</typeparam>
    /// <typeparam name="TObject">The type of validatable object.</typeparam>
    /// <typeparam name="TProp">The type of validatable property.</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined.</param>
    /// <param name="resource">Name of resource.</param>
    /// <param name="messageKey">Message key in default resource.</param>
    public static TNext WithLocalizedMessage<TObject, TProp, TNext>(
        this IRuleBuilder<TObject, TProp, TNext> ruleBuilder,
        string resource,
        string messageKey)
            where TNext : IRuleBuilder<TObject, TProp, TNext>
            where TObject : IValidatableObject
    {
        return ruleBuilder.WithMessageSource(new LanguageStringSource(resource, messageKey));
    }
}