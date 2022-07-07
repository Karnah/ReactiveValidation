using System.Globalization;

namespace ReactiveValidation
{
    /// <summary>
    /// Provider of messages, localization strings and other.
    /// </summary>
    public interface IStringProvider
    {
        /// <summary>
        /// Get string by its key and culture.
        /// </summary>
        /// <param name="key">Key of string.</param>
        /// <param name="culture">Culture for string.</param>
        /// <returns>Found string.</returns>
        string? GetString(string key, CultureInfo culture);

        /// <summary>
        /// Get string by its resource, key and culture.
        /// </summary>
        /// <param name="resource">Name of resource.</param>
        /// <param name="key">Key of string.</param>
        /// <param name="culture">Culture for string.</param>
        /// <returns>Found string.</returns>
        string? GetString(string resource, string key, CultureInfo culture);
    }
}
