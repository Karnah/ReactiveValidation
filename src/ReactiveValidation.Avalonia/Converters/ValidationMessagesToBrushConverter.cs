using System.Globalization;
using Avalonia.Data.Converters;

namespace ReactiveValidation.Avalonia.Converters;

/// <summary>
/// Converter from list of validation messages to brush.
/// </summary>
internal class ValidationMessagesToBrushConverter : IMultiValueConverter
{
    /// <inheritdoc />
    /// <remarks>
    /// values[0] - ValidationMessageType.
    /// values[1] - Error brush.
    /// values[2] - Warning brush.
    /// </remarks>
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count < 3)
            return null;

        var validationMessages = values[0] as IEnumerable<object>;
        if (validationMessages?.Any() != true)
            return null;

        var isAllMessagesAreWarnings = validationMessages
            .All(vm => (vm as ValidationMessage)?.ValidationMessageType is ValidationMessageType.Warning or ValidationMessageType.SimpleWarning);
        var errorBrush = values[1];
        var warningBrush = values[2];
        return isAllMessagesAreWarnings
            ? warningBrush
            : errorBrush;
    }
}