using ReactiveValidation.Helpers;

namespace ReactiveValidation.Resources.Languages
{
    /// <summary>
    /// English language.
    /// </summary>
    internal sealed class EnglishLanguage : StaticLanguage
    {
        /// <summary>
        /// Create new English language resource.
        /// </summary>
        public EnglishLanguage() : base("en")
        {
            AddTranslations(ValidatorsNames.BetweenValidator, "{PropertyName} must be between {From} and {To}");
            AddTranslations(ValidatorsNames.CollectionElementsAreValidValidator, "One of element of collection {PropertyName} doesn't correspond internal condition");
            AddTranslations(ValidatorsNames.CountValidator, "{PropertyName} must contain from {MinCount} to {MaxCount} elements. Now contains {TotalCount} elements");
            AddTranslations(ValidatorsNames.MinCountValidator, "{PropertyName} must contain not less {MinCount} elements. Now contains {TotalCount} elements");
            AddTranslations(ValidatorsNames.MaxCountValidator, "{PropertyName} must contain not more {MaxCount} elements. Now contains {TotalCount} elements");
            AddTranslations(ValidatorsNames.ExactCountValidator, "{PropertyName} must contain {MaxCount} elements. Now contains {TotalCount} elements");
            AddTranslations(ValidatorsNames.EachElementValidator, "One of element of collection {PropertyName} doesn't correspond specified condition");
            AddTranslations(ValidatorsNames.EqualValidator, "{PropertyName} should be equal {ValueToCompare}");
            AddTranslations(ValidatorsNames.GreaterThanOrEqualValidator, "{PropertyName} must be greater than or equal to {ValueToCompare}");
            AddTranslations(ValidatorsNames.GreaterThanValidator, "{PropertyName} must be greater than {ValueToCompare}");
            AddTranslations(ValidatorsNames.LengthValidator, "{PropertyName} must be between {MinLength} and {MaxLength} characters. You entered {TotalLength} characters");
            AddTranslations(ValidatorsNames.MinLengthValidator, "{PropertyName} must be not less than {MinLength} characters. You entered {TotalLength} characters");
            AddTranslations(ValidatorsNames.MaxLengthValidator, "{PropertyName} must be not more than {MaxLength} characters. You entered {TotalLength} characters");
            AddTranslations(ValidatorsNames.ExactLengthValidator, "{PropertyName} must be {MaxLength} characters in length. You entered {TotalLength} characters");
            AddTranslations(ValidatorsNames.LessThanOrEqualValidator, "{PropertyName} must be less than or equal to {ValueToCompare}");
            AddTranslations(ValidatorsNames.LessThanValidator, "{PropertyName} must be less than {ValueToCompare}");
            AddTranslations(ValidatorsNames.ModelIsValidValidator, "{PropertyName} doesn't correspond internal condition");
            AddTranslations(ValidatorsNames.NotEmptyCollectionValidator, "{PropertyName} must contain at least one element");
            AddTranslations(ValidatorsNames.NotEmptyStringValidator, "{PropertyName} must contain at least one character");
            AddTranslations(ValidatorsNames.NotEqualValidator, "{PropertyName} should be not equal to {ValueToCompare}");
            AddTranslations(ValidatorsNames.NotNullValidator, "{PropertyName} should not be empty");
            AddTranslations(ValidatorsNames.NullValidator, "{PropertyName} should be empty");
            AddTranslations(ValidatorsNames.PredicateValidator, "{PropertyName} doesn't correspond specified condition");
            AddTranslations(ValidatorsNames.RegularExpressionValidator, "{PropertyName} is not in the correct format");
        }
    }
}