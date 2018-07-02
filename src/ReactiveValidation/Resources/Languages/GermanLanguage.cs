using ReactiveValidation.Helpers;

namespace ReactiveValidation.Languages
{
    internal sealed class GermanLanguage : StaticLanguage
    {
        public GermanLanguage() : base("de")
        {
            AddTranslations(ValidatorsNames.BetweenValidator, "{PropertyName} muss zwischen {From} und {To} liegen");
            AddTranslations(ValidatorsNames.CollectionElementsAreValidValidator, "Ein Element der Auflistung {PropertyName} erfüllt die interne Bedingung nicht");
            AddTranslations(ValidatorsNames.CountValidator, "{PropertyName} muss zwischen {MinCount} und {MaxCount} Elemente enthalten. Es sind {TotalCount} Elemente enthalten");
            AddTranslations(ValidatorsNames.MinCountValidator, "{PropertyName} muss mindestens {MinCount} Elemente enthalten. Es sind {TotalCount} Elemente enthalten");
            AddTranslations(ValidatorsNames.MaxCountValidator, "{PropertyName} darf nicht mehr als {MaxCount} Elemente enthalten. Es sind {TotalCount} Elemente enthalten");
            AddTranslations(ValidatorsNames.ExactCountValidator, "{PropertyName} muss genau {MaxCount} Elemente enthalten. Es sind {TotalCount} Elemente enthalten");
            AddTranslations(ValidatorsNames.EachElementValidator, "Ein Element der Auflistung {PropertyName} erfüllt die Bedingung nicht");
            AddTranslations(ValidatorsNames.EqualValidator, "{PropertyName} muss gleich {ValueToCompare} sein");
            AddTranslations(ValidatorsNames.GreaterThanOrEqualValidator, "{PropertyName} muss größer oder gleich {ValueToCompare} sein");
            AddTranslations(ValidatorsNames.GreaterThanValidator, "{PropertyName} muss größer als {ValueToCompare} sein");
            AddTranslations(ValidatorsNames.LengthValidator, "{PropertyName} muss zwischen {MinLength} und {MaxLength} Zeichen enthalten. Sie haben {TotalLength} Zeichen eingegeben");
            AddTranslations(ValidatorsNames.MinLengthValidator, "{PropertyName} muss mehr als {MinLength} Zeichen enthalten. Sie haben {TotalLength} Zeichen eingegeben");
            AddTranslations(ValidatorsNames.MaxLengthValidator, "{PropertyName} muss weniger als {MaxLength} Zeichen enthalten. Sie haben {TotalLength} Zeichen eingegeben");
            AddTranslations(ValidatorsNames.ExactLengthValidator, "{PropertyName} muss genau {MaxLength} Zeichen enthalten. Sie haben {TotalLength} Zeichen eingegeben");
            AddTranslations(ValidatorsNames.LessThanOrEqualValidator, "{PropertyName} muss kleiner oder gleich {ValueToCompare} sein");
            AddTranslations(ValidatorsNames.LessThanValidator, "{PropertyName} muss kleiner als {ValueToCompare} sein");
            AddTranslations(ValidatorsNames.ModelIsValidValidator, "{PropertyName} erfüllt die interne Bedingung nicht");
            AddTranslations(ValidatorsNames.NotEmptyCollectionValidator, "{PropertyName} muss mindestens ein Element enthalten");
            AddTranslations(ValidatorsNames.NotEmptyStringValidator, "{PropertyName} muss mindestens ein Zeichen enthalten");
            AddTranslations(ValidatorsNames.NotEqualValidator, "{PropertyName} darf nicht gleich {ValueToCompare} sein");
            AddTranslations(ValidatorsNames.NotNullValidator, "{PropertyName} darf nicht leer sein");
            AddTranslations(ValidatorsNames.NullValidator, "{PropertyName} muss leer sein");
            AddTranslations(ValidatorsNames.PredicateValidator, "{PropertyName} erfüllt die angegebene Bedingung nicht");
            AddTranslations(ValidatorsNames.RegularExpressionValidator, "{PropertyName} hat nicht das richtige Format");
        }
    }
}
