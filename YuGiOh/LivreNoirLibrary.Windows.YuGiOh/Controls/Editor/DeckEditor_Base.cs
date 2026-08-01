using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Search;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public abstract partial class DeckEditor_Base : FileEditorBase<DeckHistoryData>
    {
        [DependencyProperty]
        private ICardProvider? _cardProvider;
        [DependencyProperty]
        private Deck? _deck;

        protected virtual void OnCardProviderChanged(ICardProvider? value) { }

        protected virtual void OnDeckChanged(Deck? value)
        {
            this.ClearHistory();
        }

        protected sealed override DeckHistoryData GetHistoryData() => new(Deck);
        protected sealed override void ProcessNew() => Deck?.Clear();
        protected sealed override bool ProcessOpen(string path) => Deck is { } deck && deck.LoadFile(path, CardProvider);
        protected sealed override void ProcessSave(string path) => Json.Save(path, Deck);
    }
}
