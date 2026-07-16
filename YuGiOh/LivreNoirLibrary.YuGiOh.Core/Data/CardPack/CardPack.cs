using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.YuGiOh.Converters;

namespace LivreNoirLibrary.YuGiOh.Data
{
    [JsonConverter(typeof(IWriteJsonJsonConverter<CardPack>))]
    public partial class CardPack : ObservableSortedList<CardPackItem.SortKey, CardPackItem>, IWriteJson
    {
        public static bool IsTcgPack(string pid) => pid.EndsWith('e');

        public static string EnsureTcgSuffix(ReadOnlySpan<char> span, bool tcg) => tcg ? string.Create(span.Length + 1, span, BuildTcgPid) : new(span);

        public static string GetDateText(DateTime date) => date == default ? Vocab.Unknown : date.ToString("yyyy-MM-dd");

        private static void BuildTcgPid(Span<char> span, ReadOnlySpan<char> source)
        {
            source.CopyTo(span);
            span[^1] = 'e';
        }

        public string Name { get; set => SetValue(ref field, value); } = "";
        public string ProductId { get; set => SetValue(ref field, value, [nameof(IsTcg)]); } = "";
        public DateTime Date { get; set => SetValue(ref field, value, [nameof(DateText)]); }

        public string DateText => GetDateText(Date);
        public bool IsTcg => IsTcgPack(ProductId);
        public int NameLength => Name.LengthWithoutSpace();

        public CardPack() { }
        public CardPack(Serializable.CardPack source, ICardProvider provider) { Load(source, provider); }

        protected override CardPackItem.SortKey GetKey(CardPackItem item) => item.GetSortKey();

        public void Load(Serializable.CardPack source, ICardProvider provider)
        {
            Date = source.Date;
            Name = source.Name;
            ProductId = source.ProductId;
            foreach (var item in source.Cards.AsSpan())
            {
                if (provider.TryGet(item.CardId, out var card))
                {
                    AddWithoutNotify(new(card, item.Number));
                }
            }
        }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString(JsonPropertyNames.ProductId, ProductId);
            writer.WriteString(JsonPropertyNames.Name, Name);
            writer.WritePropertyName(JsonPropertyNames.Date);
            DateOnlyJsonConverter.Write(writer, Date);

            var cards = AsSpan();
            if (cards.Length is > 0)
            {
                writer.WritePropertyName(JsonPropertyNames.Cards);
                writer.WriteStartArray();
                foreach (var info in cards)
                {
                    writer.WriteStartObject();
                    writer.WriteNumber(JsonPropertyNames.Id, info.Card.Id);
                    writer.WriteString(JsonPropertyNames.Number, info.Number);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }

            writer.WriteEndObject();
        }
    }
}
