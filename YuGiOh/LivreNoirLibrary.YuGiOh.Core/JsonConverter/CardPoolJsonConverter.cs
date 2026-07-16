using LivreNoirLibrary.Text;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Text.Json;

namespace LivreNoirLibrary.YuGiOh.Converters
{
    public sealed class CardPoolJsonConverter : IWriteJsonJsonConverter<CardPool>
    {
        public override CardPool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (JsonSerializer.Deserialize<Serializable.CardPool>(ref reader, options) is { } source)
            {
                CardPool result = new();
                result.Load(source);
                return result;
            }
            throw new JsonException();
        }
    }
}
