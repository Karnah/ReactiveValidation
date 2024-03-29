﻿using ReactiveUI.Fody.Helpers;

namespace ReactiveValidation.Avalonia.Samples._5._Validation_builder_factory
{
    /// <summary>
    /// Validating by <see cref="ViewModelValidation" />.
    /// 
    /// Please see how setup ValidatorFactory in <see cref="Program.BuildAvaloniaApp" />.
    /// </summary>
    public class ValidationBuilderFactoryViewModel : ReactiveValidatableObject
    {
        /// <inheritdoc />
        public ValidationBuilderFactoryViewModel()
        {
            // For your project a better idea pass IValidatorFactory by constructor parameter using DI.
            // If this is possible you can setup your IoC in order to it create ViewModel, and after this set Validator using IValidatorFactory.
            Validator = ValidationOptions.ValidatorFactory.GetValidator(this);
            
            // If you don't want to use factory, you can create like this:
            //Validator = new ViewModelValidation().Build(this);
        }

        [Reactive]
        public string? PhoneNumber { get; set; }

        [Reactive]
        public string? Email { get; set; }

        [Reactive]
        public string? Password { get; set; }

        [Reactive]
        public string? ConfirmPassword { get; set; }

        [Reactive]
        public bool AdditionalInformation { get; set; }

        [Reactive]
        public string? Country { get; set; }

        [Reactive]
        public string? City { get; set; }
    }
}
