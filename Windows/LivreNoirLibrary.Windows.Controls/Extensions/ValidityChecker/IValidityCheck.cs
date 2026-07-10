using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IValidityCheck
    {
        static readonly RoutedEvent ValidityChangedEvent = Events.Register<IInputElement, RoutedEventHandler<bool>>();

        event RoutedEventHandler<bool>? ValidityChanged;
        bool IsInputValid { get; }
    }
}
