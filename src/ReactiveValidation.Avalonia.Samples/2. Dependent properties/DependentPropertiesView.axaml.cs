using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReactiveValidation.Avalonia.Samples._2._Dependent_properties;

public partial class DependentPropertiesView : UserControl
{
    public DependentPropertiesView()
    {
        InitializeComponent();

        DataContext = new DependentPropertiesViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}