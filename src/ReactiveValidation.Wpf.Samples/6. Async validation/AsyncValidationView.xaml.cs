using System.Windows.Controls;

namespace ReactiveValidation.Wpf.Samples._6._Async_validation
{
    public partial class AsyncValidationView : UserControl
    {
        public AsyncValidationView()
        {
            DataContext = new AsyncValidationViewModel();

            InitializeComponent();
        }
    }
}
