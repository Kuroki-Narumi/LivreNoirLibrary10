using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public readonly struct LinkClickHandlers
    {
        public CardLinkClickedEventHandler? CardLink { get; init; }
        public RoutedEventHandler<string>? PackLink { get; init; }
        public RoutedEventHandler<string>? RelatedText { get; init; }
    }
}
