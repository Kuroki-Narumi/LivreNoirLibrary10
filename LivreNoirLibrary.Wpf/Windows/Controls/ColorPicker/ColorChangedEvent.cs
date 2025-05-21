using System;
using System.Windows;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public delegate void ColorChangedEventHandler(object sender, ColorChangedEventArgs e);

    public class ColorChangedEventArgs(Color color, RoutedEvent r, object source) : RoutedEventArgs(r, source)
    {
        public Color Color { get; } = color;
    }
}
