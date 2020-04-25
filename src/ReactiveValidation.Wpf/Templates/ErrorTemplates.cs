using System;
using System.Windows;
using System.Windows.Controls;

namespace ReactiveValidation.WPF.Templates
{
    /// <summary>
    /// Error templates for validation.
    /// </summary>
    public static class ErrorTemplates
    {
        /// <summary>
        /// Default error template for WPF application.
        /// </summary>
        public static ControlTemplate WpfErrorTemplate { get; } = GetControlTemplate("WpfErrorTemplate");

        /// <summary>
        /// Error template which support different <see cref="ValidationMessage" />.
        /// </summary>
        public static ControlTemplate ExtendedErrorTemplate { get; } = GetControlTemplate("ValidationErrorTemplate");


        /// <summary>
        /// Get control template from resource.
        /// </summary>
        /// <param name="templateName">Name of template.</param>
        private static ControlTemplate GetControlTemplate(string templateName)
        {
            var templateDictionary = new ResourceDictionary
            {
                Source = new Uri("/ReactiveValidation.Wpf;component/Themes/Generic.xaml", UriKind.RelativeOrAbsolute)
            };

            return templateDictionary[templateName] as ControlTemplate;
        }
    }
}
