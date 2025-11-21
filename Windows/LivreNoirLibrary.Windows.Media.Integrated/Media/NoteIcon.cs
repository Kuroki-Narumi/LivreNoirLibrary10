using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Media
{
    public static class NoteIcon
    {
        private const string ResourcePath = "pack://application:,,,/LivreNoirLibrary.Windows.Media;component/Resources/NoteIcons.png";
        private const int Size = 16;

        private static readonly Dictionary<int, CroppedBitmap> _resources = CreateResource();

        private static Dictionary<int, CroppedBitmap> CreateResource()
        {
            BitmapImage source = new(new Uri(ResourcePath));
            Dictionary<int, CroppedBitmap> result = [];
            var horz = source.PixelWidth / Size;
            var vert = source.PixelHeight / Size;
            var count = horz * vert;
            for (int i = 0; i < count; i++)
            {
                var x = (i % horz) * Size;
                var y = (i / horz) * Size;
                result.Add(i, new(source, new(x, y, Size, Size)));
            }
            return result;
        }

        public static CroppedBitmap Note_Border => _resources[0];
        public static CroppedBitmap Note_1 => _resources[1];
        public static CroppedBitmap Note_2 => _resources[2];
        public static CroppedBitmap Note_3 => _resources[3];
        public static CroppedBitmap Note_4 => _resources[4];
        public static CroppedBitmap Note_6 => _resources[5];
        public static CroppedBitmap Note_8 => _resources[6];
        public static CroppedBitmap Note_12 => _resources[7];
        public static CroppedBitmap Note_16 => _resources[8];
        public static CroppedBitmap Note_24 => _resources[9];
        public static CroppedBitmap Note_32 => _resources[10];
        public static CroppedBitmap Note_48 => _resources[11];
    }
}
