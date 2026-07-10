using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Converters
{
    public sealed class CardSortOptionJsonConverter : JsonConverter<CardSortOption>
    {
        public override CardSortOption Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.Number => CardSortOption.FromIntValue(reader.GetInt32()),
                JsonTokenType.Null => default,
                _ => throw new JsonException(),
            };
        }

        public override void Write(Utf8JsonWriter writer, CardSortOption value, JsonSerializerOptions options) => writer.WriteNumberValue(value.GetIntValue());
    }
}
