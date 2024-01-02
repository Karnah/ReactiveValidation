using Avalonia.Controls;

namespace ReactiveValidation.Avalonia.Samples._1._BaseSample;

public partial class BaseSampleView : UserControl
{
    public BaseSampleView()
    {
        DataContext = new BaseSampleViewModel();
        InitializeComponent();
    }
}