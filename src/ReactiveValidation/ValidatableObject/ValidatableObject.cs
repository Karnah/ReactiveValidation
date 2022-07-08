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
        private IObjectValidator? _validator;

        /// <inheritdoc />
        public ValidatableObject()
        {}


        /// <inheritdoc />
        public IObjectValidator Validator
        {
            get => _validator ?? throw new ValidationSettingsException("Validator is not set");
            set
            {
                if (_validator != null)
                    throw new MethodAlreadyCalledException("Object already has validator");

                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                
                SetAndRaiseIfChanged(ref _validator, value);
            }
        }


        /// <inheritdoc />
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
     
        
        /// <inheritdoc />
        public virtual void OnPropertyMessagesChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }


        #region INotifyDataErrorInfo

        /// <inheritdoc />
        bool INotifyDataErrorInfo.HasErrors => _validator?.IsValid == false || _validator?.HasWarnings == true;

        /// <inheritdoc />
        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            return Validator.GetMessages(propertyName);
        }

        #endregion
    }
}
