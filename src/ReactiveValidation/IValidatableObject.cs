using System.ComponentModel;

namespace ReactiveValidation
{
    /// <summary>
    /// Represents base interface for validatable ViewModel
    /// </summary>
    public interface IValidatableObject : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        IObjectValidator Validator { get; }


        void OnPropertyMessagesChanged(string propertyName);
    }
}