using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        {
            ObserverBuilders = GetObserverBuilders();
        }


        private IEnumerable<Func<TObject, TProp, Action, IDisposable>> GetObserverBuilders()
        {
            var observables = new List<Func<TObject, TProp, Action, IDisposable>>();

            if (typeof(INotifyPropertyChanged).IsAssignableFrom(typeof(TProp))) {
                observables.Add((o, prop, action) => new NotifyPropertyChangedSubsriber((INotifyPropertyChanged) prop, action));
            }

            foreach (var propertyObservable in ValidationOptions.PropertyObservers) {
                if (propertyObservable.CanObserve<TObject, TProp>()) {
                    observables.Add(propertyObservable.CreateObserver);
                }
            }

            return observables;
        }


        protected override IEnumerable<Func<TObject, TProp, Action, IDisposable>> ObserverBuilders { get; }


        private class NotifyPropertyChangedSubsriber : IDisposable
        {
            private readonly INotifyPropertyChanged _property;
            private readonly Action _action;

            public NotifyPropertyChangedSubsriber(INotifyPropertyChanged property, Action action)
            {
                _property = property;
                _action = action;

                _property.PropertyChanged += OnPropertyChanged;
            }

            public void Dispose()
            {
                _property.PropertyChanged -= OnPropertyChanged;
            }

            private void OnPropertyChanged(object o, PropertyChangedEventArgs args)
            {
                _action();
            }
        }
    }
}
