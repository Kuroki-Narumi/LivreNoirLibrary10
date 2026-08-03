using LivreNoirLibrary.Win32Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LivreNoirLibrary.Media.Capture
{
    public delegate bool WindowSelector(WindowInfo info);

    public class WindowCapturer : ICapturer
    {
        public event EventHandler? CaptureTargetClosed { add => _capturer.CaptureTargetClosed += value; remove => _capturer.CaptureTargetClosed -= value; }

        public static TimeSpan DefaultSearchInterval { get; } = TimeSpan.FromMilliseconds(500);

        private readonly Capturer _capturer;
        private readonly SynchronizationContext? _syncContext = SynchronizationContext.Current;

        private Timer? _timer;

        public IImageAdapter Adapter { get => _capturer.Adapter; set => _capturer.Adapter = value; }

        public bool IsCursorCaptureEnabled { get => _capturer.IsCursorCaptureEnabled; set => _capturer.IsCursorCaptureEnabled = value; }

        public bool IsActive => _capturer.IsActive;

        public nint CapturingWindowHandle => _capturer.CapturingWindowHandle;
        public nint CapturingMonitorHandle => _capturer.CapturingMonitorHandle;

        /// <summary>
        /// キャプチャ対象のウィンドウであるかどうかを判別するためのデリゲート。
        /// </summary>
        public WindowSelector? Selector
        {
            get;
            set
            {
                _capturer.Stop();
                field = value;
                RefreshTimer();
            }
        }

        /// <summary>
        /// キャプチャ対象が存在しない場合に探す間隔。
        /// </summary>
        public TimeSpan SearchInterval
        {
            get;
            set
            {
                field = value;
                RefreshTimer();
            }
        } = DefaultSearchInterval;

        public WindowCapturer(IImageAdapter adapter)
        {
            _capturer = new(adapter);
            _capturer.CaptureTargetClosed += OnCaptureTargetClosed;
        }

        private void StopTimer()
        {
            _timer?.Dispose();
            _timer = null;
        }

        private void RefreshTimer()
        {
            StopTimer();
            if (!_capturer.IsActive && Selector is { } selector)
            {
                OnTick(null);
                var interval = SearchInterval;
                if (!_capturer.IsActive && interval.Ticks > 0)
                {
                    _timer = new(OnTick, null, default, interval);
                }
            }
        }

        private void OnTick(object? state)
        {
            if (Selector is not null)
            {
                NativeMethods.EnumerateWindowInfo(CheckWindowInfo);
            }
        }

        private bool CheckWindowInfo(WindowInfo info)
        {
            if (Selector!(info))
            {
                if (_syncContext is { } ctx)
                {
                    ctx.Post(StartCapture, info.Handle);
                }
                else
                {
                    StartCapture(info.Handle);
                }
                return false;
            }
            return true;
        }

        private void StartCapture(object? state)
        {
            if (state is nint handle)
            {
                _capturer.StartWindowCapture(handle);
            }
            if (_capturer.IsActive)
            {
                StopTimer();
            }
        }

        private void OnCaptureTargetClosed(object? sender, EventArgs e)
        {
            RefreshTimer();
        }
    }
}
