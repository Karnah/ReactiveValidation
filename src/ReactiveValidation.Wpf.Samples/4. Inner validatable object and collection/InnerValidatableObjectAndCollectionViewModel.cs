using System.Windows.Input;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Legacy;
using ReactiveValidation.Extensions;

namespace ReactiveValidation.Wpf.Samples._4._Inner_validatable_object_and_collection
{
    /// <summary>
    /// This sample also shows the use of ReactiveUI and ReactiveUI.Fody.
    /// More information: https://reactiveui.net/ and https://github.com/kswoll/ReactiveUI.Fody
    /// Pay attention to the base class - it's inherit from ReactiveObject
    ///
    /// Please see how setup CollectionObserver in <see cref="App.OnStartup" />.
    /// </summary>
    public class InnerValidatableObjectAndCollectionViewModel : ReactiveValidatableObject
    {
        public InnerValidatableObjectAndCollectionViewModel()
        {
            InnerObjectValue = new InnerObject();
            InnerObjectsCollection = new ReactiveList<InnerObject>() {
                ChangeTrackingEnabled = true
            };

            AddItemCommand = ReactiveCommand.Create(AddItem);
            DeleteItemCommand = ReactiveCommand.Create<InnerObject>(DeleteItem);

            Validator = GetValidator();
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
