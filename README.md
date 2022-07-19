# ReactiveValidation
A small validation library for WPF and Avalonia that uses a fluent interface and allow display messages near controls in GUI with MVVM
Inspired [FluentValidation](https://github.com/JeremySkinner/FluentValidation) by Jeremy Skinner

## Sample
```csharp
public class CarViewModel : ValidatableObject
{
    public CarViewModel()
    {
        Validator = GetValidator();
    }

    private IObjectValidator GetValidator()
    {
        var builder = new ValidationBuilder<CarViewModel>();

        builder.RuleFor(vm => vm.Make).NotEmpty();
        builder.RuleFor(vm => vm.Model).NotEmpty().WithMessage("Please specify a car model");
        builder.RuleFor(vm => vm.Mileage).GreaterThan(0).When(model => model.HasMileage);
        builder.RuleFor(vm => vm.Vin).Must(BeAValidVin).WithMessage("Please specify a valid VIN");
        builder.RuleFor(vm => vm.Description).Length(10, 100);

        return builder.Build(this);
    }

    private bool BeAValidVin(string vin)
    {
        // Custom vin validating logic goes here.
    }

    // Properties with realization INotifyPropertyChanged goes here.
}
```

## WPF
* [Quick start](https://github.com/Karnah/ReactiveValidation/wiki/WPF.-Quick-start)
* [Project with samples](https://github.com/Karnah/ReactiveValidation/tree/master/src/ReactiveValidation.Wpf.Samples)

## Avalonia
* [Quick start](https://github.com/Karnah/ReactiveValidation/wiki/Avalonia.-Quick-start)
* [Project with samples](https://github.com/Karnah/ReactiveValidation/tree/master/src/ReactiveValidation.Avalonia.Samples)

## Documentation
About all features you can read in the [documentation](https://github.com/Karnah/ReactiveValidation/wiki).
