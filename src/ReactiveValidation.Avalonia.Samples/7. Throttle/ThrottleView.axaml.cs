using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReactiveValidation.Avalonia.Samples._7._Throttle;

public partial class ThrottleView : UserControl
{
    public ThrottleView()
    {
        DataContext = new ThrottleViewModel();
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}