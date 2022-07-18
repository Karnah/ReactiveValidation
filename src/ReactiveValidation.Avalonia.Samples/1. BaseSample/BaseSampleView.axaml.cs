using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReactiveValidation.Avalonia.Samples._1._BaseSample;

public partial class BaseSampleView : UserControl
{
    public BaseSampleView()
    {
        InitializeComponent();

        this.DataContext = new BaseSampleViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}