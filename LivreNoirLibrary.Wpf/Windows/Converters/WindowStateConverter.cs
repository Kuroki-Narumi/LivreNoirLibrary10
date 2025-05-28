﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LivreNoirLibrary.Windows.Converters
{
    public class WindowStateConverter : IValueConverter
    {
        public WindowState State { get; set; }

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(value, State);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
    }
}
