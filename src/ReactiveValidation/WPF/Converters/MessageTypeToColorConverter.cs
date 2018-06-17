using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ReactiveValidation.WPF.Converters
{
    internal class MessageTypeToColorConverter : IValueConverter
    {
        private static readonly Brush ErrorBrush = new SolidColorBrush(Colors.Red);
        private static readonly Brush WarningBrush = new SolidColorBrush(Colors.Orange);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var validationMessageType = value as ValidationMessageType?;
            switch (validationMessageType)
            {
                case null:
                case ValidationMessageType.Error:
                case ValidationMessageType.SimpleError:
                    return ErrorBrush;

                case ValidationMessageType.Warning:
                case ValidationMessageType.SimpleWarning:
                    return WarningBrush;

                default:
                    throw new ArgumentOutOfRangeException(nameof(ValidationMessageType));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
