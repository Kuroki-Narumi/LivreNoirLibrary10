using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Converters
{
    public sealed class ViewModelCardPackJsonConverter : JsonConverter<Data.CardPack>
    {
        public override Data.CardPack? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (JsonSerializer.Deserialize<Serializable.CardPack>(ref reader, options) is Serializable.CardPack source)
            {
                return new(source);
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Data.CardPack value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString(JsonPropertyNames.ProductId, value.ProductId);
            writer.WriteString(JsonPropertyNames.Name, value.Name);
            writer.WritePropertyName(JsonPropertyNames.Date);
            DateOnlyJsonConverter.Write(writer, value.Date);

            var cards = value.AsSpan();
            if (cards.Length is > 0)
            {
                writer.WritePropertyName(JsonPropertyNames.Cards);
                writer.WriteStartArray();
                foreach (var info in cards)
                {
                    writer.WriteStartObject();
                    writer.WriteNumber(JsonPropertyNames.Id, info.CardId);
                    writer.WriteString(JsonPropertyNames.Number, info.Number);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }

            writer.WriteEndObject();
        }
    }
}
