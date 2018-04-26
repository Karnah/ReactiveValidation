using System;

namespace ReactiveValidation.Exceptions
{
    public class MethodAlreadyCalledException : Exception
    {
        public MethodAlreadyCalledException(string message) : base(message)
        { }
    }
}
