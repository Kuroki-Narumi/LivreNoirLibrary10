using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    public interface IValidityCheck
    {
        public static readonly RoutedEvent ValidityChangedEvent = EventRegister.Register<PropertyHolder, ValidityChangedEventHandler>();

        public event ValidityChangedEventHandler? ValidityChanged;
        public bool IsInputValid { get; }
    }
}
