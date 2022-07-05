using ReactiveValidation.Extensions;

namespace ReactiveValidation.Wpf.Samples._1._BaseSample
{
    public class BaseSampleViewModel : ValidatableObject
    {
        public BaseSampleViewModel()
        {
            Validator = GetValidator();
        }

        private IObjectValidator GetValidator()
        {
            var builder = new ValidationBuilder<BaseSampleViewModel>();

            builder.RuleFor(vm => vm.Name)
                .NotEmpty()
                .MaxLength(16)
                .NotEqual("foo");

            builder.RuleFor(vm => vm.Surname)
                .Equal("foo");

            builder.RuleFor(vm => vm.PhoneNumber)
                .NotEmpty()
                .Matches(@"^\d{9,12}$");
            
            builder
                .TransformToInt(vm => vm.AgeString)
                .NotNull().WithMessage("Age should be valid integer string")
                .Between(18, 35);
            
            return builder.Build(this);
        }


        private string _name;
        public string Name {
            get => _name;
            set {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _surname;
        public string Surname {
            get => _surname;
            set {
                _surname = value;
                OnPropertyChanged();
            }
        }

        private string _phoneNumber;
        public string PhoneNumber {
            get => _phoneNumber;
            set {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        private string _ageString;
        public string AgeString {
            get => _ageString;
            set {
                _ageString = value;
                OnPropertyChanged();
            }
        }
    }
}
