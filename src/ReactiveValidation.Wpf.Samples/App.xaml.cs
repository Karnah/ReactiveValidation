using System;
using System.Collections.Generic;
using System.Resources;
using System.Windows;

using ReactiveUI.Legacy;
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

                // For sample 4.
                //.AddCollectionObserver(CanObserve, CreateReactiveCollectionItemChangedObserver)

                // For sample 5.
                .RegisterForValidatorFactory(new ViewModelValidation())
                // Also allowed pass assembly or assemblies as parameter - all validators will found and registered.
                //.RegisterForValidatorFactory(Assembly.GetExecutingAssembly())
                ;

            base.OnStartup(e);
        }

#pragma warning disable 618
        private static bool CanObserve(Type objectType, Type propertyType)
        {
            return typeof(IReactiveNotifyCollectionItemChanged<object>).IsAssignableFrom(propertyType);
        }

        private static IDisposable CreateReactiveCollectionItemChangedObserver(object o, object propertyValue, Action action)
        {
            if (propertyValue is IReactiveNotifyCollectionItemChanged<object> collection)
                return collection.ItemChanged.Subscribe(args => action());

            return null;
        }
#pragma warning restore 618
    }
}
