using System;
using System.Windows;

namespace LivreNoirLibrary.Windows
{
    public delegate void RectChangedEventHandler(object? sender, RectChangedEventArgs e);

    public class RectChangedEventArgs : EventArgs
    {
        public Rect OldRect { get; init; }
        public Rect NewRect { get; init; }

        public RectChangedEventArgs() { }
        public RectChangedEventArgs(Rect oldRect, Rect newRect)
        {
            OldRect = oldRect;
            NewRect = newRect;
        }
    }
}
