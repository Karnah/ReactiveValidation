using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ReactiveValidation
{
    /// <inheritdoc />
    public class ValidatableObject : IValidatableObject
    {
        private IObjectValidator _validator;

        /// <inheritdoc />
        public ValidatableObject()
        {}


        /// <inheritdoc />
        public IObjectValidator Validator
        {
            get => _validator;
            set
            {
                if (_validator == value)
                    return;

                _validator = value;
                OnPropertyChanged();
            }
        }


        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc />
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        /// <inheritdoc />
        bool INotifyDataErrorInfo.HasErrors => Validator?.IsValid == false || Validator?.HasWarnings == true;

        /// <summary>
        /// Raise event <see cref="INotifyPropertyChanged.PropertyChanged" />.
        /// </summary>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <inheritdoc />
        public virtual void OnPropertyMessagesChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        /// <inheritdoc />
        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            return Validator?.GetMessages(propertyName);
        }
    }
}
