using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using ReactiveValidation.Extensions;

namespace ReactiveValidation.Samples._1._BaseSample
{
    public class BaseSampleViewModel : ValidatableObject
    {
        public BaseSampleViewModel()
        {
            Validator = GetValidator();

            this.WhenAnyValue(vm => vm.PhoneNumber);
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

            builder.RuleFor(vm => vm.Age)
                .Between(18, 35);


            return builder.Build(this);
        }


        [Reactive]
        public string Name { get; set; }

        [Reactive]
        public string Surname { get; set; }

        [Reactive]
        public string PhoneNumber { get; set; }

        [Reactive]
        public int Age { get; set; }
    }
}
