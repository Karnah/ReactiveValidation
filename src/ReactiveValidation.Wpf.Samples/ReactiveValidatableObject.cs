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


        public IObjectValidator Validator { get; protected set; }


        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        bool INotifyDataErrorInfo.HasErrors => Validator?.IsValid == false || Validator?.HasWarnings == true;


        public virtual void OnPropertyMessagesChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            return Validator?.GetMessages(propertyName);
        }
    }
}
