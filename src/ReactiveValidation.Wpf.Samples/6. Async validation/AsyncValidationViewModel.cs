using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveValidation.Extensions;

namespace ReactiveValidation.Wpf.Samples._6._Async_validation
{
    /// <summary>
    /// </summary>
    public class AsyncValidationViewModel : ReactiveValidatableObject
    {
        /// <inheritdoc />
        public AsyncValidationViewModel()
        {
            Validator = GetValidator();
            WaitValidatingCompletedCommand = ReactiveCommand.CreateFromTask(WaitValidatingCompletedAsync);
        }

        
        [Reactive]
        public string? PhoneNumber { get; set; }

        [Reactive]
        public string? Email { get; set; }

        public ICommand WaitValidatingCompletedCommand { get; }

        private async Task WaitValidatingCompletedAsync()
        {
            if (Validator == null)
                return;
            
            await Validator.WaitValidatingCompletedAsync();
            MessageBox.Show("Async validation has completed");
        }
        
        private IObjectValidator GetValidator()
        {
            var builder = new ValidationBuilder<AsyncValidationViewModel>();

            // For async validators it's better to use .WithPropertyCascadeMode(CascadeMode.Stop).
            // Because it wouldn't validate if previous rules failed.
            builder.PropertyCascadeMode = CascadeMode.Stop;

            builder.RuleFor(vm => vm.PhoneNumber)
                // You can also specify CascadeMode like this.
                // .WithPropertyCascadeMode(CascadeMode.Stop)
                .NotEmpty()
                .When(vm => vm.Email, email => string.IsNullOrEmpty(email))
                .Matches(@"^\d{11}$")
                // For async validation is good idea to use Throttle.
                // See ThrottleViewModel.
                .Must(CheckPhoneIsInUseAsync).WithMessage("Phone number is already using");

            builder.RuleFor(vm => vm.Email)
                // You can also specify CascadeMode like this.
                // .WithPropertyCascadeMode(CascadeMode.Stop)
                .NotEmpty()
                .When(vm => vm.PhoneNumber, phoneNumber => string.IsNullOrEmpty(phoneNumber))
                .Must(IsValidEmail)
                // For async validation is good idea to use Throttle.
                // See ThrottleViewModel.
                .Must(CheckEmailIsInUseAsync).WithMessage("Email is already using");

            return builder.Build(this);
        }

        /// <summary>
        /// Async check that phone is already using.
        /// </summary>
        private static async Task<bool> CheckPhoneIsInUseAsync(string? phoneNumber, CancellationToken cancellationToken)
        {
            await Task.Delay(3000, cancellationToken);
            return phoneNumber != "11111111111";
        }

        /// <summary>
        /// Check of email is valid.
        /// </summary>
        private static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrEmpty(email))
                return true;

            return Regex.IsMatch(email, @"^\w+@\w+\.\w+$");
        }

        /// <summary>
        /// Async check that email is already using.
        /// </summary>
        private static async Task<bool> CheckEmailIsInUseAsync(string? email, CancellationToken cancellationToken)
        {
            await Task.Delay(3000, cancellationToken);
            return email != "foo@bar.com";
        }
    }
}
