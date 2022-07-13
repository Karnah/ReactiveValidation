using System.Collections.Generic;
using System.Resources;
using System.Windows;
using ReactiveValidation.Resources.StringProviders;
using ReactiveValidation.Wpf.Samples._5._Validation_builder_factory;

namespace ReactiveValidation.Wpf.Samples
{
    public partial class App : Application
    {
        /// <inheritdoc />
        protected override void OnStartup(StartupEventArgs e)
        {
            ValidationOptions
                .Setup()

                // For sample 3.
                .UseStringProvider(new ResourceStringProvider(
                    Samples.Resources.Default.ResourceManager,
                    new Dictionary<string, ResourceManager>
                    {
                        { nameof(Samples.Resources.Additional), Samples.Resources.Additional.ResourceManager },
                    }))
                .TrackCultureChanged()
                
                // For sample 5.
                .RegisterForValidatorFactory(new ViewModelValidation())
                // Also allowed pass assembly or assemblies as parameter - all validators will be found and registered.
                //.RegisterForValidatorFactory(Assembly.GetExecutingAssembly())
                ;

            base.OnStartup(e);
        }
    }
}
