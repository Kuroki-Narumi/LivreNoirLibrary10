using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Files;

namespace LivreNoirLibrary.Media
{
    public static partial class MediaUtils
    {
        public static bool TryGetImageSource(string? path, [MaybeNullWhen(false)] out BitmapSource bitmap)
        {
            if (FileUtils.TryGetImageFilename(path, out var path2))
            {
                bitmap = new BitmapImage(new Uri(path2, UriKind.RelativeOrAbsolute));
                return true;
            }
            bitmap = null;
            return false;
        }

        public static bool TryGetMediaUri(string? path, [MaybeNullWhen(false)] out Uri uri)
        {
            if (FileUtils.TryGetVideoFilename(path, out var path2))
            {
                uri = new Uri(path2, UriKind.RelativeOrAbsolute);
                return true;
            }
            uri = null;
            return false;
        }
    }
}
