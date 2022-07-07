using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace ReactiveValidation.WPF.Converters
{
    internal class ErrorsToValidationMessagesConverter : IMultiValueConverter
    {
        /// <inheritdoc />
        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var errors = (values[0] as IReadOnlyCollection<ValidationError>)
                ?.Select(ve => ve.ErrorContent)
                .OfType<ValidationMessage>()
                .OrderBy(vm => vm.ValidationMessageType)
                .ThenBy(vm => vm.Message)
                .ToList();

            return errors;
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
