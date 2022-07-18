using System.Windows.Controls;

namespace ReactiveValidation.Wpf.Samples._2._Dependent_properties
{
    public partial class DependentPropertiesView : UserControl
    {
        public DependentPropertiesView()
        {
            DataContext = new DependentPropertiesViewModel();
            
            InitializeComponent();
        }
    }
}
