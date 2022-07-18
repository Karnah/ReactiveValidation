using System;
using System.Collections;
using System.ComponentModel;

namespace ReactiveValidation
{
    /// <inheritdoc cref="IValidatableObject" />
    public class ValidatableObject : BaseNotifyPropertyChanged, IValidatableObject
    {
        private IObjectValidator? _objectValidator;
        
        /// <inheritdoc />
        public ValidatableObject()
        {}


        /// <inheritdoc />
        public IObjectValidator? Validator
        {
            get => _objectValidator;
            set
            {
                _objectValidator?.Dispose();
                _objectValidator = value;
                _objectValidator?.Revalidate();
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
        bool INotifyDataErrorInfo.HasErrors => Validator?.IsValid == false || Validator?.HasWarnings == true;

        /// <inheritdoc />
        IEnumerable INotifyDataErrorInfo.GetErrors(string? propertyName)
        {
            if (Validator == null)
                return Array.Empty<ValidationMessage>();
            
            return string.IsNullOrEmpty(propertyName)
                ? Validator.ValidationMessages
                : Validator.GetMessages(propertyName!);
        }

        #endregion
    }
}
