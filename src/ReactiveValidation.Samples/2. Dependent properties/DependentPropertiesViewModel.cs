using System.Text.RegularExpressions;

using ReactiveUI.Fody.Helpers;
using ReactiveValidation.Extensions;

namespace ReactiveValidation.Samples._2._Dependent_properties
{
    public class DependentPropertiesViewModel : ReactiveValidatableObject
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
                    .When(vm => Email, email => string.IsNullOrEmpty(email) == true)
                    .WithMessage("You need to specify a phone or email")
                .Matches(@"^\d{11}$")
                    .WithMessage("Phone number must contain 11 digits");

            builder.RuleFor(vm => vm.Email)
                .NotEmpty()
                    .When(vm => PhoneNumber, phoneNumber => string.IsNullOrEmpty(phoneNumber) == true)
                    .WithMessage("You need to specify a phone or email")
                .Must(IsValidEmail)
                    .WithMessage("Not valid email");


            builder.RuleFor(vm => vm.Password)
                .NotEmpty()
                .MinLength(8, ValidationMessageType.Warning);

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
            if (string.IsNullOrEmpty(email) == true)
                return true;

            return Regex.IsMatch(email, @"^\w+@\w+.\w+$");
        }


        [Reactive]
        public string PhoneNumber { get; set; }

        [Reactive]
        public string Email { get; set; }

        [Reactive]
        public string Password { get; set; }

        [Reactive]
        public string ConfirmPassword { get; set; }

        [Reactive]
        public bool AdditionalInformation { get; set; }

        [Reactive]
        public string Country { get; set; }

        [Reactive]
        public string City { get; set; }
    }
}
