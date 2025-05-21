using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    public delegate void ValidityChangedEventHandler(object sender, ValidityChangedEventArgs e);

    public class ValidityChangedEventArgs : RoutedEventArgs
    {
        public bool IsValid { get; init; }

        public ValidityChangedEventArgs() : base() { }
        public ValidityChangedEventArgs(RoutedEvent e) : base(e) { }
        public ValidityChangedEventArgs(RoutedEvent e, object source) : base(e, source) { }
        public ValidityChangedEventArgs(RoutedEvent e, bool isValid, object source) : base(e, source) { IsValid = isValid; }
    }
}
