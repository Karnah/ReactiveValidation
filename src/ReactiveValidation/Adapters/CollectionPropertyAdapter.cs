using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using ReactiveValidation.Helpers;
using ReactiveValidation.Validators;

namespace ReactiveValidation.Adapters
{
    internal class CollectionPropertyAdapter<TObject, TCollection, TItem> : BaseSinglePropertyAdapter<TObject, TCollection>
        where TObject : IValidatableObject
        where TCollection : IEnumerable<TItem>
    {
        public CollectionPropertyAdapter(
            ObjectValidator<TObject> objectValidator,
            IReadOnlyCollection<IPropertyValidator<TObject, TCollection>> propertyValidators,
            string propertyName)
            : base(objectValidator, propertyValidators, propertyName)
        {
            ObserverBuilders = GetObserverBuilders();
        }


        private IEnumerable<Func<TObject, TCollection, Action, IDisposable>> GetObserverBuilders()
        {
            var observerBuilders = new List<Func<TObject, TCollection, Action, IDisposable>>();

            var propType = ReactiveValidationHelper.GetPropertyType(typeof(TObject), PropertyName);
            if (typeof(INotifyPropertyChanged).IsAssignableFrom(propType)) {
                observerBuilders.Add((o, collection, action) => new NotifyCollectionChangedSubsriber((INotifyCollectionChanged)collection, action));
            }

            foreach (var propertyObservable in ValidationOptions.CollectionObservers) {
                if (propertyObservable.CanObserve(typeof(TObject), propType)) {
                    observerBuilders.Add(propertyObservable.CreateObserver);
                }
            }

            return observerBuilders;
        }


        protected override IEnumerable<Func<TObject, TCollection, Action, IDisposable>> ObserverBuilders { get; }


        private class NotifyCollectionChangedSubsriber : IDisposable
        {
            private readonly INotifyCollectionChanged _collection;
            private readonly Action _action;

            public NotifyCollectionChangedSubsriber(INotifyCollectionChanged collection, Action action)
            {
                _collection = collection;
                _action = action;

                _collection.CollectionChanged += OnCollectionChanged;
            }

            public void Dispose()
            {
                _collection.CollectionChanged -= OnCollectionChanged;
            }

            private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
            {
                _action();
            }
        }
    }
}
