using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.YuGiOh
{
    public delegate void CardLinkClickedEventHandler(object sender, CardLinkClickedEventArgs e);

    public class CardLinkClickedEventArgs : RoutedEventArgs
    {
        public required int Id { get; init; }
        public bool IsTcg { get; init; }

        public CardLinkClickedEventArgs() : base() { }
        public CardLinkClickedEventArgs(RoutedEvent e, object? source) : base(e, source) { }
    }
}
