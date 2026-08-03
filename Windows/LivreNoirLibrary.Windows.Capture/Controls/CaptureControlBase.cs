using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media.Capture;
using LivreNoirLibrary.Win32Api;
using LivreNoirLibrary.Windows.Media;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Controls
{
    public abstract partial class CaptureControlBase : CroppedImage, IImageAdapter
    {
        [DependencyProperty]
        private bool _isCursorCaptureEnabled;
        [DependencyProperty]
        private bool _captureClientArea;

        public abstract nint CapturingWindowHandle { get; }

        protected virtual void OnIsCursorCaptureEnabledChanged(bool value) { }

        public unsafe void WritePixels(byte* srcPtr, int width, int height, int srcStride)
        {
            if (Source is not WriteableBitmap bitmap || bitmap.PixelWidth < width || bitmap.PixelHeight < height)
            {
                bitmap = new(width, height, 96, 96, PixelFormats.Pbgra32, null);
                Source = bitmap;
            }
            using var b = bitmap.BeginWrite();
            var dstPtr = (uint*)b.Pointer;
            var nwidth = (nuint)width;
            var dstStride = b.Width;
            for (var y = 0; y < height; y++)
            {
                SimdOperations.CopyFrom(dstPtr, (uint*)srcPtr, nwidth);
                srcPtr += srcStride;
                dstPtr += dstStride;
            }
            var handle = CapturingWindowHandle;
            if (handle is not 0 && CaptureClientArea && NativeMethods.TryGetWindowRect(handle, out _, out var rect))
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
        }
    }

    public abstract partial class CaptureControlBase<T> : CaptureControlBase
        where T : ICapturer
    {
        protected readonly T _capturer;

        protected abstract T CreateCapturer();

        public sealed override nint CapturingWindowHandle => _capturer.CapturingWindowHandle;

        public CaptureControlBase()
        {
            _capturer = CreateCapturer();
        }

        protected override void OnIsCursorCaptureEnabledChanged(bool value)
        {
            _capturer.IsCursorCaptureEnabled = value;
        }
    }
}
