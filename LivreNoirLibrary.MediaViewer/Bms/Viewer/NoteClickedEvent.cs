using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public delegate void NoteClickedEventHandler(object sender, NoteClickedEventArgs e);

    public class NoteClickedEventArgs : RoutedEventArgs
    {
        public NoteRect? Rect { get; init; }
        public bool DoubleClick { get; init; }

        public NoteClickedEventArgs() : base() { }
        public NoteClickedEventArgs(RoutedEvent e, object source) : base(e, source) { }
        public NoteClickedEventArgs(RoutedEvent e, object source, NoteRect rect, bool doubleClick) : base(e, source)
        {
            Rect = rect;
            DoubleClick = doubleClick;
        }
    }
}
