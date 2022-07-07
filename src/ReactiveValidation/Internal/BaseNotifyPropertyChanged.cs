using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ReactiveValidation.Internal
{
    /// <summary>
    /// Base class for realization of <see cref="INotifyPropertyChanged" />.
    /// </summary>
    public abstract class BaseNotifyPropertyChanged : INotifyPropertyChanged
    {
        /// <inheritdoc />
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raise <see cref="PropertyChanged" /> event.
        /// </summary>
        /// <param name="propertyName">Name of property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// If new value not equal old, set new value and raise <see cref="PropertyChanged" /> event.
        /// </summary>
        /// <typeparam name="TProp">Type of property.</typeparam>
        /// <param name="field">Field of property.</param>
        /// <param name="value">New value.</param>
        /// <param name="propertyName">Name of property.</param>
        protected virtual void SetAndRaiseIfChanged<TProp>(ref TProp field, TProp value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value))
                return;

            field = value;
            OnPropertyChanged(propertyName);
        }
    }
}
