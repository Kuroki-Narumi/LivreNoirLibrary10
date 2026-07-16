using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    /// <summary>
    /// This struct provides the functionality to periodically update the screen to prevent the window from freezing when performing high-load processing on the main thread in a WPF application.
    /// </summary>
    /// <param name="isSynchronized">Whether this instance was created on the UI thread. If false, <see cref="WaitForUpdate()"/> does nothing.</param>
    /// <param name="tickInterval">The time that elapses before a forced screen refresh occurs.</param>
    public struct AntiFreezeUpdater(bool isSynchronized, long tickInterval = AntiFreezeUpdater.DefaultInterval)
    {
        public const long DefaultInterval = TimeSpan.TicksPerSecond / 30;

        public readonly bool IsSynchronized = isSynchronized;
        public readonly long Interval = tickInterval;
        private long _t0 = Stopwatch.GetTimestamp();

        public AntiFreezeUpdater() : this(DefaultInterval) { }
        public AntiFreezeUpdater(long interval) : this(Application.Current.Dispatcher.CheckAccess(), interval) { }

        /// <inheritdoc cref="WaitForUpdate()"/>
        /// <param name="c">A <see cref="CancellationToken"/> that throws if cancellation requested.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WaitForUpdate(CancellationToken c = default)
        {
            c.ThrowIfCancellationRequested();
            long t1;
            if (IsSynchronized && (t1 = Stopwatch.GetTimestamp()) - _t0 >= Interval)
            {
                DependencyObjectExtensions.WaitForUpdate();
                _t0 = t1;
            }
        }
    }
}
