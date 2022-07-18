using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReactiveValidation.Avalonia.Samples._3._Localization;

public partial class LocalizationView : UserControl
{
    public LocalizationView()
    {
        InitializeComponent();

        DataContext = new LocalizationViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}