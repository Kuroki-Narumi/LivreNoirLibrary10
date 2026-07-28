using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial interface IValidityCheck : IInputElement
    {
        [RoutedEvent(typeof(IInputElement), typeof(RoutedEventHandler<bool>))]
        public static readonly RoutedEvent ValidityChangedEvent = RegisterEvent();

        bool IsInputValid { get; }
    }
}
