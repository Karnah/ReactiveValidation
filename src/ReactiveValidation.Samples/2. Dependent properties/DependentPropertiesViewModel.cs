using System.Text.RegularExpressions;

using ReactiveValidation.Extensions;

namespace ReactiveValidation.Samples._2._Dependent_properties
{
    public class DependentPropertiesViewModel : ValidatableObject
    {
        public DependentPropertiesViewModel()
        {
            Validator = GetValidator();
        }

        private IObjectValidator GetValidator()
        {
            var builder = new ValidationBuilder<DependentPropertiesViewModel>();

            builder.RuleFor(vm => vm.PhoneNumber)
                .NotEmpty()
                    .When(vm => Email, string.IsNullOrEmpty)
                    .WithMessage("You need to specify a phone or email")
                .Matches(@"^\d{11}$")
                    .WithMessage("Phone number must contain 11 digits");

            builder.RuleFor(vm => vm.Email)
                .NotEmpty()
                    .When(vm => PhoneNumber, string.IsNullOrEmpty)
                    .WithMessage("You need to specify a phone or email")
                .Must(IsValidEmail)
                    .WithMessage("Not valid email");


            builder.RuleFor(vm => vm.Password)
                .NotEmpty()
                .MinLength(8, ValidationMessageType.Warning)
                    .WithMessage("For a secure password, enter more than {MinLength} characters. You entered {TotalLength} characters");

            builder.RuleFor(vm => vm.ConfirmPassword)
                .Equal(vm => vm.Password);


            builder.RuleFor(vm => vm.Country)
                .NotEmpty()
                .AllWhen(vm => vm.AdditionalInformation);

            builder.RuleFor(vm => vm.City)
                .NotEmpty()
                .AllWhen(vm => vm.AdditionalInformation);


            return builder.Build(this);
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return true;

            return Regex.IsMatch(email, @"^\w+@\w+.\w+$");
        }


        private string _phoneNumber;
        public string PhoneNumber {
            get => _phoneNumber;
            set {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        private string _email;
        public string Email {
            get => _email;
            set {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password {
            get => _password;
            set {
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _confirmPassword;
        public string ConfirmPassword {
            get => _confirmPassword;
            set {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }

        private bool _additionalInformation;
        public bool AdditionalInformation {
            get => _additionalInformation;
            set {
                _additionalInformation = value;
                OnPropertyChanged();
            }
        }

        private string _country;
        public string Country {
            get => _country;
            set {
                _country = value;
                OnPropertyChanged();
            }
        }

        private string _city;
        public string City {
            get => _city;
            set {
                _city = value;
                OnPropertyChanged();
            }
        }
    }
}
