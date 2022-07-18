using System.ComponentModel;

namespace ReactiveValidation
{
    /// <summary>
    /// Represents base interface for validatable object.
    /// </summary>
    public interface IValidatableObject : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        /// <summary>
        /// Validator of current object.
        /// </summary>
        IObjectValidator? Validator { get; set; }

        /// <summary>
        /// Raise event <see cref="INotifyDataErrorInfo.ErrorsChanged" />.
        /// </summary>
        void OnPropertyMessagesChanged(string propertyName);
    }
}