using System;
using System.Globalization;

namespace ReactiveValidation
{
    public class CultureChangedEventArgs : EventArgs
    {
        public CultureChangedEventArgs(CultureInfo cultureInfo)
        {
            CultureInfo = cultureInfo;
        }


        public CultureInfo CultureInfo { get; }
    }
}
