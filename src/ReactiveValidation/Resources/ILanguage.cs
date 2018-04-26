namespace ReactiveValidation
{
    internal interface ILanguage
    {
        string Name { get; }


        string GetTranslation(string key);
    }
}
