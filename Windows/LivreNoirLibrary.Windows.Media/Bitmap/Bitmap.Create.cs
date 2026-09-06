using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Media
{
    public static partial class Bitmap
    {
        public static PixelFormat PixelFormat => PixelFormats.Bgra32;
        public const double DefaultDpi = 96;

        public static WriteableBitmap Empty() => Create(1, 1);

        public static WriteableBitmap Create(int width, int height, double dpiX = DefaultDpi, double dpiY = DefaultDpi) => new(width, height, dpiX, dpiY, PixelFormat, null);

        public static RenderTargetBitmap CreateRenderTarget(int width, int height, double dpiX = DefaultDpi, double dpiY = DefaultDpi) => new(width, height, dpiX, dpiY, PixelFormats.Pbgra32);
        public static RenderTargetBitmap CreateRenderTarget(double width, double height, double dpiX = DefaultDpi, double dpiY = DefaultDpi) => new((int)width, (int)height, dpiX, dpiY, PixelFormats.Pbgra32);

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

        public static WriteableBitmap CreateFromClipboard()
        {
            if (GetSourceFromClipboard() is { } source)
            {
                return Create(source);
            }
            return Empty();
        }

        public static WriteableBitmap CreateFromUri(string uri) => CreateFromUri(new Uri(uri));

        public static WriteableBitmap CreateFromUri(Uri uri) => Create(new BitmapImage(uri));

        public static WriteableBitmap CreateFromFile(string path)
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

        public static WriteableBitmap CreateFromVisual(Visual visual, in RenderVisualOptions options = default) => Create(GetSourceFromVisual(visual, options));

        public static BitmapImage? GetSourceFromUri(Uri uri)
        {
            try
            {
                BitmapImage src = new();
                src.BeginInit();
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.CreateOptions = BitmapCreateOptions.None;
                src.UriSource = uri;
                src.EndInit();
                return src;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public static BitmapImage? GetSourceFromStream(Stream stream)
        {
            try
            {
                BitmapImage src = new();
                src.BeginInit();
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.CreateOptions = BitmapCreateOptions.None;
                src.StreamSource = stream;
                src.EndInit();
                return src;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public static BitmapImage? GetSourceFromFile(string path)
        {
            if (File.Exists(path))
            {
                using var fs = File.OpenRead(path);
                return GetSourceFromStream(fs);
            }
            return null;
        }

        public static BitmapSource? GetSourceFromClipboard()
        {
            if (Clipboard.GetData("PNG") is { } obj)
            {
                var ms = obj as MemoryStream;
                if (obj is byte[] bytes)
                {
                    ms = new(bytes);
                }
                if (ms is not null)
                {
                    return GetSourceFromStream(ms);
                }
            }
            if (Clipboard.GetImage() is BitmapSource source)
            {
                return source;
            }
            if (Clipboard.GetData("FileNameW") is string[] names)
            {
                foreach (var name in names)
                {
                    if (GetSourceFromFile(name) is { } bitmap)
                    {
                        return bitmap;
                    }
                }
            }
            return null;
        }

        private static void PrepareRender(Visual visual, in RenderVisualOptions options, out Rect bounds, out DrawingVisual dv)
        {
            if (options.WaitForUpdate)
            {
                DependencyObjectExtensions.WaitForUpdate();
            }
            dv = new();
            bounds = VisualTreeHelper.GetDescendantBounds(visual);
            if (bounds.Width is <= 0 || bounds.Height is <= 0)
            {
                return;
            }
            bounds.X = 0;
            bounds.Y = 0;
            var (sx, sy) = options.Scale;
            if (sx > 0)
            {
                bounds.Width *= sx;
            }
            if (sy > 0)
            {
                bounds.Height *= sy;
            }
            VisualBrush brush = new(visual);
            using var dc = dv.RenderOpen();
            if (options.Background is not null)
            {
                dc.DrawRectangle(options.Background, null, bounds);
            }
            dc.DrawRectangle(brush, null, bounds);
        }

        public static void RenderVisual(this RenderTargetBitmap bitmap, Visual visual, in RenderVisualOptions options = default)
        {
            PrepareRender(visual, options, out _, out var dv);
            bitmap.Clear();
            bitmap.Render(dv);
        }

        public static BitmapSource GetSourceFromVisual(Visual visual, in RenderVisualOptions options = default)
        {
            PrepareRender(visual, options, out var bounds, out var dv);
            if (bounds.Width is <= 0 || bounds.Height is <= 0)
            {
                return Empty();
            }
            var unit = Math.Max(options.SizeUnit, 1);
            var w = Math.Ceiling(bounds.Width / unit) * unit;
            var h = Math.Ceiling(bounds.Height / unit) * unit;
            var buffer = CreateRenderTarget(w, h);
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

        public static unsafe void CopyPixelsFromVisual(this RenderTargetBitmap bitmap, Visual source, Span<byte> destination, in RenderVisualOptions options = default)
        {
            PrepareRender(source, options, out var bounds, out var dv);
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
            var buffer = CreateRenderTarget(width, height);
            buffer.Render(dv);
            return buffer;
        }
    }
}
