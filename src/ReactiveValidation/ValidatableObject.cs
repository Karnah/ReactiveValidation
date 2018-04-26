using System;
using System.Collections;
using System.ComponentModel;

using ReactiveUI;

namespace ReactiveValidation
{
    public class ValidatableObject : ReactiveObject, IValidatableObject
    {
        public ValidatableObject()
        {}

        public IObjectValidator Validator { get; protected set; }



        private event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        event EventHandler<DataErrorsChangedEventArgs> INotifyDataErrorInfo.ErrorsChanged {
            add => this.ErrorsChanged += value;
            remove => this.ErrorsChanged -= value;
        }


        bool INotifyDataErrorInfo.HasErrors => Validator?.IsValid == false || Validator?.HasWarnings == true;


        void IValidatableObject.OnPropertyMessagesChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            return Validator?.GetMessages(propertyName);
        }
    }
}
