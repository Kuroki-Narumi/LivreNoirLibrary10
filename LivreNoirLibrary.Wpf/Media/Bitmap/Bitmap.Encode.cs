using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Files;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media
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

        public static bool SaveImage(this Visual visual, string path, VisualConvertOptions? options = null, BitmapEncodeType encoder = BitmapEncodeType.PNG)
        {
            return SaveImage(GetSourceFromVisual(visual, options), path, encoder);
        }

        public static void ToClipboard(this BitmapSource bitmap)
        {
            Clipboard.SetImage(bitmap);
        }

        public static void ToClipboard(string path)
        {
            if (GetSourceFromFile(path) is BitmapSource source)
            {
                Clipboard.SetImage(source);
            }
        }

        public static void ToClipboard(this Visual visual, VisualConvertOptions? options = null)
        {
            Clipboard.SetImage(GetSourceFromVisual(visual, options));
        }
    }
}
