using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.YuGiOh;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class DeckRecipeOrderItem(VocabData name, bool isNameFirst)
    {
        public static DeckRecipeOrderItem[] Items { get; } = [new(Vocab.Current.Deck_NumberFirst, false), new(Vocab.Current.Deck_NameFirst, true)];

        public VocabData Name { get; } = name;
        public bool IsNameFirst { get; } = isNameFirst;
    }
}
