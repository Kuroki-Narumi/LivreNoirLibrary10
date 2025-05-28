using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Converters
{
    public class CardTypeJsonConverter : JsonConverter<CardType>
    {
        public override CardType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    var str = reader.GetString();
                    if (Enum.TryParse<CardType>(str, out var value))
                    {
                        return value;
                    }
                    return Vocab.GetCardType(str);
                case JsonTokenType.Number:
                    return (CardType)reader.GetInt32();
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, CardType value, JsonSerializerOptions options) => writer.WriteStringValue(Vocab.GetName(value));
    }
}
