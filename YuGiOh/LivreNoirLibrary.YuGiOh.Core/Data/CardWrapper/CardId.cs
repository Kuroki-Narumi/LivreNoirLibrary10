using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class CardId(int id) : ObservableObjectBase, ICardId
    {
        public int Id { get; set => SetValue(ref field, value); } = id;
    }
}
