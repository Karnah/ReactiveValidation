using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ReactiveValidation
{
    public class ValidationMessage : INotifyPropertyChanged
    {
        public static ValidationMessage Empty = null;


        private readonly IStringSource _stringSource;

        public ValidationMessage(string message, ValidationMessageType validationMessageType = ValidationMessageType.Error)
            : this(new StaticStringSource(message), validationMessageType)
        { }

        public ValidationMessage(IStringSource stringSource, ValidationMessageType validationMessageType = ValidationMessageType.Error)
        {
            _stringSource = stringSource;

            ValidationMessageType = validationMessageType;
            Message = _stringSource.GetString();

            if (ValidationOptions.LanguageManager.TrackCultureChanged == true) {
                WeakEventManager<ILanguageManager, CultureChangedEventArgs>.AddHandler(
                    ValidationOptions.LanguageManager, nameof(ILanguageManager.CultureChanged), OnCultureChanged);
            }
        }

        private void OnCultureChanged(object sender, CultureChangedEventArgs args)
        {
            Message = _stringSource.GetString();
            OnPropertyChanged(nameof(Message));
        }


        public string Message { get; private set; }

        public ValidationMessageType ValidationMessageType { get; protected set; }


        public override string ToString()
        {
            return Message;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
