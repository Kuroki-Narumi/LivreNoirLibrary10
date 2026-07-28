using System;
using System.Windows;
using LivreNoirLibrary.YuGiOh.Search;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public partial class CardSortPresetPresenter : PresetPresenterBase
    {
        [RoutedEvent]
        public partial event RoutedEventHandler<CardSortOptionsPreset>? Apply;

        protected override void TryRaiseApplyEvent()
        {
            if (DataContext is CardSortOptionsPreset preset)
            {
                RaiseEvent(new RoutedEventArgs<CardSortOptionsPreset>(preset, ApplyEvent, this));
            }
        }
    }
}
