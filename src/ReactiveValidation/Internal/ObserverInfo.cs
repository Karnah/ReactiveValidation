using System;

namespace ReactiveValidation.Internal
{
    internal class ObserverInfo
    {
        private readonly Func<Type, Type, bool> _canObserve;
        private readonly Func<object, object, Action, IDisposable> _createObserver;

        public ObserverInfo(Func<Type, Type, bool> canObserve, Func<object, object, Action, IDisposable> createObserver)
        {
            _canObserve = canObserve ?? throw new ArgumentNullException(nameof(canObserve));
            _createObserver = createObserver ?? throw new ArgumentNullException(nameof(createObserver));
        }


        public bool CanObserve(Type instanceType, Type propertyType)
        {
            return _canObserve.Invoke(instanceType, propertyType);
        }

        public bool CanObserve<TObject, TProp>()
            where TObject : IValidatableObject
        {
            return CanObserve(typeof(TObject), typeof(TProp));
        }


        public IDisposable CreateObserver<TObject, TProp>(TObject instance, TProp property, Action action)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            return _createObserver.Invoke(instance, property, action);
        }
    }
}
