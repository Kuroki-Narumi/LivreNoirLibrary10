using System;
using System.Windows;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows
{
    public static partial class Events
    {
        [RoutedEvent(typeof(Control))]
        public static readonly RoutedEvent ModifiedEvent = RegisterEvent();

        public static void RaiseModifiedEvent(this Control control, bool raise = true)
        {
            if (raise)
            {
                control.RaiseEvent(new(ModifiedEvent, control));
            }
        }
    }
}
