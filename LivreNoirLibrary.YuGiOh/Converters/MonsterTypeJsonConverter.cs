using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Converters
{
    internal class MonsterTypeJsonConverter : JsonConverter<MonsterType>
    {
        public override MonsterType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    var str = reader.GetString();
                    if (Enum.TryParse<MonsterType>(str, out var value))
                    {
                        return value;
                    }
                    return Vocab.GetMonsterType(str);
                case JsonTokenType.Number:
                    return (MonsterType)reader.GetInt32();
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, MonsterType value, JsonSerializerOptions options) => writer.WriteStringValue(Vocab.GetName(value));
    }
}
