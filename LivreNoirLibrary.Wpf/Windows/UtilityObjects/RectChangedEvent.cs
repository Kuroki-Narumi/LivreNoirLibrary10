using System;
using System.Windows;

namespace LivreNoirLibrary.Windows
{
    public delegate void RectChangedEventHandler(object? sender, RectChangedEventArgs e);

    public class RectChangedEventArgs : EventArgs
    {
        public Rect OldValue { get; init; }
        public Rect NewValue { get; init; }

        public RectChangedEventArgs() { }
        public RectChangedEventArgs(Rect oldRect, Rect newRect)
        {
            OldValue = oldRect;
            NewValue = newRect;
        }
    }
}
