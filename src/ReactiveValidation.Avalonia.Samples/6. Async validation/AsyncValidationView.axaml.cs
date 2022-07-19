using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReactiveValidation.Avalonia.Samples._6._Async_validation;

public partial class AsyncValidationView : UserControl
{
    public AsyncValidationView()
    {
        DataContext = new AsyncValidationViewModel();
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}