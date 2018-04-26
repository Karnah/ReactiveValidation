using System;
using System.Collections.Generic;
using System.Collections.Specialized;

using ReactiveUI;
using ReactiveValidation.Validators;

namespace ReactiveValidation.Adapters
{
    internal class CollectionPropertyAdapter<TObject, TCollection, TProp> : BaseSinglePropertyAdapter<TObject, TCollection>
        where TObject : IValidatableObject
        where TCollection : IEnumerable<TProp>
    {
        public CollectionPropertyAdapter(
            ObjectValidator<TObject> objectValidator,
            IReadOnlyCollection<IPropertyValidator<TObject, TCollection>> propertyValidators,
            string propertyName)
            : base(objectValidator, propertyValidators, propertyName)
        { }


        protected override bool IsPropertyTypeObservable()
        {
            var isObservableCollection = typeof(TProp).IsAssignableFrom(typeof(INotifyCollectionChanged));
            var isReactiveCollection = typeof(TProp).IsAssignableFrom(typeof(IReactiveNotifyCollectionItemChanged<>));

            return isObservableCollection || isReactiveCollection;
        }

        protected override IEnumerable<IDisposable> SubsribeToProperty(TCollection property)
        {
            if (property is INotifyCollectionChanged observableCollection) {
                yield return new NotificationsSubscriber<INotifyCollectionChanged>(observableCollection,
                    SubscribeCollectionChanged, UnsubscribeCollectionChanged);
            }

            if (property is IReactiveNotifyCollectionItemChanged<TProp> reactiveCollection) {
                yield return reactiveCollection.ItemChanged.Subscribe(args => Revalidate());
            }
        }


        private void SubscribeCollectionChanged(INotifyCollectionChanged observableProperty)
        {
            observableProperty.CollectionChanged += OnCollectionChanged;
        }

        private void UnsubscribeCollectionChanged(INotifyCollectionChanged observableProperty)
        {
            observableProperty.CollectionChanged -= OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            Revalidate();
        }
    }
}
