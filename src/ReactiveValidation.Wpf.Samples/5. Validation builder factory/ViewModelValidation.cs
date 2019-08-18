using System.Text.RegularExpressions;
using ReactiveValidation.Extensions;

namespace ReactiveValidation.Wpf.Samples._5._Validation_builder_factory
{
    public class ViewModelValidation : ValidationRuleBuilder<ValidationBuilderFactoryViewModel>
    {
        /// <inheritdoc />
        public ViewModelValidation()
        {
            RuleFor(vm => vm.PhoneNumber)
                .NotEmpty()
                .When(vm => vm.Email, email => string.IsNullOrEmpty(email))
                .Matches(@"^\d{11}$");

            RuleFor(vm => vm.Email)
                .NotEmpty()
                .When(vm => vm.PhoneNumber, phoneNumber => string.IsNullOrEmpty(phoneNumber))
                .Must(IsValidEmail);


            RuleFor(vm => vm.Password)
                .NotEmpty()
                .MinLength(8, ValidationMessageType.Warning);

            RuleFor(vm => vm.ConfirmPassword)
                .Equal(vm => vm.Password);


            RuleFor(vm => vm.Country)
                .NotEmpty()
                .AllWhen(vm => vm.AdditionalInformation);

            RuleFor(vm => vm.City)
                .NotEmpty()
                .AllWhen(vm => vm.AdditionalInformation);
        }

        /// <summary>
        /// Check of email is valid.
        /// </summary>
        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return true;

            return Regex.IsMatch(email, @"^\w+@\w+.\w+$");
        }
    }
}
