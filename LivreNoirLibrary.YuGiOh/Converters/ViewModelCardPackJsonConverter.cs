using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Converters
{
    public class ViewModelCardPackJsonConverter : JsonConverter<ViewModel.CardPack>
    {
        public override ViewModel.CardPack? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (JsonSerializer.Deserialize<Serializable.CardPack>(ref reader, options) is Serializable.CardPack source)
            {
                return new(source);
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, ViewModel.CardPack value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString(JsonPropertyNames.ProductId, value._productId);
            writer.WriteString(JsonPropertyNames.Name, value._name);
            writer.WritePropertyName(JsonPropertyNames.Date);
            DateOnlyJsonConverter.Write(writer, value._date);
            writer.WriteEndObject();
        }
    }
}
