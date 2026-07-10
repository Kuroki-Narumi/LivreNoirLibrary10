using System;
using System.Windows;
using LivreNoirLibrary.YuGiOh.Search;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class CardSearchPresetPresenter : PresetPresenterBase
    {
        public static readonly RoutedEvent ApplyEvent = Events.Register<CardSearchPresetPresenter, RoutedEventHandler<CardSearchConditionsPreset>>();

        public event RoutedEventHandler<CardSearchConditionsPreset>? Apply { add => AddHandler(ApplyEvent, value); remove => RemoveHandler(ApplyEvent, value); }

        protected override void TryRaiseApplyEvent()
        {
            if (DataContext is CardSearchConditionsPreset preset)
            {
                RaiseEvent(new RoutedEventArgs<CardSearchConditionsPreset>(preset, ApplyEvent, this));
            }
        }
    }
}
