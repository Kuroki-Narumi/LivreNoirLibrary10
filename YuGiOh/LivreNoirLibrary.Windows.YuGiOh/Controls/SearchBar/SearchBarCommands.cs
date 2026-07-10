using LivreNoirLibrary.Windows.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public static class SearchCommands
    {
        public static RoutedCommand Search { get; } = Commands.Create(typeof(SearchBar));
        public static RoutedCommand Sort { get; } = Commands.Create(typeof(SearchBar));
        public static RoutedCommand Clear { get; } = Commands.Create(typeof(SearchBar));
    }
}
