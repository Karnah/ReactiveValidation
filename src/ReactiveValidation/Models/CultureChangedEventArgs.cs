using System;
using System.Globalization;

namespace ReactiveValidation
{
    /// <summary>
    /// Arguments of culture changed event.
    /// </summary>
    public class CultureChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Create culture changed event arguments.
        /// </summary>
        /// <param name="cultureInfo">New culture of validation.</param>
        public CultureChangedEventArgs(CultureInfo cultureInfo)
        {
            CultureInfo = cultureInfo;
        }

        /// <summary>
        /// New culture of validation.
        /// </summary>
        public CultureInfo CultureInfo { get; }
    }
}
