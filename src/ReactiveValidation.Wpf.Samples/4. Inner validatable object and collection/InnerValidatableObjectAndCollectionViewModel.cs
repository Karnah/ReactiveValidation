using System.Collections.ObjectModel;
using System.Windows.Input;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveValidation.Extensions;

namespace ReactiveValidation.Wpf.Samples._4._Inner_validatable_object_and_collection
{
    /// <summary>
    /// This sample also shows the use of ReactiveUI and ReactiveUI.Fody.
    /// More information: https://reactiveui.net/ and https://github.com/kswoll/ReactiveUI.Fody
    /// Pay attention to the base class - it's inherit from ReactiveObject
    /// </summary>
    public class InnerValidatableObjectAndCollectionViewModel : ReactiveValidatableObject
    {
        public InnerValidatableObjectAndCollectionViewModel()
        {
            InnerObjectValue = new InnerObject();
            InnerObjectsCollection = new ObservableCollection<InnerObject>();

            AddItemCommand = ReactiveCommand.Create(AddItem);
            DeleteItemCommand = ReactiveCommand.Create<InnerObject>(DeleteItem);

            Validator = GetValidator();
        }


        private IObjectValidator GetValidator()
        {
            var builder = new ValidationBuilder<InnerValidatableObjectAndCollectionViewModel>();


            builder.RuleFor(vm => vm.InnerObjectValue)
                .SetValueValidator(GetInnerObjectValidator)
                .NotNull()
                .ModelIsValid();

            builder.RuleForCollection(vm => vm.InnerObjectsCollection)
                .SetCollectionItemValidator(GetInnerObjectValidator)
                .NotNull()
                .Count(3, 5)
                .CollectionElementsAreValid();

            return builder.Build(this);
        }

        public InnerObject InnerObjectValue { get; }

        public ObservableCollection<InnerObject> InnerObjectsCollection { get; }


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

        private static IObjectValidator GetInnerObjectValidator(InnerObject io)
        {
            var builder = new ValidationBuilder<InnerObject>();

            builder.RuleFor(vm => vm.Name)
                .NotEmpty();

            return builder.Build(io);
        }

        public class InnerObject : ReactiveValidatableObject
        {
            public InnerObject()
            {
            }

            [Reactive]
            public string? Name { get; set; }
        }
    }
}
