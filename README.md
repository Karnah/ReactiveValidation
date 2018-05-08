# ReactiveValidation
A small validation library for WPF that uses a fluent interface and allow display messages near controls in GUI with MVVM
Inspired [FluentValidation](https://github.com/JeremySkinner/FluentValidation) by Jeremy Skinner

### NuGet Package
```
Install-Package ReactiveValidation
```

### Sample
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
        // custom vin validating logic goes here
    }

    //Properties with realization INotifyPropertyChanged goes here
}
```

* Merge dictionary with default ControlTemplate for validation
* Create default style(optional, but recommended) with AutoRefreshErrorTemplate and ErrorTemplate properties 
```xaml
xmlns:b="clr-namespace:ReactiveValidation.WPF.Behaviors;assembly=ReactiveValidation"
...
<ResourceDictionary>
    <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="/ReactiveValidation;component/WPF/Themes/Generic.xaml" />
    </ResourceDictionary.MergedDictionaries>

    <Style x:Key="TextBox" TargetType="TextBox">
        <Setter Property="b:ReactiveValidation.AutoRefreshErrorTemplate" Value="True" />
        <Setter Property="b:ReactiveValidation.ErrorTemplate" Value="{StaticResource ValidationErrorTemplate}" />
    </Style>
</ResourceDictionary>
```

Apply style and nothing more!
```xaml
<TextBlock Grid.Row="0" Grid.Column="0" Text="Make: " />
<TextBox Grid.Row="0" Grid.Column="1" Style="{StaticResource TextBox}"
         Text="{Binding Make, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" />

<TextBlock Grid.Row="1" Grid.Column="0" Text="Model: " />
<TextBox Grid.Row="1" Grid.Column="1" Style="{StaticResource TextBox}"
         Text="{Binding Model, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" />

<TextBlock Grid.Row="2" Grid.Column="0" Text="Has mileage: " />
<CheckBox Grid.Row="2" Grid.Column="1"
          IsChecked="{Binding HasMileage, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" />

<TextBlock Grid.Row="3" Grid.Column="0" Text="Mileage: " />
<TextBox Grid.Row="3" Grid.Column="1" Style="{StaticResource TextBox}" IsEnabled="{Binding HasMileage}"
         Text="{Binding Mileage, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" />

<TextBlock Grid.Row="4" Grid.Column="0" Text="Vin: " />
<TextBox Grid.Row="4" Grid.Column="1" Style="{StaticResource TextBox}"
         Text="{Binding Vin, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" />

<TextBlock Grid.Row="5" Grid.Column="0" Text="Description: " />
<TextBox Grid.Row="5" Grid.Column="1" Style="{StaticResource TextBox}"
         Text="{Binding Description, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" />
```

More samples you can find [here](https://github.com/Karnah/ReactiveValidation/tree/master/src/ReactiveValidation.Samples)
