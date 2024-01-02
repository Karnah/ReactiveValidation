using Avalonia.Controls;

namespace ReactiveValidation.Avalonia.Samples._3._Localization;

public partial class LocalizationView : UserControl
{
    public LocalizationView()
    {
        DataContext = new LocalizationViewModel();
        InitializeComponent();
    }
}