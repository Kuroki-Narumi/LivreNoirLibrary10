using System;
using System.IO;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Debug;

namespace LivreNoirLibrary.Windows.Media
{
    public abstract class MediaCache : DisposableBase
    {
        public static MediaCache Create(string path)
        {
            ExConsole.Write($"Create MediaBuffer from \"{path}\"");
            if (File.Exists(path))
            {
                if (ExtRegs.Image.IsMatch(path))
                {
                    return new ImageCache(path);
                }
                else
                {
                    return new VideoCache(path);
                }
            }
            else
            {
                return new NullMediaCache();
            }
        }

        public abstract BitmapSource? GetBitmap(long ticks);
    }
}
