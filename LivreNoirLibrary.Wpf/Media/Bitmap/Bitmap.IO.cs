using System;
using System.Windows.Media.Imaging;
using System.IO;

namespace LivreNoirLibrary.IO
{
    public static partial class BitmapIOExtensions
    {
        public static BitmapSource ReadBitmap(this Stream stream)
        {
            BitmapImage bitmap = new();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.CreateOptions = BitmapCreateOptions.None;
            bitmap.StreamSource = stream;
            bitmap.EndInit();
            return bitmap;
        }

        public static void Write(this Stream stream, BitmapSource bitmap, bool withSize = true)
        {
            // initial position
            var size_pos = stream.Position;
            if (withSize)
            {
                // dummy size header
                stream.Write(BitConverter.GetBytes(0L));
            }
            // beginning of data
            var begin_pos = stream.Position;
            // write bitmap bytes
            PngBitmapEncoder encoder = new();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(stream);
            // end of data
            var end_pos = stream.Position;
            if (withSize)
            {
                // write correct data size
                stream.Position = size_pos;
                stream.Write(BitConverter.GetBytes(end_pos - begin_pos));
                // restore position
                stream.Position = end_pos;
            }
        }
    }
}
