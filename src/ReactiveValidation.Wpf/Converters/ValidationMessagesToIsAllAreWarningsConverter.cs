using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace ReactiveValidation.WPF.Converters;

/// <summary>
/// Check if all validation messages are warnings or simple warnings.
/// </summary>
internal class ValidationMessagesToIsAllAreWarningsConverter : IMultiValueConverter
{
    /// <inheritdoc />
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var errors = (values[0] as IReadOnlyCollection<ValidationError>)
            ?.Select(ve => ve.ErrorContent)
            .OfType<ValidationMessage>()
            .ToList();
        if (errors?.Any() != true)
            return false;

        return errors
            .All(e => e.ValidationMessageType is ValidationMessageType.Warning or ValidationMessageType.SimpleWarning);
    }

    /// <inheritdoc />
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}