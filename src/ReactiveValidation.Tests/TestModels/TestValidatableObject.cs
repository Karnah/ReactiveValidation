using System.Collections.ObjectModel;

namespace ReactiveValidation.Tests.TestModels
{
    public class TestValidatableObject : ValidatableObject
    {
        private int _number;
        public int Number
        {
            get => _number;
            set => SetAndRaiseIfChanged(ref _number, value);
        }

        private TestValidatableObject? _innerValidatableObject;
        public TestValidatableObject? InnerValidatableObject
        {
            get => _innerValidatableObject;
            set => SetAndRaiseIfChanged(ref _innerValidatableObject, value);
        }

        private ObservableCollection<TestValidatableObject?>? _collection;
        public ObservableCollection<TestValidatableObject?>? Collection
        {
            get => _collection;
            set => SetAndRaiseIfChanged(ref _collection, value);
        }

        public void RaisePropertyChangedEvent(string? propertyName)
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}
