using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace ReactiveValidation.WPF.Converters
{
    internal class ShowValidationPopupConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var isTargetVisible = (bool) values[0];
            if (isTargetVisible == false)
                return false;

            var isKeyboardFocused = (bool) values[1];
            var isMouseOver = (bool) values[2];
            if (isKeyboardFocused == false && isMouseOver == false)
                return false;


            var errors = values[3] as IReadOnlyCollection<ValidationError>;
            if (errors == null)
                throw new ArgumentNullException(nameof(errors));


            var existsVisibleMessages =
                errors.Select(result => result.ErrorContent as ValidationMessage)
                    .Any(message => message?.ValidationMessageType == ValidationMessageType.Error ||
                                    message?.ValidationMessageType == ValidationMessageType.Warning ||
                                    isMouseOver == true);

            return existsVisibleMessages == true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
