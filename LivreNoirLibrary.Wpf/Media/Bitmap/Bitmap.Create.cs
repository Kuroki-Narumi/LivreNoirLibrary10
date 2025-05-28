using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Windows;

namespace LivreNoirLibrary.Media
{
    public static partial class Bitmap
    {
        public static PixelFormat PixelFormat => PixelFormats.Pbgra32;
        public const double DefaultDpi = 96;

        public static WriteableBitmap Empty() => Create(1, 1);

        public static WriteableBitmap Create(int width, int height, double dpiX = DefaultDpi, double dpiY = DefaultDpi) => new(width, height, dpiX, dpiY, PixelFormat, null);

        public static WriteableBitmap Create(BitmapSource source)
        {
            if (source.Format != PixelFormat)
            {
                FormatConvertedBitmap dst = new(source, PixelFormat, null, 0);
                return new(dst);
            }
            else
            {
                return new(source);
            }
        }

        public static WriteableBitmap Create(BitmapSource source, int x, int y, int width, int height)
        {
            AdjustRect(source, ref x, ref y, ref width, ref height);
            return Create(new CroppedBitmap(source, new(x, y, width, height)));
        }

        public static WriteableBitmap FromClipboard()
        {
            if (Clipboard.GetImage() is BitmapSource source)
            {
                return Create(source);
            }
            return Empty();
        }

        public static WriteableBitmap FromResource(string uri) => FromResource(new Uri(uri));

        public static WriteableBitmap FromResource(Uri uri) => Create(new BitmapImage(uri));

        public static WriteableBitmap FromFile(string path)
        {
            if (GetSourceFromFile(path) is BitmapSource source)
            {
                return Create(source);
            }
            else
            {
                throw new FileNotFoundException("file not found", path);
            }
        }

        public static WriteableBitmap FromVisual(Visual visual, VisualConvertOptions? options = null) => Create(GetSourceFromVisual(visual, options));

        public static BitmapImage? GetSourceFromFile(string path)
        {
            if (File.Exists(path))
            {
                using var fs = File.OpenRead(path);
                BitmapImage src = new();
                src.BeginInit();
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.CreateOptions = BitmapCreateOptions.None;
                src.StreamSource = fs;
                src.EndInit();
                return src;
            }
            return null;
        }

        private static void PrepareRender(Visual visual, [NotNull]ref VisualConvertOptions? options, out Rect bounds, out DrawingVisual dv)
        {
            options ??= VisualConvertOptions.Default;
            if (options.WaitForUpdate)
            {
                WindowsExtensions.WaitForUpdate();
            }
            dv = new();
            bounds = VisualTreeHelper.GetDescendantBounds(visual);
            if (bounds.Width is <= 0 || bounds.Height is <= 0)
            {
                return;
            }
            bounds.X = 0;
            bounds.Y = 0;
            VisualBrush brush = new(visual);
            using var dc = dv.RenderOpen();
            if (options.Background is not null)
            {
                dc.DrawRectangle(options.Background, null, bounds);
            }
            dc.DrawRectangle(brush, null, bounds);
        }

        public static void RenderVisual(this RenderTargetBitmap bitmap, Visual visual, VisualConvertOptions? options = null)
        {
            PrepareRender(visual, ref options, out _, out var dv);
            bitmap.Clear();
            bitmap.Render(dv);
        }

        public static BitmapSource GetSourceFromVisual(Visual visual, VisualConvertOptions? options = null)
        {
            PrepareRender(visual, ref options, out var bounds, out var dv);
            if (bounds.Width is <= 0 || bounds.Height is <= 0)
            {
                return Empty();
            }
            var unit = Math.Max(options.SizeUnit, 1);
            var w = Math.Ceiling(bounds.Width / unit) * unit;
            var h = Math.Ceiling(bounds.Height / unit) * unit;
            RenderTargetBitmap buffer = new((int)w, (int)h, 96, 96, PixelFormat);
            buffer.Render(dv);
            var r = options.Rect;
            if (r.Width is <= 0 || r.Height is <= 0)
            {
                return buffer;
            }
            else
            {
                CroppedBitmap cr = new(buffer, new((int)(r.X - bounds.X), (int)(r.Y - bounds.Y), (int)Math.Ceiling(r.Width), (int)Math.Ceiling(r.Height)));
                return cr;
            }
        }

        public static unsafe void CopyPixelsFromVisual(this RenderTargetBitmap bitmap, Visual source, Span<byte> destination, VisualConvertOptions? options = null)
        {
            PrepareRender(source, ref options, out var bounds, out var dv);
            if (bounds.Width is <= 0 || bounds.Height is <= 0)
            {
                return;
            }
            bitmap.Clear();
            bitmap.Render(dv);
            Int32Rect rect;
            var r = options.Rect;
            if (r.Width is <= 0 || r.Height is <= 0)
            {
                rect = new(0, 0, bitmap.PixelWidth, bitmap.PixelHeight);
            }
            else
            {
                rect = new((int)(r.X - bounds.X), (int)(r.Y - bounds.Y), (int)Math.Ceiling(r.Width), (int)Math.Ceiling(r.Height));
            }
            var bytesPerPixel = bitmap.Format.BitsPerPixel / 8;
            var stride = rect.Width * bytesPerPixel;
            var requiredSize = stride * rect.Height;
            if (destination.Length < requiredSize)
            {
                throw new IndexOutOfRangeException($"buffer length must be >= {requiredSize} ({rect.Width}x{rect.Height}x{bytesPerPixel})");
            }
            fixed (byte* ptr = destination)
            {
                bitmap.CopyPixels(rect, (nint)ptr, destination.Length, stride);
            }
        }

        public static BitmapSource GetSourceFromDrawing(Drawing drawing, Brush? background = null, double width = double.NaN, double height = double.NaN)
        {
            var (x, y, w, h) = drawing.Bounds;
            var finite_w = double.IsFinite(width) && width is > 0;
            var finite_h = double.IsFinite(height) && height is > 0;
            if (finite_w)
            {
                if (!finite_h)
                {
                    height = width / w * h;
                }
            }
            else if (finite_h)
            {
                width = height / h * w;
            }
            else
            {
                width = w;
                height = h;
            }
            DrawingVisual dv = new();
            using (var dc = dv.RenderOpen())
            {
                if (background is not null)
                {
                    dc.DrawRectangle(background, null, new(0, 0, width, height));
                }
                Matrix m = new();
                m.Translate(-x, -y);
                m.Scale(width / w, height / h);
                MatrixTransform mt = new(m);
                dc.PushTransform(mt);
                dc.DrawDrawing(drawing);
                dc.Pop();
            }
            RenderTargetBitmap buffer = new((int)width, (int)height, 96, 96, PixelFormat);
            buffer.Render(dv);
            return buffer;
        }
    }
}
