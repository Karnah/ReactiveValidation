using System;
using System.Globalization;
using System.Windows.Data;

namespace ReactiveValidation.WPF.Converters;

/// <summary>
/// Check if value not equal to parameter.
/// </summary>
internal class NotEqualConverter : IValueConverter
{
    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return false;

        return !value.Equals(parameter);
    }

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}