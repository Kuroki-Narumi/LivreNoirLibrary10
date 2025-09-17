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
            writer.WriteEndObject();
        }
    }
}
