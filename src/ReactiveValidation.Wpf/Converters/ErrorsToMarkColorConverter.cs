using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ReactiveValidation.WPF.Converters
{
    internal class ErrorsToMarkColorConverter : IMultiValueConverter
    {
        private static readonly Brush EmptyBrush = new SolidColorBrush(Colors.Transparent);
        private static readonly Brush ErrorBrush = new SolidColorBrush(Colors.Red);
        private static readonly Brush WarningBrush = new SolidColorBrush(Colors.Orange);

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var errors = (values[0] as IReadOnlyCollection<ValidationError>)
                ?.Select(ve => ve.ErrorContent)
                .OfType<ValidationMessage>()
                .ToList();
            if (errors?.Any() != true)
                return EmptyBrush;

            if (errors.Any(vm => vm?.ValidationMessageType == ValidationMessageType.Error ||
                                 vm?.ValidationMessageType == ValidationMessageType.SimpleError))
                return ErrorBrush;

            return WarningBrush;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
