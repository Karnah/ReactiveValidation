﻿using System.Windows.Controls;

namespace ReactiveValidation.Wpf.Samples._3._Localization
{
    public partial class LocalizationView : UserControl
    {
        public LocalizationView()
        {
            DataContext = new LocalizationViewModel();
            
            InitializeComponent();
        }
    }
}
