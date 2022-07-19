using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReactiveValidation.Avalonia.Samples._5._Validation_builder_factory;

public partial class ValidationBuilderFactoryView : UserControl
{
    public ValidationBuilderFactoryView()
    {
        DataContext = new ValidationBuilderFactoryViewModel();
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}