using Avalonia.Controls;

namespace ReactiveValidation.Avalonia.Samples._5._Validation_builder_factory;

public partial class ValidationBuilderFactoryView : UserControl
{
    public ValidationBuilderFactoryView()
    {
        DataContext = new ValidationBuilderFactoryViewModel();
        InitializeComponent();
    }
}