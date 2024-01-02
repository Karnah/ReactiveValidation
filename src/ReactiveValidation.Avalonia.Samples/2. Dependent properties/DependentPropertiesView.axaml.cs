using Avalonia.Controls;

namespace ReactiveValidation.Avalonia.Samples._2._Dependent_properties;

public partial class DependentPropertiesView : UserControl
{
    public DependentPropertiesView()
    {
        DataContext = new DependentPropertiesViewModel();
        InitializeComponent();
    }
}