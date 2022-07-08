using ReactiveValidation.Helpers;

namespace ReactiveValidation.Resources.Languages
{
    /// <summary>
    /// Russian language.
    /// </summary>
    internal sealed class RussianLanguage : StaticLanguage
    {
        /// <summary>
        /// Create new Russian language resource.
        /// </summary>
        public RussianLanguage() : base("ru")
        {
            AddTranslations(ValidatorsNames.BetweenValidator, "Значение поля {PropertyName} должно быть от {From} и до {To}");
            AddTranslations(ValidatorsNames.CollectionElementsAreValidValidator, "Один из элементов коллекции {PropertyName} не удовлетворяет внутренним условиям");
            AddTranslations(ValidatorsNames.CountValidator, "Коллекция {PropertyName} должна содержать от {MinCount} до {MaxCount} элементов. Сейчас содержится {TotalCount} элементов");
            AddTranslations(ValidatorsNames.MinCountValidator, "Коллекция {PropertyName} должна содержать не менее {MinCount} элементов. Сейчас содержится {TotalCount} элементов");
            AddTranslations(ValidatorsNames.MaxCountValidator, "Коллекция {PropertyName} должна содержать не более {MaxCount} элементов. Сейчас содержится {TotalCount} элементов");
            AddTranslations(ValidatorsNames.ExactCountValidator, "Коллекция {PropertyName} должна содержать {MaxCount} элементов. Сейчас содержится {TotalCount} элементов");
            AddTranslations(ValidatorsNames.EachElementValidator, "Один из элементов коллекции {PropertyName} не удовлетворяет указанному условию");
            AddTranslations(ValidatorsNames.EqualValidator, "Значение поля {PropertyName} должно быть равно {ValueToCompare}");
            AddTranslations(ValidatorsNames.GreaterThanOrEqualValidator, "Значение поля {PropertyName} должно быть больше или равно {ValueToCompare}");
            AddTranslations(ValidatorsNames.GreaterThanValidator, "Значение поля {PropertyName} должно быть больше {ValueToCompare}");
            AddTranslations(ValidatorsNames.LengthValidator, "Текст {PropertyName} должен быть длиной от {MinLength} до {MaxLength} символов. Вы ввели {TotalLength} символов");
            AddTranslations(ValidatorsNames.MinLengthValidator, "Текст {PropertyName} должен быть длиной не менее {MinLength} символов. Вы ввели {TotalLength} символов");
            AddTranslations(ValidatorsNames.MaxLengthValidator, "Текст {PropertyName} должен быть длиной не более {MaxLength} символов. Вы ввели {TotalLength} символов");
            AddTranslations(ValidatorsNames.ExactLengthValidator, "Длина текста {PropertyName} должна равняться {MaxLength} символов. Вы ввели {TotalLength} символов");
            AddTranslations(ValidatorsNames.LessThanOrEqualValidator, "Значение поля {PropertyName} должно быть меньше или равно {ValueToCompare}");
            AddTranslations(ValidatorsNames.LessThanValidator, "Значение поля {PropertyName} должно быть меньше {ValueToCompare}");
            AddTranslations(ValidatorsNames.ModelIsValidValidator, "Значение поля {PropertyName} не удовлетворяет внутренним условиям");
            AddTranslations(ValidatorsNames.NotEmptyCollectionValidator, "Коллекция {PropertyName} должна содержать хотя бы один элемент");
            AddTranslations(ValidatorsNames.NotEmptyStringValidator, "Значение поля {PropertyName} должно содержать хотя бы один символ");
            AddTranslations(ValidatorsNames.NotEqualValidator, "Значение поля {PropertyName} не должно быть равно {ValueToCompare}");
            AddTranslations(ValidatorsNames.NotNullValidator, "Значение поля {PropertyName} обязано быть непустым");
            AddTranslations(ValidatorsNames.NullValidator, "Значение поля {PropertyName} обязано быть пустым");
            AddTranslations(ValidatorsNames.PredicateValidator, "Значение поля {PropertyName} не удовлетворяет указанному условию");
            AddTranslations(ValidatorsNames.RegularExpressionValidator, "Значение поля {PropertyName} имеет неверный формат");
        }
    }
}
