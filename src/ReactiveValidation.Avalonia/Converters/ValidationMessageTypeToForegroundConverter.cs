using System.Globalization;
using Avalonia.Data.Converters;

namespace ReactiveValidation.Avalonia.Converters;

/// <summary>
/// Converter from validation message type to brush.
/// </summary>
internal class ValidationMessageTypeToForegroundConverter : IMultiValueConverter
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
        
        var validationMessageType = values[0] as ValidationMessageType?;
        var errorBrush = values[1];
        var warningBrush = values[2];
        
        switch (validationMessageType)
        {
            case ValidationMessageType.Error:
            case ValidationMessageType.SimpleError:
                return errorBrush;
            
            case ValidationMessageType.Warning:
            case ValidationMessageType.SimpleWarning:
                return warningBrush;
            
            default:
                return errorBrush;
        }
    }
}