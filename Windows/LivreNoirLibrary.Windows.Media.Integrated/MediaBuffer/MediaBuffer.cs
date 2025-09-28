using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using DrRect = System.Drawing.Rectangle;
using LivreNoirLibrary.Files;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Debug;

namespace LivreNoirLibrary.Windows.Media
{
    public abstract class MediaBuffer : DisposableBase
    {
        public static MediaBuffer Create(string path, in DrRect requiredRect)
        {
            ExConsole.Write($"Create MediaBuffer ({path})");
            if (File.Exists(path))
            {
                if (ExtRegs.Image.IsMatch(path))
                {
                    return new ImageBuffer(path, requiredRect);
                }
                else
                {
                    return new VideoBuffer(path, requiredRect);
                }
            }
            else
            {
                return new NullMediaBuffer();
            }
        }

        public abstract void RefreshRect(string path, in DrRect requiredRect);
        public abstract (WriteableBitmap?, Rect) GetBitmap(long ticks);
    }
}
