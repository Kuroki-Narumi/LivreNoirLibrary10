using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Converters
{
    public class AbilityJsonConverter : JsonConverter<Ability>
    {
        public override Ability Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    if (reader.GetString() is string str)
                    {
                        return Vocab.GetAbility(str.Split("/"));
                    }
                    return 0;
                case JsonTokenType.Number:
                    return (Ability)reader.GetInt32();
                case JsonTokenType.Null:
                    return 0;
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Ability value, JsonSerializerOptions options) => writer.WriteStringValue(string.Join("/", Vocab.GetNames(value)));
    }
}
