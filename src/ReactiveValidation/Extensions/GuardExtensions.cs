using ReactiveValidation.Exceptions;

namespace ReactiveValidation.Extensions
{
    internal static class GuardExtensions
    {
        /// <summary>
        /// Check and throw exception if value already been assign
        /// </summary>
        public static void GuardNotCallTwice(this object o, string message)
        {
            if (o != null)
                throw new MethodAlreadyCalledException(message);
        }
    }
}
