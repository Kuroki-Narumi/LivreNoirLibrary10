using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Converters
{
    public sealed class AbilityJsonConverter : JsonConverter<Ability>
    {
        public override Ability Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.String => Vocab.GetAbility(reader.GetString()),
                JsonTokenType.Number => (Ability)reader.GetInt32(),
                JsonTokenType.Null => 0,
                _ => throw new JsonException(),
            };
        }

        public override void Write(Utf8JsonWriter writer, Ability value, JsonSerializerOptions options) => writer.WriteStringValue(string.Join("/", Vocab.GetNames(value)));
    }
}
