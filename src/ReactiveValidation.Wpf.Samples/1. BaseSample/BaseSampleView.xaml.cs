using System.Windows.Controls;

namespace ReactiveValidation.Wpf.Samples._1._BaseSample
{
    public partial class BaseSampleView : UserControl
    {
        public BaseSampleView()
        {
            DataContext = new BaseSampleViewModel();
            
            InitializeComponent();
        }
    }
}
