using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class ColorSelector
    {
        public const int CanvasWidth = 256;
        public const int CanvasHeight = 256;

        public const int PixelSize = CanvasWidth * CanvasHeight;
        public const int Stride = CanvasWidth * 4;
        public const int BufferSize = Stride * CanvasHeight;
        public static readonly Int32Rect BitmapRect = new(0, 0, CanvasWidth, CanvasHeight);

        private static WriteableBitmap CreateBitmap() => new(CanvasWidth, CanvasHeight, 96, 96, PixelFormats.Pbgra32, null);

        private readonly WriteableBitmap _src_r = CreateBitmap();
        private readonly WriteableBitmap _src_g = CreateBitmap();
        private readonly WriteableBitmap _src_b = CreateBitmap();

        public WriteableBitmap Palette_R => _src_r;
        public WriteableBitmap Palette_G => _src_g;
        public WriteableBitmap Palette_B => _src_b;

        private void LockPalettes()
        {
            _src_r.Lock();
            _src_g.Lock();
            _src_b.Lock();
        }

        private void UnlockPalettes()
        {
            _src_r.AddDirtyRect(BitmapRect);
            _src_g.AddDirtyRect(BitmapRect);
            _src_b.AddDirtyRect(BitmapRect);
            _src_r.Unlock();
            _src_g.Unlock();
            _src_b.Unlock();
        }

        private unsafe void InitializePalettes()
        {
            LockPalettes();
            var ptr_r = (byte*)_src_r.BackBuffer;
            var ptr_g = (byte*)_src_g.BackBuffer;
            var ptr_b = (byte*)_src_b.BackBuffer;
            for (var y = 0; y is < CanvasHeight; y++)
            {
                var cy = (byte)(255 - y);
                for (var x = 0; x is < CanvasWidth; x++, ptr_r += 4, ptr_g += 4, ptr_b += 4)
                {
                    ptr_r[1] = ptr_g[2] = ptr_b[2] = (byte)x;
                    ptr_r[0] = ptr_g[0] = ptr_b[1] = cy;
                    ptr_r[3] = ptr_g[3] = ptr_b[3] = 255; // alpha
                }
            }
            UnlockPalettes();
        }

        private unsafe void UpdatePalettes()
        {
            var (r, g, b) = _color_info.GetBytes();
            LockPalettes();
            var span_r = new Span<uint>((uint*)_src_r.BackBuffer, PixelSize);
            var span_g = new Span<uint>((uint*)_src_g.BackBuffer, PixelSize);
            var span_b = new Span<uint>((uint*)_src_b.BackBuffer, PixelSize);
            span_r.And(0xff00ffff);
            span_g.And(0xffff00ff);
            span_b.And(0xffffff00);
            span_r.Or((uint)r << 16);
            span_g.Or((uint)g << 8);
            span_b.Or((uint)b);
            UnlockPalettes();
        }
    }
}
