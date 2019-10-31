namespace ReactiveValidation
{
    /// <summary>
    /// Static resource which contains strings for certain language.
    /// </summary>
    internal interface ILanguage
    {
        /// <summary>
        /// Name of language.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Get localized string of by its key.
        /// </summary>
        /// <param name="key">The key of message.</param>
        /// <returns>
        /// Localized string if resource contains key.
        /// <see langword="null" /> otherwise.
        /// </returns>
        string GetTranslation(string key);
    }
}
