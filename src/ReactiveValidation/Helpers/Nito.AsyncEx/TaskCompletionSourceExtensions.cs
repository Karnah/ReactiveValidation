using System.Threading.Tasks;

// Original code from StephenCleary: https://github.com/StephenCleary/AsyncEx

namespace ReactiveValidation.Helpers.Nito.AsyncEx
{
    /// <summary>
    /// Provides extension methods for <see cref="TaskCompletionSource{TResult}"/>.
    /// </summary>
    internal static class TaskCompletionSourceExtensions
    {
        /// <summary>
        /// Creates a new TCS for use with async code, and which forces its continuations to execute asynchronously.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of the TCS.</typeparam>
        public static TaskCompletionSource<TResult> CreateAsyncTaskSource<TResult>()
        {
            return new TaskCompletionSource<TResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        }
    }
}
