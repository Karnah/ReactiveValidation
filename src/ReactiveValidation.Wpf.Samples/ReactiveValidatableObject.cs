using System;
using System.Collections;
using System.ComponentModel;
using ReactiveUI;

namespace ReactiveValidation.Wpf.Samples
{
    public abstract class ReactiveValidatableObject : ReactiveObject, IValidatableObject
    {
        /// <inheritdoc />
        public IObjectValidator? Validator { get; set; }


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
