using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;
using Dr = System.Drawing;
using DrIm = System.Drawing.Imaging;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public static partial class Screen
    {
        public const DrIm.PixelFormat DrPF = DrIm.PixelFormat.Format32bppArgb;
        public static BitmapSource Buffer => _buffer;

        public static double Left { get; }
        public static double Top { get; }
        public static double Width { get; }
        public static double Height { get; }

        public static double Right { get; }
        public static double Bottom { get; }
        public static Rect Rect { get; }

        static Screen()
        {
            Left = SystemParameters.VirtualScreenLeft;
            Top = SystemParameters.VirtualScreenTop;
            Width = SystemParameters.VirtualScreenWidth;
            Height = SystemParameters.VirtualScreenHeight;
            Rect = new(Left, Top, Width, Height);
            Right = Left + Width;
            Bottom = Top + Height;
        }

        private static Dr.Bitmap _dr_buffer = new(1, 1, DrPF);
        private static WriteableBitmap _buffer = Bitmap.Create(1,1);

        public static void SetSize(int width, int height)
        {
            if (width != _buffer.Width || height != _buffer.Height)
            {
                _dr_buffer.Dispose();
                _dr_buffer = new(width, height, DrPF);
                _buffer = Bitmap.Create(width, height);
            }
        }
        public static void SetSize(double width, double height) => SetSize((int)Math.Ceiling(width), (int)Math.Ceiling(height));

        public static BitmapSource Snapshot(int x, int y)
        {
            using var graphics = Dr.Graphics.FromImage(_dr_buffer);
            graphics.CopyFromScreen(x, y, 0, 0, _dr_buffer.Size);
            CopyFromDrawingBitmap(_buffer, _dr_buffer);
            return _buffer;
        }

        public static BitmapSource Snapshot(double x, double y) => Snapshot((int)x, (int)y);
        public static BitmapSource Snapshot(int x, int y, int width, int height)
        {
            SetSize(width, height);
            return Snapshot(x, y);
        }
        public static BitmapSource Snapshot(double x, double y, double width, double height) => Snapshot((int)x, (int)y, (int)Math.Ceiling(width), (int)Math.Ceiling(height));
        public static BitmapSource Snapshot(Int32Rect rect) => Snapshot(rect.X, rect.Y, rect.Width, rect.Height);
        public static BitmapSource Snapshot(Rect rect) => Snapshot(rect.X, rect.Y, rect.Width, rect.Height);

        private static unsafe void CopyFromDrawingBitmap(WriteableBitmap bitmap, Dr.Bitmap source)
        {
            var w = source.Width;
            var h = source.Height;
            var data = source.LockBits(new(0, 0, w, h), DrIm.ImageLockMode.ReadOnly, DrPF);
            try
            {
                bitmap.Lock();
                SimdOperations.CopyFrom((byte*)bitmap.BackBuffer, (byte*)data.Scan0, bitmap.BackBufferStride * h);
                bitmap.AddDirtyRect(new(0, 0, w, h));
            }
            finally
            {
                bitmap.Unlock();
                source.UnlockBits(data);
            }
        }
    }
}
