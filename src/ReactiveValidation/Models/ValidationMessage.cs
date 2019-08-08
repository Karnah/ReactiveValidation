using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ReactiveValidation
{
    public class ValidationMessage : INotifyPropertyChanged
    {
        /// <summary>
        /// Empty validation message which doesn't display or affect on <see cref="IObjectValidator.IsValid" /> property.
        /// </summary>
        public static ValidationMessage Empty = null;

        /// <summary>
        /// Source which allow dynamic get messages.
        /// </summary>
        private readonly IStringSource _stringSource;

        /// <remarks>
        /// This is very important thing. <see cref="LanguageManager.CultureChanged" /> uses a weak reference to delegates.
        /// If there is no reference to delegate, target will be collected by GC.
        /// ValidationMessage keep this reference until it will be collected.
        /// After this WeakReference will be collected too.
        /// </remarks>
        /// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly EventHandler<CultureChangedEventArgs> _eventHandler;

        /// <summary>
        /// Create new validation message with static message.
        /// </summary>
        public ValidationMessage(string message, ValidationMessageType validationMessageType = ValidationMessageType.Error)
            : this(new StaticStringSource(message), validationMessageType)
        { }

        /// <summary>
        /// Create new validation message with dynamic messages.
        /// </summary>
        public ValidationMessage(IStringSource stringSource, ValidationMessageType validationMessageType = ValidationMessageType.Error)
        {
            _stringSource = stringSource;

            ValidationMessageType = validationMessageType;
            Message = _stringSource.GetString();

            var languageManager = ValidationOptions.LanguageManager;
            if (languageManager.TrackCultureChanged)
            {
                _eventHandler = OnCultureChanged;
                languageManager.CultureChanged += _eventHandler;
            }
        }

        /// <summary>
        /// Message.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Validation message type.
        /// </summary>
        public ValidationMessageType ValidationMessageType { get; protected set; }


        /// <inheritdoc />
        public override string ToString()
        {
            return Message;
        }

        /// <summary>
        /// Handle culture changed event.
        /// </summary>
        private void OnCultureChanged(object sender, CultureChangedEventArgs args)
        {
            Message = _stringSource.GetString();
            OnPropertyChanged(nameof(Message));
        }


        #region INotifyPropertyChanged

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise <see cref="PropertyChanged" /> event.
        /// </summary>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
