using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public delegate void ColorClickEventHandler(object sender, ColorClickEventArgs e);

    public class ColorClickEventArgs : RoutedEventArgs
    {
        public int Index { get; init; }
        public Color Color { get; init; }
        public MouseButton MouseButton { get; init; }
        public int ClickCount { get; init; }

        public ColorClickEventArgs() : base() { }
        public ColorClickEventArgs(RoutedEvent e, object? source) : base(e, source) { }
    }

    public partial class ColorPalette
    {
        public static readonly RoutedEvent ClickEvent = Events.Register<ColorPalette, ColorClickEventHandler>();

        public event ColorClickEventHandler Click { add => AddHandler(ClickEvent, value); remove => RemoveHandler(ClickEvent, value); }

        private void RaiseClick(int index, Color color, MouseButton button, int clickCount)
        {
            RaiseEvent(new ColorClickEventArgs(ClickEvent, this)
            {
                Index = index,
                Color = color,
                MouseButton = button,
                ClickCount = clickCount,
            });
        }
    }
}
