using System;
using System.Windows;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows
{
    public static partial class Events
    {
        public static readonly RoutedEvent ModifiedEvent = Register<Control, RoutedEventHandler>();

        public static void AddModifiedHandler(this DependencyObject d, RoutedEventHandler handler) => (d as IInputElement)?.AddHandler(ModifiedEvent, handler);
        public static void RemoveModifiedHandler(this DependencyObject d, RoutedEventHandler handler) => (d as IInputElement)?.RemoveHandler(ModifiedEvent, handler);

        public static void RaiseModifiedEvent(this Control control, bool raise = true)
        {
            if (raise)
            {
                control.RaiseEvent(new(ModifiedEvent, control));
            }
        }
    }
}
