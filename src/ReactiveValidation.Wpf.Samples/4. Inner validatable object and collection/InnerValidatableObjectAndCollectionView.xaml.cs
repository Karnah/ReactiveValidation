using System.Windows.Controls;

namespace ReactiveValidation.Wpf.Samples._4._Inner_validatable_object_and_collection
{
    public partial class InnerValidatableObjectAndCollectionView : UserControl
    {
        public InnerValidatableObjectAndCollectionView()
        {
            DataContext = new InnerValidatableObjectAndCollectionViewModel();
            
            InitializeComponent();
        }
    }
}
