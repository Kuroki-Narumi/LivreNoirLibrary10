using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.Windows.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Xps.Serialization;

namespace LivreNoirLibrary.Windows.YuGiOh.Converters
{
    public class IdToCardImageConverter : IValueConverter
    {
        public static string ResourceDirectory { get; set; } = Path.Join(General.GetAssemblyDir(), "Resources");

        public const string CardImageDirectory = @"CardImages";

        public const string DefultImagePath = "/LivreNoirLibrary.Windows.YuGiOh;component/Resources/dummy_card.png";

        public static BitmapImage DefaultImage { get; } = new(new Uri(DefultImagePath, UriKind.Relative));

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case int id:
                    return GetImage(id);
                case ICard card:
                    return GetImage(card.ThisCard.Id);
                case IId iid:
                    return GetImage(iid.Id);
            }
            if (NumberExtensions.TryGetInt(value, out var i))
            {
                return GetImage(i);
            }
            return null;
        }

        public static string GetImagePath(int id) => Path.Join(ResourceDirectory, CardImageDirectory, $"{id}.png");

        public static bool ImageExists(int id) => FileUtils.TryGetImageFileName(GetImagePath(id), out _);

        public static BitmapImage? GetImage(int id)
        {
            var path = GetImagePath(id);
            if (FileUtils.TryGetImageFileName(path, out var actualPath))
            {
                return Bitmap.GetSourceFromFile(actualPath);
            }
            return DefaultImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
