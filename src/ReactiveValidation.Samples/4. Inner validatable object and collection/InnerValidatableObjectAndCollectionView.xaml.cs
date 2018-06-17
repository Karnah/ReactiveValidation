using System.Windows.Controls;

namespace ReactiveValidation.Samples._4._Inner_validatable_object_and_collection
{
    public partial class InnerValidatableObjectAndCollectionView : UserControl
    {
        public InnerValidatableObjectAndCollectionView()
        {
            InitializeComponent();

            DataContext = new InnerValidatableObjectAndCollectionViewModel();
        }
    }
}
