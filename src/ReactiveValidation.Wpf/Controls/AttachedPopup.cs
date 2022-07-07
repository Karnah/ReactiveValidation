using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace ReactiveValidation.WPF.Controls
{
    internal class AttachedPopup : Popup
    {
        public static readonly DependencyProperty IsTargetVisibleProperty = DependencyProperty.Register(
            nameof(IsTargetVisible), typeof(bool), typeof(AttachedPopup), new PropertyMetadata(default(bool)));


        private UIElement? _lastInputHitTest;

        public AttachedPopup()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var element = sender as FrameworkElement;
            if (element == null)
                return;

            element.Loaded -= OnLoaded;

            var target = PlacementTarget as FrameworkElement;
            if (target != null) {
                target.LayoutUpdated += TargetOnLayoutUpdated;
            }

            var window = Window.GetWindow(target ?? this);
            if (window != null) {
                window.LocationChanged += WindowOnLocationChanged;
                window.SizeChanged += WindowOnSizeChanged;
            }

            CheckTargetVisibility();
        }


        public bool IsTargetVisible {
            get => (bool) GetValue(IsTargetVisibleProperty);
            set => SetValue(IsTargetVisibleProperty, value);
        }


        private void TargetOnLayoutUpdated(object sender, EventArgs eventArgs)
        {
            CheckTargetVisibility();
            ResetPlacement();
        }

        private void WindowOnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            CheckTargetVisibility();
            ResetPlacement();
        }


        private void WindowOnLocationChanged(object sender, EventArgs eventArgs)
        {
            CheckTargetVisibility();
            ResetPlacement();
        }


        private void ResetPlacement()
        {
            HorizontalOffset += 0.1;
            HorizontalOffset -= 0.1;
        }

        private void CheckTargetVisibility()
        {
            var target = PlacementTarget as FrameworkElement;
            if (target == null)
                return;

            var window = Window.GetWindow(target);
            if (window == null)
                return;


            var childCenter = target.TranslatePoint(new Point(target.ActualWidth / 2, target.ActualHeight / 2), window);
            var control = window.InputHitTest(childCenter) as UIElement;

            if (Equals(_lastInputHitTest, control))
                return;

            _lastInputHitTest = control;

            var parent = control;
            while (parent != null && Equals(target, parent) == false) {
                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }

            IsTargetVisible = parent != null;
        }
    }
}
