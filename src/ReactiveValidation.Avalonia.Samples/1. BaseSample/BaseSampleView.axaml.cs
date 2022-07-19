using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReactiveValidation.Avalonia.Samples._1._BaseSample;

public partial class BaseSampleView : UserControl
{
    public BaseSampleView()
    {
        DataContext = new BaseSampleViewModel();
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}