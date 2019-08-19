using System.Windows.Controls;

namespace ReactiveValidation.Wpf.Samples._5._Validation_builder_factory
{
    public partial class ValidationBuilderFactoryView : UserControl
    {
        public ValidationBuilderFactoryView()
        {
            DataContext = new ValidationBuilderFactoryViewModel();

            InitializeComponent();
        }
    }
}
