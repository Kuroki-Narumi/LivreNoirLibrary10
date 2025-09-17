using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Controls
{
    public class TitlebarIconConverter : IValueConverter
    {
        public bool SmallIcon { get; set; } = true;

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not null)
            {
                if (value is BitmapFrame frame)
                {
                    var (w, h) = WindowsExtensions.GetSystemIconSize(SmallIcon);
                    var frames = frame.Decoder.Frames;
                    return GetBestMatch(frames, w, h);
                }
                return value;
            }
            return WindowsExtensions.GetApplicationIcon(SmallIcon);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;

        // cf: https://github.com/dotnet/wpf/blob/e4779d8e8d179c8181b51ca8c40a31daeacb7271/src/Microsoft.DotNet.Wpf/src/PresentationFramework/MS/Internal/AppModel/IconHelper.cs
        private static BitmapFrame GetBestMatch(ReadOnlyCollection<BitmapFrame> frames, int width, int height)
        {
            var bestScore = int.MaxValue;
            var bestBpp = 0;
            var bestIndex = 0;
            var isBitmapIconDecoder = frames[0].Decoder is IconBitmapDecoder;

            for (int i = 0; i < frames.Count && bestScore != 0; ++i)
            {
                var currentIconBitDepth = isBitmapIconDecoder ? frames[i].Thumbnail.Format.BitsPerPixel : frames[i].Format.BitsPerPixel;
                if (currentIconBitDepth is 0)
                {
                    currentIconBitDepth = 8;
                }
                int score = MatchImage(frames[i], width, height, currentIconBitDepth);
                if (score < bestScore)
                {
                    bestIndex = i;
                    bestBpp = currentIconBitDepth;
                    bestScore = score;
                }
                else if (score == bestScore)
                {
                    if (bestBpp < currentIconBitDepth)
                    {
                        bestIndex = i;
                        bestBpp = currentIconBitDepth;
                    }
                }
            }

            return frames[bestIndex];
        }

        private static int MatchImage(BitmapFrame frame, int width, int height, int bpp)
        {
            return 2 * MyAbs(bpp, 32, false) +
                   MyAbs(frame.PixelWidth, width, true) +
                   MyAbs(frame.PixelHeight, height, true);
        }

        private static int MyAbs(int valueHave, int valueWant, bool fPunish)
        {
            var diff = valueHave - valueWant;

            if (diff < 0)
            {
                diff = (fPunish ? -2 : -1) * diff;
            }

            return diff;
        }
    }
}
