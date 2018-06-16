using System;

namespace ReactiveValidation
{
    public class ExceptionSource : IStringSource
    {
        private readonly Exception _exception;

        public ExceptionSource(Exception exception)
        {
            _exception = exception;
        }


        public string GetString()
        {
            return _exception.Message;
        }
    }
}
