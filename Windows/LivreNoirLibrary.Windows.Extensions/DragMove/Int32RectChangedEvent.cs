using System;
using System.Windows;

namespace LivreNoirLibrary.Windows
{
    public delegate void Int32RectChangedEventHandler(object? sender, Int32RectChangedEventArgs e);

    public class Int32RectChangedEventArgs : EventArgs
    {
        public Int32Rect OldValue { get; init; }
        public Int32Rect NewValue { get; init; }

        public Int32RectChangedEventArgs() { }
        public Int32RectChangedEventArgs(Int32Rect oldRect, Int32Rect newRect)
        {
            OldValue = oldRect;
            NewValue = newRect;
        }
    }
}
