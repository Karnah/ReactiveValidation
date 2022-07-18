using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReactiveValidation.Avalonia.Samples._4._Inner_validatable_object_and_collection;

public partial class InnerValidatableObjectAndCollectionView : UserControl
{
    public InnerValidatableObjectAndCollectionView()
    {
        InitializeComponent();

        DataContext = new InnerValidatableObjectAndCollectionViewModel();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}