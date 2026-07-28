using LivreNoirLibrary.Windows.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.YuGiOh
{
    public static class YgoCommands
    {
        public static RoutedCommand DetachCardInfo { get; } = Commands.Create();
        public static RoutedCommand DetachCardList { get; } = Commands.Create();

        public static RoutedCommand CardLink { get; } = Commands.Create();
        public static RoutedCommand PackLink { get; } = Commands.Create();
        public static RoutedCommand RelatedText { get; } = Commands.Create();

        public static RoutedCommand UpdateDatabase { get; } = Commands.Create();
        public static RoutedCommand LoadOcgRegulation { get; } = Commands.Create();
        public static RoutedCommand LoadTcgRegulation { get; } = Commands.Create();

        public static RoutedCommand AddToDeck { get; } = Commands.Create();
        public static RoutedCommand RemoveFromDeck { get; } = Commands.Create();

        public static RoutedCommand RefreshItems { get; } = Commands.Create();
    }
}
