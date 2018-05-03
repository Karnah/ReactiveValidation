using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ReactiveValidation
{
    public class ValidatableObject : IValidatableObject
    {
        public ValidatableObject()
        {}


        private IObjectValidator _validator;
        public IObjectValidator Validator {
            get => _validator;
            protected set {
                if (_validator == value)
                    return;

                _validator = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        bool INotifyDataErrorInfo.HasErrors => Validator?.IsValid == false || Validator?.HasWarnings == true;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
