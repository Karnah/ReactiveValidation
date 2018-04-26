using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Disposables;

using ReactiveValidation.Validators;

namespace ReactiveValidation.Adapters
{
    internal class SinglePropertyAdapter<TObject, TProp> : BaseSinglePropertyAdapter<TObject, TProp>
        where TObject : IValidatableObject
    {
        public SinglePropertyAdapter(
            ObjectValidator<TObject> objectValidator,
            IReadOnlyCollection<IPropertyValidator<TObject, TProp>> propertyValidators,
            string propertyName)
            : base(objectValidator, propertyValidators, propertyName)
        { }


        protected override bool IsPropertyTypeObservable()
        {
            return typeof(TProp).IsAssignableFrom(typeof(INotifyPropertyChanged));
        }

        protected override IEnumerable<IDisposable> SubsribeToProperty(TProp property)
        {
            var observableProperty = property as INotifyPropertyChanged;
            if (observableProperty == null)
                return new[] { Disposable.Empty };

            return new[] { new NotificationsSubscriber<INotifyPropertyChanged>(observableProperty, Subscribe, Unsubscribe) };
        }


        private void Subscribe(INotifyPropertyChanged observableProperty)
        {
            observableProperty.PropertyChanged += ObservablePropertyOnPropertyChanged;
        }

        private void Unsubscribe(INotifyPropertyChanged observableProperty)
        {
            observableProperty.PropertyChanged -= ObservablePropertyOnPropertyChanged;
        }

        private void ObservablePropertyOnPropertyChanged(object o, PropertyChangedEventArgs args)
        {
            Revalidate();
        }
    }
}
