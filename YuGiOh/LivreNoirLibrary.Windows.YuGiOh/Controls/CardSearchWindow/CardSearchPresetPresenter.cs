using System;
using System.Windows;
using LivreNoirLibrary.YuGiOh.Search;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public partial class CardSearchPresetPresenter : PresetPresenterBase
    {
        [RoutedEvent]
        public partial event RoutedEventHandler<CardSearchConditionsPreset>? Apply;

        protected override void TryRaiseApplyEvent()
        {
            if (DataContext is CardSearchConditionsPreset preset)
            {
                RaiseEvent(new RoutedEventArgs<CardSearchConditionsPreset>(preset, ApplyEvent, this));
            }
        }
    }
}
