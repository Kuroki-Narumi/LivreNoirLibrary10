using LivreNoirLibrary.Text;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Text.Json;

namespace LivreNoirLibrary.YuGiOh.Converters
{
    public sealed class CardJsonConverter : IWriteJsonJsonConverter<Card>
    {
        public override Card? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (JsonSerializer.Deserialize<Serializable.Card>(ref reader, options) is { } source)
            {
                return new(source);
            }
            throw new JsonException();
        }
    }
}
