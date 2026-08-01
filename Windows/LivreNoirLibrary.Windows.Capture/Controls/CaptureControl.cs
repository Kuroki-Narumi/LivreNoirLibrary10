using LivreNoirLibrary.Media.Capture;
using LivreNoirLibrary.Win32Api;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class CaptureControl : CroppedImage
    {
        private readonly DispatcherTimer _timer;
        private readonly BitmapAdapter _adapter;
        private readonly Capturer<WriteableBitmap> _capturer;
        private WriteableBitmap? _captureSource;

        [DependencyProperty]
        private object? _captureTarget;
        [DependencyProperty]
        private bool _isCursorCaptureEnabled;
        [DependencyProperty]
        private bool _captureClientArea;
        [DependencyProperty(SetterScope = Scope.Private)]
        private nint _capturingHandle;

        private bool _isWindow;

        public CaptureControl()
        {
            _adapter = new();
            _capturer = new(_adapter);
            _timer = new(TimeSpan.FromMilliseconds(10), DispatcherPriority.Normal, OnTick, Dispatcher);
            _timer.Start();
        }

        private void OnCaptureTargetChanged(object? value)
        {
            _isWindow = false;
            if (value is WindowInfo winfo)
            {
                var handle = winfo.Handle;
                if (NativeMethods.IsWindow(handle))
                {
                    _isWindow = true;
                    _capturer.StartWindowCapture(handle);
                    CapturingHandle = handle;
                    return;
                }
            }
            else if (value is MonitorInfo minfo)
            {
                var handle = minfo.Handle;
                _capturer.StartMonitorCapture(handle);
                CapturingHandle = handle;
                return;
            }
            _capturer.Stop();
            CapturingHandle = 0;
        }

        private void OnIsCursorCaptureEnabledChanged(bool value)
        {
            _capturer.IsCursorCaptureEnabled = value;
        }

        private void OnTick(object? sender, EventArgs e)
        {
            var capturer = _capturer;
            if (!capturer.IsActive)
            {
                return;
            }

            var bitmap = _captureSource;
            var newBitmap = capturer.GetFrame(bitmap, out var width, out var height);
            Source = newBitmap;
            if (_isWindow && CaptureClientArea && NativeMethods.TryGetWindowRect(CapturingHandle, out _, out var rect))
            {
            }
            else
            {
                rect = new(0, 0, width, height);
            }
            var r32 = rect.ToInt32Rect();
            if (r32 != SourceRect)
            {
                SourceRect = r32;
            }
            _captureSource = newBitmap;
        }
    }
}
