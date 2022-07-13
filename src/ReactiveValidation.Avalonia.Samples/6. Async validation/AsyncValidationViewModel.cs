using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveValidation.Extensions;

namespace ReactiveValidation.Avalonia.Samples._6._Async_validation
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
            await Validator.WaitValidatingCompletedAsync();
            
            var dialog = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow("", "Async validation has ended");
            await dialog.Show();
        }
        
        private IObjectValidator GetValidator()
        {
            var builder = new ValidationBuilder<AsyncValidationViewModel>();


            builder.RuleFor(vm => vm.PhoneNumber)
                .NotEmpty()
                .When(vm => vm.Email, email => string.IsNullOrEmpty(email))
                .Matches(@"^\d{11}$")
                .Must(CheckPhoneIsInUseAsync).WithMessage("Phone number is already using");

            builder.RuleFor(vm => vm.Email)
                .NotEmpty()
                .When(vm => vm.PhoneNumber, phoneNumber => string.IsNullOrEmpty(phoneNumber))
                .Must(IsValidEmail)
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
