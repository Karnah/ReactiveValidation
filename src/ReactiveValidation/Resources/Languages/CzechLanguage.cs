using ReactiveValidation.Helpers;

namespace ReactiveValidation.Resources.Languages
{
    /// <summary>
    /// Czech language.
    /// </summary>
    internal sealed class CzechLanguage : StaticLanguage
    {
        /// <summary>
        /// Create new English language resource.
        /// </summary>
        public CzechLanguage() : base("cs")
        {
            AddTranslations(ValidatorsNames.BetweenValidator, "{PropertyName} musí být mezi {From} a {To}");
            AddTranslations(ValidatorsNames.CollectionElementsAreValidValidator, "Jeden z prvků kolekce {PropertyName} není validní");
            AddTranslations(ValidatorsNames.CountValidator, "{PropertyName} musí obsahovat mezi {MinCount} a {MaxCount} prvky. Počet prvků je {TotalCount}");
            AddTranslations(ValidatorsNames.MinCountValidator, "{PropertyName} nesmí obsahovat méně než {MinCount} prvků. Počet prvků je {TotalCount}");
            AddTranslations(ValidatorsNames.MaxCountValidator, "{PropertyName} nesmí obsahovat více než {MaxCount} prvků. Počet prvků je {TotalCount}");
            AddTranslations(ValidatorsNames.ExactCountValidator, "{PropertyName} musí obsahovat {MaxCount} prvků. Počet prvků je {TotalCount}");
            AddTranslations(ValidatorsNames.EachElementValidator, "Jeden z prvků v kolekci {PropertyName} není validní");
            AddTranslations(ValidatorsNames.EqualValidator, "{PropertyName} musí být {ValueToCompare}");
            AddTranslations(ValidatorsNames.GreaterThanOrEqualValidator, "{PropertyName} musí být větší nebo rovnat se {ValueToCompare}");
            AddTranslations(ValidatorsNames.GreaterThanValidator, "{PropertyName} musí být větší než {ValueToCompare}");
            AddTranslations(ValidatorsNames.LengthValidator, "{PropertyName} musí mít délku mezi {MinLength} a {MaxLength} znaky. Je zadáno {TotalLength} znaků");
            AddTranslations(ValidatorsNames.MinLengthValidator, "{PropertyName} může mít nejméně {MinLength} znaků. Je zadáno {TotalLength} znaků");
            AddTranslations(ValidatorsNames.MaxLengthValidator, "{PropertyName} může mít nejvíce {MaxLength} znaků. Je zadáno {TotalLength} znaků");
            AddTranslations(ValidatorsNames.ExactLengthValidator, "{PropertyName} musí mít {MaxLength} znaků. Je zadáno {TotalLength} znaků");
            AddTranslations(ValidatorsNames.LessThanOrEqualValidator, "{PropertyName} musí být nejméně nebo rovnat se {ValueToCompare}");
            AddTranslations(ValidatorsNames.LessThanValidator, "{PropertyName} musí být menší než {ValueToCompare}");
            AddTranslations(ValidatorsNames.ModelIsValidValidator, "{PropertyName} není validní");
            AddTranslations(ValidatorsNames.NotEmptyCollectionValidator, "{PropertyName} musí obsahovat alespoň jeden prvek");
            AddTranslations(ValidatorsNames.NotEmptyStringValidator, "{PropertyName} musí obsahovat alespoň jeden znak");
            AddTranslations(ValidatorsNames.NotEqualValidator, "{PropertyName} nesmí být {ValueToCompare}");
            AddTranslations(ValidatorsNames.NotNullValidator, "{PropertyName} nesmí být prázdný");
            AddTranslations(ValidatorsNames.NullValidator, "{PropertyName} musí být prázdný");
            AddTranslations(ValidatorsNames.PredicateValidator, "{PropertyName} není validní");
            AddTranslations(ValidatorsNames.RegularExpressionValidator, "{PropertyName} není ve správném formátu");
        }
    }
}