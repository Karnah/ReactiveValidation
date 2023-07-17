using Avalonia;

namespace ReactiveValidation.Avalonia;

/// <summary>
/// Extensions to set up app to use ReactiveValidation.
/// </summary>
public static class AppBuilderExtensions
{
    /// <summary>
    /// Set up application for using ReactiveValidation.
    /// </summary>
    /// <param name="builder">The application builder.</param>
    /// <param name="optionsBuilder">Action to set up additional properties of validation.</param>
    /// <returns>The application builder.</returns>
    public static AppBuilder UseReactiveValidation(
        this AppBuilder builder,
        Action<ValidationOptionsBuilder>? optionsBuilder = null)
    {
        optionsBuilder?.Invoke(ValidationOptions.Setup());
        return builder;
    }
}