using System;
using System.Collections.Generic;
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
        /// List of using brushes for templates.
        /// </summary>
        private static readonly IReadOnlyList<string> Brushes = new[]{ "ValidationErrorBrush", "ValidationWarningBrush", "ValidationMessageForegroundBrush", "ValidationMessageBackgroundBrush" };

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

            var template = (ControlTemplate)templateDictionary[templateName];

            // WPF weird behavior: if template is get from ResourceDictionary in code, it doesn't have access to local resources of this file.
            // So, we check if this brush overriden at resources of application.
            // If not we put default value from local resource file.
            // Else it will be used automatically.
            var applicationResources = Application.Current.Resources;
            foreach (var brushName  in Brushes)
            {
                if (!applicationResources.Contains(brushName))
                    template.Resources.Add(brushName, templateDictionary[brushName]);
            }
            
            return template;
        }
    }
}
