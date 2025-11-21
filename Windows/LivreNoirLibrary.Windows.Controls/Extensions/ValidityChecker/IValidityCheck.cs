using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IValidityCheck
    {
        static readonly RoutedEvent ValidityChangedEvent = Events.Register<IInputElement, ValidityChangedEventHandler>();

        event ValidityChangedEventHandler? ValidityChanged;
        bool IsInputValid { get; }
    }
}
