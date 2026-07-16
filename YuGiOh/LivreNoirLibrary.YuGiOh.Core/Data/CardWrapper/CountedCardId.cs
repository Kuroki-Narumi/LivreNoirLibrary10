namespace LivreNoirLibrary.YuGiOh.Data
{
    public partial class CountedCard(Card card) : CardWrapper(card)
    {
        public int Count { get; set => SetValue(ref field, value); }
    }
}
