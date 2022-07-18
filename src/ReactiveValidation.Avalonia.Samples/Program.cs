using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.Collections.Generic;
using System.Resources;
using ReactiveValidation.Avalonia.Samples._5._Validation_builder_factory;
using ReactiveValidation.Resources.StringProviders;

namespace ReactiveValidation.Avalonia.Samples
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI()
                .UseReactiveValidation(op => op
                        // For sample 3.
                        .UseStringProvider(new ResourceStringProvider(
                            Resources.Default.ResourceManager,
                            new Dictionary<string, ResourceManager>
                            {
                                { nameof(Resources.Additional), Resources.Additional.ResourceManager },
                            }))
                        .TrackCultureChanged()

                        // For sample 5.
                        .RegisterForValidatorFactory(new ViewModelValidation())
                        // Also allowed pass assembly or assemblies as parameter - all validators will be found and registered.
                        //.RegisterForValidatorFactory(Assembly.GetExecutingAssembly()));
                );

    }
}