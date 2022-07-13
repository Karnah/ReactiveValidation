using Avalonia.Controls;

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
    /// <typeparam name="TAppBuilder">The type of application.</typeparam>
    /// <returns>The application builder.</returns>
    public static TAppBuilder UseReactiveValidation<TAppBuilder>(
        this TAppBuilder builder,
        Action<ValidationOptionsBuilder>? optionsBuilder = null)
            where TAppBuilder : AppBuilderBase<TAppBuilder>, new()
    {
        optionsBuilder?.Invoke(ValidationOptions.Setup());
        return builder;
    }
}