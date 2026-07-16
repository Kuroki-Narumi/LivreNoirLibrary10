using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Search;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class CardSearchViewModel : ObservableObjectBase
    {
        public string CardSearchText { get; set => SetValue(ref field, value); } = "";
        public CardSearchConditions CardSearchConditions { get; } = new();
    }
}
