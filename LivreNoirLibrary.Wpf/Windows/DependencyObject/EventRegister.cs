using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;

namespace LivreNoirLibrary.Windows
{
    public static partial class EventRegister
    {
        [GeneratedRegex(@"Event?$", RegexOptions.CultureInvariant)]
        public static partial Regex EventRegex { get; }

        public static string GetEventName(string caller) => EventRegex.Replace(caller, "");

        public static RoutedEvent Register<TOwner, THandler>(RoutingStrategy strategy = RoutingStrategy.Bubble, [CallerMemberName] string caller = "")
            where TOwner : DependencyObject
            where THandler : Delegate
            => EventManager.RegisterRoutedEvent(GetEventName(caller), strategy, typeof(THandler), typeof(TOwner));
    }
}
