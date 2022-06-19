using System;
using System.Collections;
using System.ComponentModel;
using ReactiveUI;

namespace ReactiveValidation.Wpf.Samples
{
    public class ReactiveValidatableObject : ReactiveObject, IValidatableObject
    {
        public ReactiveValidatableObject()
        { }


        /// <inheritdoc />
        public IObjectValidator Validator { get; set; }


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
