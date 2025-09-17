using System;
using System.Globalization;
using System.Windows.Data;

namespace LivreNoirLibrary.Windows.Controls
{
    public class TimeItemConverter : IValueConverter
    {
        public TimeItemType Type { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => TimeItem.GetItem(Type, (int)value);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (value as TimeItem)!.Value;
    }
}
