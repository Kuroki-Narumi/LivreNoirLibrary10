using System;
using System.Diagnostics;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    /// <summary>
    /// This struct provides the functionality to periodically update the screen to prevent the window from freezing when performing high-load processing on the main thread in a WPF application.
    /// </summary>
    /// <param name="isSynchronized">Whether this instance was created on the UI thread. If false, <see cref="WaitForUpdate()"/> does nothing.</param>
    /// <param name="interval">The time that elapses before a forced screen refresh occurs.</param>
    public struct AntiFreezeUpdater(bool isSynchronized, long interval = AntiFreezeUpdater.DefaultInterval)
    {
        public const long DefaultInterval = TimeSpan.TicksPerSecond / 30;

        public readonly bool IsSynchronized = isSynchronized;
        public readonly long Interval = interval;
        private long _t0 = Stopwatch.GetTimestamp();

        public AntiFreezeUpdater() : this(DefaultInterval) { }
        public AntiFreezeUpdater(long interval) : this(Application.Current.Dispatcher.CheckAccess(), interval) { }

        /// <summary>
        /// Forces the screen update if the specified interval has passed since tha last call of <see cref="WaitForUpdate()"/>.
        /// </summary>
        /// <remarks>This method only performs an update if the instance is on the UI thread and the
        /// required interval has passed since the last update. If these conditions are not met, the method returns
        /// immediately without performing any action.</remarks>
        public void WaitForUpdate()
        {
            long t1;
            if (IsSynchronized && (t1 = Stopwatch.GetTimestamp()) - _t0 >= Interval)
            {
                DependencyObjectExtensions.WaitForUpdate();
                _t0 = t1;
            }
        }
    }
}
