using LivreNoirLibrary.Windows.Input;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    using OwnerType = UIElement;

    public static partial class YgoEvents
    {
        [RoutedEvent(typeof(OwnerType), typeof(CardLinkClickedEventHandler))]
        public static readonly RoutedEvent RequestOpenCardUrlEvent = RegisterEvent();

        [RoutedEvent(typeof(OwnerType), typeof(RoutedEventHandler<string>))]
        public static readonly RoutedEvent RequestOpenPackUrlEvent = RegisterEvent();

        [RoutedEvent(typeof(OwnerType), typeof(RoutedEventHandler<CardSearchConditionsPreset>))]
        public static readonly RoutedEvent DefaultSearchPresetChangedEvent = RegisterEvent();

        [RoutedEvent(typeof(OwnerType), typeof(RoutedEventHandler<CardSortOptionsPreset>))]
        public static readonly RoutedEvent DefaultSortPresetChangedEvent = RegisterEvent();

        public static void RaiseRequestOpenCardUrl(this OwnerType sender, int id, bool isTcg)
        {
            var args = new CardLinkClickedEventArgs(RequestOpenCardUrlEvent, sender)
            {
                Id = id,
                IsTcg = isTcg
            };
            sender.RaiseEvent(args);
        }

        public static void RaiseRequestOpenPackUrl(this OwnerType sender, string pid)
        {
            var args = new RoutedEventArgs<string>(pid, RequestOpenPackUrlEvent, sender);
            sender.RaiseEvent(args);
        }

        public static void RaiseDefaultSearchPresetChanged(this OwnerType sender, CardSearchConditionsPreset preset)
        {
            var args = new RoutedEventArgs<CardSearchConditionsPreset>(preset, DefaultSearchPresetChangedEvent, sender);
            sender.RaiseEvent(args);
        }

        public static void RaiseDefaultSortPresetChanged(this OwnerType sender, CardSortOptionsPreset preset)
        {
            var args = new RoutedEventArgs<CardSortOptionsPreset>(preset, DefaultSortPresetChangedEvent, sender);
            sender.RaiseEvent(args);
        }
    }
}
