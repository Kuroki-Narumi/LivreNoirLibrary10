using System;
using System.Globalization;
using System.Windows.Data;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    internal class ExpanderStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool flag && flag)
            {
                return CardInfoView.ExpandedPackListHeight;
            }
            return CardInfoView.DefaultPackListHeight;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
