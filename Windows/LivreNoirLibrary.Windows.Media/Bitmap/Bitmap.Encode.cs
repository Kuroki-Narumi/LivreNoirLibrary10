using LivreNoirLibrary.IO;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Media
{
    public static partial class Bitmap
    {
        public static bool SaveImage(this BitmapSource source, string path, BitmapEncodeType encoder = BitmapEncodeType.PNG)
        {
            BitmapEncoder? e = null;
            switch (encoder)
            {
                case BitmapEncodeType.Auto:
                    e = GetEncoder(path);
                    break;
                case BitmapEncodeType.PNG:
                    e = new PngBitmapEncoder();
                    if (!ExtRegs.Png.IsMatch(path)) { path += $".{Exts.Png}"; }
                    break;
                case BitmapEncodeType.BMP:
                    e = new BmpBitmapEncoder();
                    if (!ExtRegs.Bmp.IsMatch(path)) { path += $".{Exts.Bmp}"; }
                    break;
                case BitmapEncodeType.GIF:
                    e = new GifBitmapEncoder();
                    if (!ExtRegs.Gif.IsMatch(path)) { path += $".{Exts.Gif}"; }
                    break;
                case BitmapEncodeType.TIFF:
                    e = new TiffBitmapEncoder();
                    if (!ExtRegs.Tiff.IsMatch(path)) { path += $".{Exts.Tif}"; }
                    break;
            }
            if (e is null)
            {
                return false;
            }
            e.Frames.Add(BitmapFrame.Create(source));
            using var fs = General.CreateSafe(path);
            e.Save(fs);
            return true;
        }

        public static void SaveByDialog(this BitmapSource bitmap)
        {
            Microsoft.Win32.SaveFileDialog dialog = new()
            {
                Filter = Filters.Join(Filters.Png, Filters.Bmp, Filters.Gif, Filters.Tiff),
                OverwritePrompt = true
            };
            if (dialog.ShowDialog() is true)
            {
                SaveImage(bitmap, dialog.FileName, BitmapEncodeType.Auto);
            }
        }

        private static BitmapEncoder? GetEncoder(string path)
        {
            if (ExtRegs.Png.IsMatch(path))
            {
                return new PngBitmapEncoder();
            }
            if (ExtRegs.Bmp.IsMatch(path))
            {
                return new BmpBitmapEncoder();
            }
            if (ExtRegs.Gif.IsMatch(path))
            {
                return new GifBitmapEncoder();
            }
            if (ExtRegs.Tiff.IsMatch(path))
            {
                return new TiffBitmapEncoder();
            }
            return null;
        }

        public static bool SaveImage(this Visual visual, string path, in RenderVisualOptions options = default, BitmapEncodeType encoder = BitmapEncodeType.PNG)
        {
            return SaveImage(GetSourceFromVisual(visual, options), path, encoder);
        }

        private static readonly Lock _clipboard_lock = new();
        private static readonly PngBitmapEncoder _clipboard_encoder = new();
        private static readonly MemoryStream _clipboard = new(32768);

        private static void ToClipboardImpl(BitmapSource bitmap)
        {
            lock (_clipboard_lock)
            {
                Clipboard.SetImage(bitmap);
                DataObject obj = new();
                var encoder = _clipboard_encoder;
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                _clipboard.SetLength(0);
                encoder.Save(_clipboard);
                _clipboard.Position = 0;
                encoder.Frames.Clear();
                obj.SetData("PNG", _clipboard, false);
                AddData(obj, DataFormats.Bitmap);
                Clipboard.SetDataObject(obj);
            }
        }

        private static void AddData(DataObject obj, string format)
        {
            if (Clipboard.ContainsData(format))
            {
                obj.SetData(format, Clipboard.GetData(format));
            }
        }

        public static void ToClipboard(this BitmapSource bitmap) => ToClipboardImpl(bitmap);

        public static void ToClipboard(string path)
        {
            if (GetSourceFromFile(path) is BitmapSource source)
            {
                ToClipboardImpl(source);
            }
        }

        public static void ToClipboard(this Visual visual, in RenderVisualOptions options = default) => ToClipboardImpl(GetSourceFromVisual(visual, options));
    }
}
