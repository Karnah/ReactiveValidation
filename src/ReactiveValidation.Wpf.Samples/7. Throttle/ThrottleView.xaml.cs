using System.Windows.Controls;

namespace ReactiveValidation.Wpf.Samples._7._Throttle
{
    public partial class ThrottleView : UserControl
    {
        public ThrottleView()
        {
            DataContext = new ThrottleViewModel();

            InitializeComponent();
        }
    }
}
