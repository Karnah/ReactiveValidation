using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using MsBox.Avalonia;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveValidation.Extensions;

namespace ReactiveValidation.Avalonia.Samples._7._Throttle
{
    /// <summary>
    /// </summary>
    public class ThrottleViewModel : ReactiveValidatableObject
    {
        /// <inheritdoc />
        public ThrottleViewModel()
        {
            Validator = GetValidator();
            WaitValidatingCompletedCommand = ReactiveCommand.CreateFromTask(WaitValidatingCompletedAsync);
        }

        
        [Reactive]
        public string? PhoneNumber { get; set; }

        [Reactive]
        public bool IsEmailEnabled { get; set; }
        
        [Reactive]
        public string? Email { get; set; }

        public ICommand WaitValidatingCompletedCommand { get; }

        private async Task WaitValidatingCompletedAsync()
        {
            if (Validator == null)
                return;
            
            await Validator.WaitValidatingCompletedAsync();
            
            var dialog = MessageBoxManager
                    .GetMessageBoxStandard("", "Async validation has completed");
            await dialog.ShowWindowDialogAsync(App.MainWindow);
        }

        private IObjectValidator GetValidator()
        {
            var builder = new ValidationBuilder<ThrottleViewModel>();

            builder.RuleFor(vm => vm.PhoneNumber)
                .NotEmpty()
                .Matches(@"^\d{11}$")
                .CommonThrottle(2000);

            builder.RuleFor(vm => vm.Email)
                .NotEmpty()
                .Must(IsValidEmail)
                    .Throttle(1000)
                .AllWhen(vm => vm.IsEmailEnabled)
                .CommonThrottle(throttleBuilder => throttleBuilder
                    .AddRelatedPropertyThrottle(vm => vm.IsEmailEnabled, 3000));

            return builder.Build(this);
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
    }
}
