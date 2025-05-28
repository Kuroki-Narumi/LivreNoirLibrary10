using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Converters;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    [JsonConverter(typeof(ViewModelCardPackJsonConverter))]
    public partial class CardPack : SortedCardWrapperList<NumberedCard>
    {
        public static bool IsTcgPack(string pid) => pid.EndsWith('e');

        [ObservableProperty]
        internal string _name = "";
        [ObservableProperty(Related = [nameof(IsTcg)])]
        internal string _productId = "";
        [ObservableProperty(Related = [nameof(DateString)])]
        internal DateTime _date;

        protected override int GetKey(NumberedCard item) => item.Index;

        public string DateString => _date == default ? "不明" : Date.ToString("yyyy-MM-dd");
        public bool IsTcg => IsTcgPack(_productId);

        public CardPack() { }
        public CardPack(CardPack source) { Load(source); }
        public CardPack(Serializable.CardPack source) { Load(source); }

        public override void Add(Card card)
        {
            NumberedCard w = new(card, card.GetNumber(_productId));
            var key = GetKey(w);
            var index = _key_list.BinarySearch(key);
            if (index is < 0)
            {
                index = ~index;
                AddItem(index, w);
                OnCollectionAdded(w, index);
            }
        }

        internal void AddWithoutNotify(Card card)
        {
            NumberedCard w = new(card, card.GetNumber(_productId));
            var key = GetKey(w);
            var index = _key_list.BinarySearch(key);
            if (index is < 0)
            {
                index = ~index;
                AddItem(index, w);
            }
        }

        public override bool Remove(Card card) => RemoveKey(NumberedCard.GetIndex(card, card.GetNumber(_productId)));

        public void Load(Serializable.CardPack source)
        {
            Date = source.Date;
            Name = source.Name;
            ProductId = source.ProductId;
        }

        public void Load(CardPack source)
        {
            Date = source._date;
            Name = source._name;
            ProductId = source._productId;
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
