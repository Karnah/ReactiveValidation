using System;
using System.Collections;
using System.ComponentModel;
using ReactiveUI;

namespace ReactiveValidation.Avalonia.Samples
{
    public abstract class ReactiveValidatableObject : ReactiveObject, IValidatableObject
    {
        private IObjectValidator? _objectValidator;
        
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
        public virtual void OnPropertyMessagesChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        
        #region INotifyDataErrorInfo

        /// <inheritdoc />
        bool INotifyDataErrorInfo.HasErrors => Validator?.IsValid == false || Validator?.HasWarnings == true;

        /// <inheritdoc />
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        
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