using System;
using System.Windows;
using System.Diagnostics;

namespace LivreNoirLibrary.Windows
{
    public class FpsCounter(int bufferSize = 30) : DependencyObject
    {
        private static readonly DependencyPropertyKey FpsPropertyKey = PropertyUtils.RegisterAttachedReadOnly(typeof(FpsCounter), 0d);
        public static readonly DependencyProperty FpsProperty = FpsPropertyKey.DependencyProperty;

        public static double GetFps(DependencyObject obj) => (double)obj.GetValue(FpsProperty);
        public static void SetFps(DependencyObject obj, double value) => obj.SetValue(FpsPropertyKey, value);

        private readonly int _buffer_size = bufferSize;
        private readonly long[] _buffer = new long[bufferSize];
        private int _index;
        private long _total;
        private long _t0;
        private readonly double _ticks_per_buffer = bufferSize * TimeSpan.TicksPerSecond;

        public void StartCount()
        {
            _t0 = Stopwatch.GetTimestamp();
        }

        public void FinishCount(Action<double> callback)
        {
            if (FinishCountCore(out var fps))
            {
                callback(fps);
            }
        }

        public void FinishCount(DependencyObject d)
        {
            if (FinishCountCore(out var fps))
            {
                SetFps(d, fps);
            }
        }

        private bool FinishCountCore(out double fps)
        {
            var t = Stopwatch.GetTimestamp() - _t0;
            var buffer = _buffer;
            var index = _index;
            var total = _total;
            try
            {
                total -= buffer[index];
                total += t;
                buffer[index] = t;
                index++;
                if (index >= _buffer_size)
                {
                    fps = _ticks_per_buffer / total;
                    index = 0;
                    return true;
                }
                fps = 0;
                return false;
            }
            finally
            {
                _index = index;
                _total = total;
            }
        }
    }
}
