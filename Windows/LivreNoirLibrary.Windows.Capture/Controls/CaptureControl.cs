using LivreNoirLibrary.Media.Capture;
using LivreNoirLibrary.Win32Api;
using System;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class CaptureControl : CaptureControlBase<Capturer>
    {
        [DependencyProperty]
        private object? _captureTarget;
        [DependencyProperty(SetterScope = Scope.Private)]
        private nint _capturingHandle;

        protected override Capturer CreateCapturer()
        {
            var capturer = new Capturer(this);
            capturer.CaptureTargetClosed += Capturer_CaptureTargetClosed;
            return capturer;
        }

        private void Capturer_CaptureTargetClosed(object? sender, EventArgs e)
        {
            CaptureTarget = null;
        }

        private void OnCaptureTargetChanged(object? value)
        {
            if (value is WindowInfo winfo)
            {
                var handle = winfo.Handle;
                if (NativeMethods.IsWindow(handle))
                {
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
    }
}
