using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using ReactiveValidation.WPF.Templates;

namespace ReactiveValidation.WPF.Behaviors
{
    /// <summary>
    /// The class is designed to handle implicit validation behaviors in WPF
    /// 1. If the control was not immediately initialized or it was called Unload() - when you call Load () ErrorTemplate will not be used with a high probability (the error is due to the fact that AdornedPlaceholder does not have time to initialize)
    /// 2. If BindingMode is a OneWayToSource, then the validation error is not always displayed
    /// 3. In some cases, even if the HasError property is false, ErrorTemplate is still used
    /// </summary>
    public static class ReactiveValidation
    {
        #region AutoRefreshErrorTemplateProperty
        
        /// <summary>
        /// Additional functionality which helps with WPF broken behavior, when after unloading and loading elements - error template working incorrectly.
        /// </summary>
        public static readonly DependencyProperty AutoRefreshErrorTemplateProperty = DependencyProperty.RegisterAttached(
            "AutoRefreshErrorTemplate", typeof(bool), typeof(ReactiveValidation), new PropertyMetadata(default(bool), AutoRefreshErrorTemplatePropertyChangedCallback));

        /// <summary>
        /// Set value to <see cref="AutoRefreshErrorTemplateProperty" />.
        /// </summary>
        public static void SetAutoRefreshErrorTemplate(DependencyObject element, bool value)
        {
            element.SetValue(AutoRefreshErrorTemplateProperty, value);
        }

        /// <summary>
        /// Get value from <see cref="AutoRefreshErrorTemplateProperty" />.
        /// </summary>
        public static bool GetAutoRefreshErrorTemplate(DependencyObject element)
        {
            return (bool)element.GetValue(AutoRefreshErrorTemplateProperty);
        }

        private static void AutoRefreshErrorTemplatePropertyChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var element = obj as FrameworkElement;
            if (element == null)
                return;

            var autoRefreshTemplate = (bool) args.NewValue;
            if (autoRefreshTemplate)
            {
                element.Loaded += ElementOnLoaded;
                element.Unloaded += ElementOnUnloaded;
            }
            else
            {
                element.Loaded -= ElementOnLoaded;
                element.Unloaded -= ElementOnUnloaded;
            }
        }

        private static void ElementOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var element = sender as FrameworkElement;
            if (element == null)
                return;

            // Revalidate all Bindings.
            foreach (var be in BindingOperations.GetSourceUpdatingBindings(element))
            {
                be.UpdateSource();
            }

            // Apply error template.
            ChangeErrorTemplate(sender, routedEventArgs);

            // Subscribing on HasErrorProperty, in order to when HasError = false - disable ErrorTemplate
            // Otherwise, there are cases when ErrorTemplate is applied, but in fact there are no errors.
            DependencyPropertyDescriptor
                .FromProperty(Validation.HasErrorProperty, typeof(FrameworkElement))
                .AddValueChanged(element, ChangeErrorTemplate);
        }

        private static void ElementOnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var element = sender as FrameworkElement;
            if (element == null)
                return;

            DependencyPropertyDescriptor
                .FromProperty(Validation.HasErrorProperty, typeof(FrameworkElement))
                .RemoveValueChanged(element, ChangeErrorTemplate);

            element.ClearValue(Validation.ErrorTemplateProperty);
        }

        private static void ChangeErrorTemplate(object? sender, EventArgs eventArgs)
        {
            var element = sender as FrameworkElement;
            if (element == null)
                return;

            // Set error template to null force redraw call.
            Validation.SetErrorTemplate(element, null);

            var hasError = Validation.GetHasError(element);
            if (hasError)
            {
                var errorTemplate = GetErrorTemplate(element);
                if (errorTemplate == null)
                    return;

                Validation.SetErrorTemplate(element, errorTemplate);
            }
        }
        
        #endregion

        #region ErrorTemplateProperty

        /// <summary>
        /// Error template of control.
        /// </summary>
        public static readonly DependencyProperty ErrorTemplateProperty = DependencyProperty.RegisterAttached(
            "ErrorTemplate", typeof(ControlTemplate), typeof(ReactiveValidation), new PropertyMetadata(ErrorTemplates.WpfErrorTemplate));

        /// <summary>
        /// Set value to <see cref="ErrorTemplateProperty" />.
        /// </summary>
        public static void SetErrorTemplate(DependencyObject element, ControlTemplate value)
        {
            element.SetValue(ErrorTemplateProperty, value);
        }

        /// <summary>
        /// Get value from <see cref="ErrorTemplateProperty" />.
        /// </summary>
        public static ControlTemplate? GetErrorTemplate(DependencyObject element)
        {
            return (ControlTemplate?)element.GetValue(ErrorTemplateProperty);
        }
        
        #endregion
    }
}
