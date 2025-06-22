using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IValidityCheck
    {
        public static readonly RoutedEvent ValidityChangedEvent = Events.Register<IInputElement, ValidityChangedEventHandler>();

        public event ValidityChangedEventHandler? ValidityChanged;
        public bool IsInputValid { get; }
    }
}
