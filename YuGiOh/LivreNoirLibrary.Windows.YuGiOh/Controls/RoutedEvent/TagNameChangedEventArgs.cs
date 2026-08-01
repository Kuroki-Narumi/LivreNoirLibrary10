using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public delegate void TagNameChangedEventHandler(object sender, TagNameChangedEventArgs e);

    public class TagNameChangedEventArgs(string? oldName, string? newName, RoutedEvent routedEvent, object source) : RoutedEventArgs(routedEvent, source)
    {
        public string? OldName { get; } = oldName;
        public string? NewName { get; } = newName;
    }
}
