﻿using System.Windows.Controls;

namespace ReactiveValidation.Samples._3._Localization
{
    public partial class LocalizationView : UserControl
    {
        public LocalizationView()
        {
            InitializeComponent();

            this.DataContext = new LocalizationViewModel();
        }
    }
}
