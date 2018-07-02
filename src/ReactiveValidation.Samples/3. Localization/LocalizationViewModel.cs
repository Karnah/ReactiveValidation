﻿using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Input;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveValidation.Attributes;
using ReactiveValidation.Extensions;

namespace ReactiveValidation.Samples._3._Localization
{
    /// <summary>
    /// This sample also shows the use of ReactiveUI and ReactiveUI.Fody.
    /// More information: https://reactiveui.net/ and https://github.com/kswoll/ReactiveUI.Fody
    /// Pay attention to the base class - it's inherit from ReactiveObject
    /// </summary>
    public class LocalizationViewModel : ReactiveValidatableObject
    {
        public LocalizationViewModel()
        {
            //In your project this is better do in App.xaml.cs
            ValidationOptions.LanguageManager.TrackCultureChanged = true;
            ValidationOptions.LanguageManager.DefaultResourceManager = Resources.Default.ResourceManager;

            Validator = GetValidator();


            //You can change both the CurrentUICulture and ValidationOptions.LanguageManager.Culture
            SetEnglishLanguageCommand = ReactiveCommand.Create(() => {
                ValidationOptions.LanguageManager.Culture = new CultureInfo("en-US");
            });
            SetRussianLanguageCommand = ReactiveCommand.Create(() => {
                ValidationOptions.LanguageManager.Culture = new CultureInfo("ru-RU");
            });
            SetGermanLanguageCommand = ReactiveCommand.Create(() => {
                ValidationOptions.LanguageManager.Culture = new CultureInfo("de-DE");
            });

            //SetEnglishLanguageCommand = ReactiveCommand.Create(() => {
            //    CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            //    ValidationOptions.LanguageManager.OnCultureChanged();
            //});
            //SetRussianLanguageCommand = ReactiveCommand.Create(() => {
            //    CultureInfo.CurrentUICulture = new CultureInfo("ru-RU");
            //    ValidationOptions.LanguageManager.OnCultureChanged();
            //});
        }

        private IObjectValidator GetValidator()
        {
            var builder = new ValidationBuilder<LocalizationViewModel>();

            builder.RuleFor(vm => vm.PhoneNumber)
                .NotEmpty()
                    .When(vm => Email, email => string.IsNullOrEmpty(email) == true)
                    .WithLocalizedMessage(nameof(Resources.Default.PhoneNumberOrEmailRequired))
                .Matches(@"^\d{11}$")
                    .WithLocalizedMessage(nameof(Resources.Default.PhoneNumberFormat));

            builder.RuleFor(vm => vm.Email)
                .NotEmpty()
                    .When(vm => PhoneNumber, phoneNumber => string.IsNullOrEmpty(phoneNumber) == true)
                    .WithLocalizedMessage(nameof(Resources.Default.PhoneNumberOrEmailRequired))
                .Must(IsValidEmail)
                    .WithLocalizedMessage(Resources.Additional.ResourceManager, nameof(Resources.Additional.NotValidEmail));


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

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email) == true)
                return true;

            return Regex.IsMatch(email, @"^\w+@\w+.\w+$");
        }


        [Reactive, DisplayName(DisplayNameKey = nameof(Resources.Default.PhoneNumber))]
        public string PhoneNumber { get; set; }

        [Reactive, DisplayName(DisplayNameKey = nameof(Resources.Default.Email))]
        public string Email { get; set; }

        [Reactive, DisplayName(DisplayNameKey = nameof(Resources.Default.Password))]
        public string Password { get; set; }

        [Reactive, DisplayName(DisplayNameKey = nameof(Resources.Default.ConfirmPassword))]
        public string ConfirmPassword { get; set; }

        [Reactive]
        public bool AdditionalInformation { get; set; }

        [Reactive]
        [DisplayName(ResourceType = typeof(Resources.Additional), DisplayNameKey = nameof(Resources.Additional.Country))]
        public string Country { get; set; }

        [Reactive, DisplayName(DisplayNameKey = nameof(Resources.Default.City))]
        public string City { get; set; }


        public ICommand SetEnglishLanguageCommand { get; }

        public ICommand SetRussianLanguageCommand { get; }

        public ICommand SetGermanLanguageCommand { get; }
    }
}
