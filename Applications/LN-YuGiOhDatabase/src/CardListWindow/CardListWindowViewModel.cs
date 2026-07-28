using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Data;

namespace LivreNoir.YuGiOhDatabase
{
    public class CardListWindowViewModel(CardDataCollection cards) : ObservableObjectBase
    {
        public ICardProvider CardProvider { get; } = cards;
        public bool ShowInTaskbar { get; set => SetValue(ref field, value); }
        public CloningCardList Cards { get; } = new(cards);
        public Card? SelectedCard { get; set => SetValue(ref field, value); }
        public string? SearchText { get; set => SetValue(ref field, value); }
    }
}
