using System;
using System.Windows;
using System.Windows.Controls;

namespace ReactiveValidation.WPF.Templates
{
    public static class ErrorTemplates
    {
        public static ControlTemplate WpfErrorTemplate { get; } = GetControlTemplate("WpfErrorTemplate");

        public static ControlTemplate ExtendedErrorTemplate { get; } = GetControlTemplate("ValidationErrorTemplate");


        private static ControlTemplate GetControlTemplate(string templateName)
        {
            var templateDictionary = new ResourceDictionary {
                Source = new Uri("/ReactiveValidation.Wpf;component/Themes/Generic.xaml", UriKind.RelativeOrAbsolute)
            };

            return templateDictionary[templateName] as ControlTemplate;
        }
    }
}
