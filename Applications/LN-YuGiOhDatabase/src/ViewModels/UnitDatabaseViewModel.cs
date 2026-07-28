using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.YuGiOh.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Search;
using System.Collections.Generic;

namespace LivreNoir.YuGiOhDatabase
{
    public class UnitDatabaseViewModel : ObservableObjectBase
    {
        public bool IsUpdateVisible { get; set => SetValue(ref field, value); }
        public Card? SelectedCard { get; set => SetValue(ref field, value); }
        public CardPack? SelectedPack { get; set => SetValue(ref field, value); }
        public CardSearchConditions DefaultCardSearchConditions { get; } = new();
        public CardSortOptionCollection DefaultCardSortOptions { get; } = [];
    }
}
