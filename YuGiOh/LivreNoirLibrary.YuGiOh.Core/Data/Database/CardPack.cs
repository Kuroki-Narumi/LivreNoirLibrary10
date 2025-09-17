using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using LivreNoirLibrary.YuGiOh.Converters;

namespace LivreNoirLibrary.YuGiOh.Data
{
    [JsonConverter(typeof(ViewModelCardPackJsonConverter))]
    public partial class CardPack : SortedCardWrapperList<NumberedCard>
    {
        public static bool IsTcgPack(string pid) => pid.EndsWith('e');

        public string Name { get; set => SetValue(ref field, value); } = "";
        public string ProductId { get; set => SetValue(ref field, value, [nameof(IsTcg)]); } = "";
        public DateTime Date { get; set => SetValue(ref field, value, [nameof(DateText)]); }

        protected override int GetKey(NumberedCard item) => item.Index;

        public string DateText => Date == default ? "不明" : Date.ToString("yyyy-MM-dd");
        public bool IsTcg => IsTcgPack(ProductId);

        public CardPack() { }
        public CardPack(CardPack source) { Load(source); }
        public CardPack(Serializable.CardPack source) { Load(source); }

        public override void Add(Card card)
        {
            NumberedCard w = new(card, card.GetNumber(ProductId));
            var key = GetKey(w);
            var index = IndexOfKey(key);
            if (index is < 0)
            {
                index = ~index;
                InsertItem(index, key, w);
                OnCollectionAdded(w, index);
            }
        }

        internal void AddWithoutNotify(Card card)
        {
            NumberedCard w = new(card, card.GetNumber(ProductId));
            var (key, index) = GetKeyAndIndex(w);
            if (index is < 0)
            {
                InsertItem(~index, key, w);
            }
        }

        public override bool Remove(Card card) => RemoveKey(NumberedCard.GetIndex(card, card.GetNumber(ProductId)));

        public void Load(Serializable.CardPack source)
        {
            Date = source.Date;
            Name = source.Name;
            ProductId = source.ProductId;
        }

        public void Load(CardPack source)
        {
            Date = source.Date;
            Name = source.Name;
            ProductId = source.ProductId;
            ClearWithoutNotify();
            foreach (var item in source)
            {
                AddWithoutNotify(item);
            }
            NotifyCollectionReset();
        }

        public override void Load(IEnumerable<Card> source)
        {
            ClearWithoutNotify();
            foreach (var card in source)
            {
                AddWithoutNotify(card);
            }
            NotifyCollectionReset();
        }
    }
}
