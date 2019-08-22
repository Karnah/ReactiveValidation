using System;
using System.Collections;
using System.ComponentModel;

using ReactiveValidation.Exceptions;
using ReactiveValidation.Internal;

namespace ReactiveValidation
{
    /// <inheritdoc cref="IValidatableObject" />
    public class ValidatableObject : BaseNotifyPropertyChanged, IValidatableObject
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
                if (_validator != null)
                    throw new MethodAlreadyCalledException("Object already has validator");

                SetAndRaiseIfChanged(ref _validator, value);
            }
        }


        /// <inheritdoc />
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        /// <inheritdoc />
        bool INotifyDataErrorInfo.HasErrors => Validator?.IsValid == false || Validator?.HasWarnings == true;

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
