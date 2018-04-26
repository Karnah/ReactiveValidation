namespace ReactiveValidation
{
    public class StaticStringSource : IStringSource
    {
        private readonly string _message;

        public StaticStringSource(string message)
        {
            _message = message;
        }


        public string GetString()
        {
            return _message;
        }
    }
}
