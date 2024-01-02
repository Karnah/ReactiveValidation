using Avalonia.Controls;

namespace ReactiveValidation.Avalonia.Samples._6._Async_validation;

public partial class AsyncValidationView : UserControl
{
    public AsyncValidationView()
    {
        DataContext = new AsyncValidationViewModel();
        InitializeComponent();
    }
}