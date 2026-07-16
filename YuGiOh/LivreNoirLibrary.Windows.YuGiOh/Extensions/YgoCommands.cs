using LivreNoirLibrary.Windows.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.YuGiOh
{
    public static class YgoCommands
    {
        public static RoutedCommand CardLink { get; } = Commands.Create();

        public static RoutedCommand UpdateDatabase { get; } = Commands.Create();
        public static RoutedCommand LoadOcgRegulation { get; } = Commands.Create();
        public static RoutedCommand LoadTcgRegulation { get; } = Commands.Create();

        public static RoutedCommand OpenSearch { get; } = Commands.Create();
        public static RoutedCommand OpenSort { get; } = Commands.Create();
        public static RoutedCommand SearchClear { get; } = Commands.Create();
    }
}
