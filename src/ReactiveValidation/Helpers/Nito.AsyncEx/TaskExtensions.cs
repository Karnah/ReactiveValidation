#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// Original code from StephenCleary: https://github.com/StephenCleary/AsyncEx

namespace ReactiveValidation.Helpers.Nito.AsyncEx
{
    /// <summary>
    /// Provides synchronous extension methods for tasks.
    /// </summary>
    internal static class TaskExtensions
    {
        /// <summary>
        /// Waits for the task to complete, unwrapping any exceptions.
        /// </summary>
        /// <param name="task">The task. May not be <c>null</c>.</param>
        public static void WaitAndUnwrapException(this Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Waits for the task to complete, unwrapping any exceptions.
        /// </summary>
        /// <param name="task">The task. May not be <c>null</c>.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <exception cref="OperationCanceledException">The <paramref name="cancellationToken"/> was cancelled before the <paramref name="task"/> completed, or the <paramref name="task"/> raised an <see cref="OperationCanceledException"/>.</exception>
        public static void WaitAndUnwrapException(this Task task, CancellationToken cancellationToken)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            try
            {
                task.Wait(cancellationToken);
            }
            catch (AggregateException ex)
            {
                throw ExceptionHelpers.PrepareForRethrow(ex.InnerException);
            }
        }

        /// <summary>
        /// Waits for the task to complete, unwrapping any exceptions.
        /// </summary>
        /// <typeparam name="TResult">The type of the result of the task.</typeparam>
        /// <param name="task">The task. May not be <c>null</c>.</param>
        /// <returns>The result of the task.</returns>
        public static TResult WaitAndUnwrapException<TResult>(this Task<TResult> task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            return task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Asynchronously waits for the task to complete, or for the cancellation token to be canceled.
        /// </summary>
        /// <param name="this">The task to wait for. May not be <c>null</c>.</param>
        /// <param name="cancellationToken">The cancellation token that cancels the wait.</param>
        public static Task WaitAsync(this Task @this, CancellationToken cancellationToken)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            if (!cancellationToken.CanBeCanceled)
                return @this;
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled(cancellationToken);
            return DoWaitAsync(@this, cancellationToken);
        }

        private static async Task DoWaitAsync(Task task, CancellationToken cancellationToken)
        {
            using (var cancelTaskSource = new CancellationTokenTaskSource<object>(cancellationToken))
                await (await Task.WhenAny(task, cancelTaskSource.Task).ConfigureAwait(false)).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously waits for the task to complete, or for the cancellation token to be canceled.
        /// </summary>
        /// <typeparam name="TResult">The type of the task result.</typeparam>
        /// <param name="this">The task to wait for. May not be <c>null</c>.</param>
        /// <param name="cancellationToken">The cancellation token that cancels the wait.</param>
        public static Task<TResult> WaitAsync<TResult>(this Task<TResult> @this, CancellationToken cancellationToken)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            if (!cancellationToken.CanBeCanceled)
                return @this;
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled<TResult>(cancellationToken);
            return DoWaitAsync(@this, cancellationToken);
        }

        private static async Task<TResult> DoWaitAsync<TResult>(Task<TResult> task, CancellationToken cancellationToken)
        {
            using (var cancelTaskSource = new CancellationTokenTaskSource<TResult>(cancellationToken))
                return await (await Task.WhenAny(task, cancelTaskSource.Task).ConfigureAwait(false)).ConfigureAwait(false);
        }

    }
}