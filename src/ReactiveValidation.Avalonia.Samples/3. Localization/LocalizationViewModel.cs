using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveValidation.Attributes;
using ReactiveValidation.Extensions;

namespace ReactiveValidation.Avalonia.Samples._3._Localization
{
    /// <summary>
    /// This sample also shows the use of ReactiveUI and ReactiveUI.Fody.
    /// More information: https://docs.avaloniaui.net/guides/deep-dives/reactiveui and https://github.com/kswoll/ReactiveUI.Fody
    /// Pay attention to the base class - it's inherit from ReactiveObject.
    ///
    /// Please see how setup base ResourceManager and TrackCultureChanged in <see cref="Program.BuildAvaloniaApp" />.
    /// </summary>
    public class LocalizationViewModel : ReactiveValidatableObject
    {
        public LocalizationViewModel()
        {
            Validator = GetValidator();


            SetEnglishLanguageCommand = ReactiveCommand.Create(() =>
            {
                var culture = new CultureInfo("en-US");
                CultureInfo.CurrentUICulture = culture;
                ValidationOptions.LanguageManager.Culture = culture;
            });
            SetRussianLanguageCommand = ReactiveCommand.Create(() =>
            {
                var culture = new CultureInfo("ru-RU");
                CultureInfo.CurrentUICulture = culture;
                ValidationOptions.LanguageManager.Culture = culture;
            });
            SetGermanLanguageCommand = ReactiveCommand.Create(() =>
            {
                var culture = new CultureInfo("de-DE");
                CultureInfo.CurrentUICulture = culture;
                ValidationOptions.LanguageManager.Culture = culture;
            });
            SetCzechLanguageCommand = ReactiveCommand.Create(() =>
            {
                var culture = new CultureInfo("cs-CZ");
                CultureInfo.CurrentUICulture = culture;
                ValidationOptions.LanguageManager.Culture = culture;
            });
        }

        private IObjectValidator GetValidator()
        {
            var builder = new ValidationBuilder<LocalizationViewModel>();

            builder.RuleFor(vm => vm.PhoneNumber)
                .NotEmpty()
                    .When(vm => vm.Email, email => string.IsNullOrEmpty(email))
                    .WithLocalizedMessage(nameof(Resources.Default.PhoneNumberOrEmailRequired))
                .Matches(@"^\d{11}$")
                    .WithLocalizedMessage(nameof(Resources.Default.PhoneNumberFormat));

            builder.RuleFor(vm => vm.Email)
                .NotEmpty()
                    .When(vm => vm.PhoneNumber, phoneNumber => string.IsNullOrEmpty(phoneNumber))
                .WithLocalizedMessage(nameof(Resources.Default.PhoneNumberOrEmailRequired))
                .Must(IsValidEmail)
                    .WithLocalizedMessage(nameof(Resources.Additional), nameof(Resources.Additional.NotValidEmail));


            builder.RuleFor(vm => vm.Password)
                .NotEmpty()
                .MinLength(8, ValidationMessageType.Warning)
                    .WithLocalizedMessage(nameof(Resources.Default.SecurePassword));

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

        private static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrEmpty(email))
                return true;

            return Regex.IsMatch(email, @"^\w+@\w+\.\w+$");
        }


        [Reactive, DisplayName(DisplayNameKey = nameof(Resources.Default.PhoneNumber))]
        public string? PhoneNumber { get; set; }

        [Reactive, DisplayName(DisplayNameKey = nameof(Resources.Default.Email))]
        public string? Email { get; set; }

        [Reactive, DisplayName(DisplayNameKey = nameof(Resources.Default.Password))]
        public string? Password { get; set; }

        [Reactive, DisplayName(DisplayNameKey = nameof(Resources.Default.ConfirmPassword))]
        public string? ConfirmPassword { get; set; }

        [Reactive]
        public bool AdditionalInformation { get; set; }

        [Reactive]
        [DisplayName(DisplayNameResource = nameof(Resources.Additional), DisplayNameKey = nameof(Resources.Additional.Country))]
        public string? Country { get; set; }

        [Reactive, DisplayName(DisplayNameKey = nameof(Resources.Default.City))]
        public string? City { get; set; }


        public ICommand SetEnglishLanguageCommand { get; }

        public ICommand SetRussianLanguageCommand { get; }

        public ICommand SetGermanLanguageCommand { get; }

        public ICommand SetCzechLanguageCommand { get; }
    }
}
