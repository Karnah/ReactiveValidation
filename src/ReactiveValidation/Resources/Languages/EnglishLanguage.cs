using ReactiveValidation.Helpers;

namespace ReactiveValidation.Languages
{
    internal sealed class EnglishLanguage : StaticLanguage
    {
        public EnglishLanguage() : base("en")
        {
            AddTranslations(ValidatorsNames.BetweenValidator, "{PropertyName} should be between {From} and {To}");
            AddTranslations(ValidatorsNames.CollectionElementsAreValidValidator, "One of element of collection {PropertyName} doesn't correspond internal condition");
            AddTranslations(ValidatorsNames.CountValidator, "{PropertyName} should contains from {MinCount} to {MaxCount} elements. Now contains {TotalCount} elements");
            AddTranslations(ValidatorsNames.MinCountValidator, "{PropertyName} should contains not less {MinCount} elements. Now contains {TotalCount} elements");
            AddTranslations(ValidatorsNames.MaxCountValidator, "{PropertyName} should contains not more {MaxCount} elements. Now contains {TotalCount} elements");
            AddTranslations(ValidatorsNames.ExactCountValidator, "{PropertyName} should contains {MaxCount} elements. Now contains {TotalCount} elements");
            AddTranslations(ValidatorsNames.EachElementValidator, "One of element of collection {PropertyName} doesn't correspond specified condition");
            AddTranslations(ValidatorsNames.EqualValidator, "{PropertyName} should be equal {ValueToCompare}");
            AddTranslations(ValidatorsNames.GreaterThanOrEqualValidator, "{PropertyName} should be greater than or equal to {ValueToCompare}");
            AddTranslations(ValidatorsNames.GreaterThanValidator, "{PropertyName} should be greater than {ValueToCompare}");
            AddTranslations(ValidatorsNames.LengthValidator, "{PropertyName} should be between {MinLength} and {MaxLength} characters. You entered {TotalLength} characters");
            AddTranslations(ValidatorsNames.MinLengthValidator, "{PropertyName} should be more than {MinLength} characters. You entered {TotalLength} characters");
            AddTranslations(ValidatorsNames.MaxLengthValidator, "{PropertyName} should be less than {MaxLength} characters. You entered {TotalLength} characters");
            AddTranslations(ValidatorsNames.ExactLengthValidator, "{PropertyName} should be {MaxLength} characters in length. You entered {TotalLength} characters");
            AddTranslations(ValidatorsNames.LessThanOrEqualValidator, "{PropertyName} should be less than or equal to {ValueToCompare}");
            AddTranslations(ValidatorsNames.LessThanValidator, "{PropertyName} should be less than {ValueToCompare}");
            AddTranslations(ValidatorsNames.ModelIsValidValidator, "{PropertyName} doesn't correspond internal condition");
            AddTranslations(ValidatorsNames.NotEmptyCollectionValidator, "{PropertyName} should contains at least one element");
            AddTranslations(ValidatorsNames.NotEmptyStringValidator, "{PropertyName} should contains at least one character");
            AddTranslations(ValidatorsNames.NotEqualValidator, "{PropertyName} should be not equal to {ValueToCompare}");
            AddTranslations(ValidatorsNames.NotNullValidator, "{PropertyName} should not be empty");
            AddTranslations(ValidatorsNames.NullValidator, "{PropertyName} should be empty");
            AddTranslations(ValidatorsNames.PredicateValidator, "{PropertyName} doesn't correspond specified condition");
            AddTranslations(ValidatorsNames.RegularExpressionValidator, "{PropertyName} is not in the correct format");
        }
    }
}
