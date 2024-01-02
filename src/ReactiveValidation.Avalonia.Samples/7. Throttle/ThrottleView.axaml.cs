using Avalonia.Controls;

namespace ReactiveValidation.Avalonia.Samples._7._Throttle;

public partial class ThrottleView : UserControl
{
    public ThrottleView()
    {
        DataContext = new ThrottleViewModel();
        InitializeComponent();
    }
}