using Avalonia.Controls;

namespace ReactiveValidation.Avalonia.Samples._4._Inner_validatable_object_and_collection;

public partial class InnerValidatableObjectAndCollectionView : UserControl
{
    public InnerValidatableObjectAndCollectionView()
    {
        DataContext = new InnerValidatableObjectAndCollectionViewModel();
        InitializeComponent();
    }
}