namespace ReactiveValidation.Resources.StringSources
{
    /// <summary>
    /// Special class which allows get string for a specific case.
    /// </summary>
    public interface IStringSource
    {
        /// <summary>
        /// Get string.
        /// </summary>
        /// <returns>String for current case.</returns>
        string GetString();
    }
}
