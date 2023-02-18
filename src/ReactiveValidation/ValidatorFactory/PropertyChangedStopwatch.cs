using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ReactiveValidation.ValidatorFactory
{
    /// <summary>
    /// Stopwatch for property changed event.
    /// </summary>
    public class PropertyChangedStopwatch
    {
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        /// <summary>
        /// Create instance of <see cref="PropertyChangedStopwatch" /> class.
        /// </summary>
        internal PropertyChangedStopwatch()
        {
        }

        /// <summary>
        /// Wait until <paramref name="dueTime" /> have passed since the last property change.
        /// </summary>
        public Task WaitUntilAsync(TimeSpan dueTime, CancellationToken cancellationToken)
        {
            var elapsed = _stopwatch.Elapsed;
            if (elapsed >= dueTime)
                return Task.CompletedTask;

            return Task.Delay(dueTime - elapsed, cancellationToken);
        }

        /// <summary>
        /// Restart stopwatch.
        /// </summary>
        internal PropertyChangedStopwatch Restart()
        {
            _stopwatch.Restart();
            return this;
        }
    }
}
