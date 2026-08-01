using LivreNoirLibrary.Windows.Media;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace LivreNoirLibrary.Windows.Converters
{
    public class SelectedItemTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // values = [string? Text, int ItemsCount, TextBlock element, double limitWidth];
            if (values.Length < 4 || values[0] is not string text || values[1] is not int itemsCount || values[2] is not TextBlock element || values[3] is not double limitWidth)
            {
                return Binding.DoNothing;
            }
            var ft = MediaUtils.CreateFormattedText(text, new(element));
            if (ft.Width > limitWidth)
            {
                return $"({itemsCount} selected)";
            }
            return text;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
