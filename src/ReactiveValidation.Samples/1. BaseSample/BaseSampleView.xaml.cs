using System.Windows.Controls;

namespace ReactiveValidation.Samples._1._BaseSample
{
    public partial class BaseSampleView : UserControl
    {
        public BaseSampleView()
        {
            InitializeComponent();

            DataContext = new BaseSampleViewModel();
        }
    }
}
