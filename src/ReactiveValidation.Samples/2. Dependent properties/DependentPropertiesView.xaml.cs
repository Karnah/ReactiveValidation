using System.Windows.Controls;

namespace ReactiveValidation.Samples._2._Dependent_properties
{
    public partial class DependentPropertiesView : UserControl
    {
        public DependentPropertiesView()
        {
            InitializeComponent();

            DataContext = new DependentPropertiesViewModel();
        }
    }
}
