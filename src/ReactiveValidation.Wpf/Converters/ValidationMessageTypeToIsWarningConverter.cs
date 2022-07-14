using System;
using System.Globalization;
using System.Windows.Data;

namespace ReactiveValidation.WPF.Converters;

/// <summary>
/// Check if validation message type is waning or simple warning.
/// </summary>
internal class ValidationMessageTypeToIsWarningConverter : IValueConverter
{
    /// <inheritdoc />
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var validationMessageType = value as ValidationMessageType?;
        return validationMessageType is ValidationMessageType.Warning or ValidationMessageType.SimpleWarning;
    }

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}