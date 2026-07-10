using LivreNoirLibrary.Windows.Input;
using System;
using System.Windows;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    using OwnerType = UIElement;

    public static class HyperLinks
    {
        public static RoutedCommand CardLinkCommand { get; } = Commands.Create();

        public static readonly RoutedEvent CardLinkClickedEvent = Events.Register<OwnerType, CardLinkClickedEventHandler>();
        public static readonly RoutedEvent PackLinkClickedEvent = Events.Register<OwnerType, RoutedEventHandler<string>>();
        public static readonly RoutedEvent RelatedTextClickedEvent = Events.Register<OwnerType, RoutedEventHandler<string>>();

        public static void AddCardLinkClickedHandler(DependencyObject d, CardLinkClickedEventHandler handler)
            => (d as OwnerType)?.AddHandler(CardLinkClickedEvent, handler);

        public static void RemoveCardLinkClickedHandler(DependencyObject d, CardLinkClickedEventHandler handler)
            => (d as OwnerType)?.RemoveHandler(CardLinkClickedEvent, handler);

        public static void AddPackLinkClickedHandler(DependencyObject d, RoutedEventHandler<string> handler)
            => (d as OwnerType)?.AddHandler(PackLinkClickedEvent, handler);

        public static void RemovePackLinkClickedHandler(DependencyObject d, RoutedEventHandler<string> handler)
            => (d as OwnerType)?.RemoveHandler(PackLinkClickedEvent, handler);

        public static void AddRelatedTextClickedHandler(DependencyObject d, RoutedEventHandler<string> handler)
            => (d as OwnerType)?.AddHandler(RelatedTextClickedEvent, handler);

        public static void RemoveRelatedTextClickedHandler(DependencyObject d, RoutedEventHandler<string> handler)
            => (d as OwnerType)?.RemoveHandler(RelatedTextClickedEvent, handler);

        public static void RaiseCardLinkClicked(this OwnerType sender, int id, bool isTcg)
        {
            var args = new CardLinkClickedEventArgs(CardLinkClickedEvent, sender)
            {
                Id = id,
                IsTcg = isTcg
            };
            sender.RaiseEvent(args);
        }

        public static void RaisePackLinkClicked(this OwnerType sender, string pid)
        {
            var args = new RoutedEventArgs<string>(pid, PackLinkClickedEvent, sender);
            sender.RaiseEvent(args);
        }

        public static void RaiseRelatedTextClicked(this OwnerType sender, string text)
        {
            var args = new RoutedEventArgs<string>(text, RelatedTextClickedEvent, sender);
            sender.RaiseEvent(args);
        }

        public static void AddLinkClickHandlers(this OwnerType d, LinkClickHandlers handlers)
        {
            if (handlers.CardLink is { } cardLink)
            {
                AddCardLinkClickedHandler(d, cardLink);
            }
            if (handlers.PackLink is { } packLink)
            {
                AddPackLinkClickedHandler(d, packLink);
            }
            if (handlers.RelatedText is { } related)
            {
                AddRelatedTextClickedHandler(d, related);
            }
        }
    }
}
