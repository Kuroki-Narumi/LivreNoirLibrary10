using System;
using System.Windows;

namespace LivreNoirLibrary.Windows
{
    public delegate void RoutedEventHandler<T>(object sender, RoutedEventArgs<T> e);

    public class RoutedEventArgs<T> : RoutedEventArgs
    {
        public T Value { get; }

        public RoutedEventArgs(T value) : base()
        {
            Value = value;
        }

        public RoutedEventArgs(T value, RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
            Value = value;
        }
    }
}
