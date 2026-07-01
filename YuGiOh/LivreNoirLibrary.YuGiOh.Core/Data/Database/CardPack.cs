using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.YuGiOh.Converters;

namespace LivreNoirLibrary.YuGiOh.Data
{
    [JsonConverter(typeof(ViewModelCardPackJsonConverter))]
    public partial class CardPack : ObservableSortedList<CardPackItem.SortKey, CardPackItem>
    {
        public static bool IsTcgPack(string pid) => pid.EndsWith('e');

        public static string EnsureTcgSuffix(ReadOnlySpan<char> span, bool tcg) => tcg ? string.Create(span.Length + 1, span, BuildTcgPid) : new(span);

        private static void BuildTcgPid(Span<char> span, ReadOnlySpan<char> source)
        {
            source.CopyTo(span);
            span[^1] = 'e';
        }

        public string Name { get; set => SetValue(ref field, value); } = "";
        public string ProductId { get; set => SetValue(ref field, value, [nameof(IsTcg)]); } = "";
        public DateTime Date { get; set => SetValue(ref field, value, [nameof(DateText)]); }

        protected override CardPackItem.SortKey GetKey(CardPackItem item) => item.GetSortKey();

        public string DateText => Date == default ? "不明" : Date.ToString("yyyy-MM-dd");
        public bool IsTcg => IsTcgPack(ProductId);

        public CardPack() { }
        public CardPack(CardPack source) { Load(source); }
        public CardPack(Serializable.CardPack source) { Load(source); }

        public void Load(Serializable.CardPack source)
        {
            Date = source.Date;
            Name = source.Name;
            ProductId = source.ProductId;
            foreach (var item in source.Cards.AsSpan())
            {
                AddWithoutNotify(new(item));
            }
        }

        public void Load(CardPack source)
        {
            Date = source.Date;
            Name = source.Name;
            ProductId = source.ProductId;
            ClearWithoutNotify();
            foreach (var item in source.AsSpan())
            {
                AddWithoutNotify(item);
            }
            NotifyCollectionReset();
        }
    }
}
