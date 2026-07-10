using System;
using System.Windows;
using LivreNoirLibrary.YuGiOh.Search;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class CardSortPresetPresenter : PresetPresenterBase
    {
        public static readonly RoutedEvent ApplyEvent = Events.Register<CardSortPresetPresenter, RoutedEventHandler<CardSortOptionsPreset>>();

        public event RoutedEventHandler<CardSortOptionsPreset>? Apply { add => AddHandler(ApplyEvent, value); remove => RemoveHandler(ApplyEvent, value); }

        protected override void TryRaiseApplyEvent()
        {
            if (DataContext is CardSortOptionsPreset preset)
            {
                RaiseEvent(new RoutedEventArgs<CardSortOptionsPreset>(preset, ApplyEvent, this));
            }
        }
    }
}
