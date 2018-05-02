using System;
using System.Reactive.Disposables;
using System.Windows.Input;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveValidation.Extensions;

namespace ReactiveValidation.Samples._4._Inner_validatable_object_and_collection
{
    public class InnerValidatableObjectAndCollectionViewModel : ReactiveValidatableObject
    {
        public InnerValidatableObjectAndCollectionViewModel()
        {
            //If you are using Reactive UI, you can add custom logic to ReactiveList
            //In your project this is better do in App.xaml.cs
            ValidationOptions.AddCollectionObserver(CanObserve, CreateReactiveCollectionItemChangedObserver);


            InnerObjectValue = new InnerObject();
            InnerObjectsCollection = new ReactiveList<InnerObject>() {
                ChangeTrackingEnabled = true
            };

            AddItemCommand = ReactiveCommand.Create(AddItem);
            DeleteItemCommand = ReactiveCommand.Create<InnerObject>(DeleteItem);

            this.Validator = GetValidator();
        }


        private static bool CanObserve(Type objectType, Type propertyType)
        {
            return typeof(IReactiveNotifyCollectionItemChanged<object>).IsAssignableFrom(propertyType);
        }

        private static IDisposable CreateReactiveCollectionItemChangedObserver(object o, object propertyValue, Action action)
        {
            if (propertyValue is IReactiveNotifyCollectionItemChanged<object> collection) {
                return collection.ItemChanged.Subscribe(args => action());
            }

            return Disposable.Empty;
        }


        private IObjectValidator GetValidator()
        {
            var builder = new ValidationBuilder<InnerValidatableObjectAndCollectionViewModel>();


            builder.RuleFor(vm => vm.InnerObjectValue)
                .NotNull()
                .ModelIsValid();

            builder.RuleForCollection(vm => vm.InnerObjectsCollection)
                .NotNull()
                .Count(3, 5)
                .CollectionElementsAreValid(i => i.Validator?.IsValid != false);


            return builder.Build(this);
        }


        [Reactive]
        public InnerObject InnerObjectValue { get; set; }

        [Reactive]
        public ReactiveList<InnerObject> InnerObjectsCollection { get; set; }


        public ICommand AddItemCommand { get; }

        public ICommand DeleteItemCommand { get; }


        private void AddItem()
        {
            InnerObjectsCollection.Add(new InnerObject());
        }

        private void DeleteItem(InnerObject item)
        {
            InnerObjectsCollection.Remove(item);
        }


        public class InnerObject : ReactiveValidatableObject
        {
            public InnerObject()
            {
                this.Validator = GetValidator();
            }

            private IObjectValidator GetValidator()
            {
                var builder = new ValidationBuilder<InnerObject>();

                builder.RuleFor(vm => vm.Name)
                    .NotEmpty();

                return builder.Build(this);
            }


            [Reactive]
            public string Name { get; set; }
        }
    }
}
