using LivreNoirLibrary.Text;
using LivreNoirLibrary.YuGiOh.Converters;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Data
{
    [JsonConverter(typeof(CardJsonConverter))]
    public partial class Card : IWriteJson
    {
        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumberIfNotZero(JsonPropertyNames.Id, Id);
            writer.WriteStringIfNotNull(JsonPropertyNames.Name, Name);
            writer.WriteStringIfNotNull(JsonPropertyNames.Ruby, Ruby);
            writer.WriteStringIfNotNull(JsonPropertyNames.EnName, EnName);
            writer.WriteString(JsonPropertyNames.Type, Vocab.GetName(CardType));
            writer.WriteStringIfNotNull(JsonPropertyNames.Text, Text);
            writer.WriteBooleanIfTrue(JsonPropertyNames.Unusable, Unusable);
            if (CardType.IsMonster())
            {
                writer.WritePropertyName(JsonPropertyNames.MonsterInfo);
                writer.WriteStartObject();
                writer.WriteString(JsonPropertyNames.Attribute, Vocab.GetName(Attribute));
                writer.WriteString(JsonPropertyNames.Type, Vocab.GetName(MonsterType));
                writer.WriteBooleanIfTrue(JsonPropertyNames.HasEffect, HasEffect);
                Write(writer, Ability);
                writer.WriteNumber(JsonPropertyNames.Level, Level);
                WriteStatus(writer, JsonPropertyNames.Atk, Atk);
                WriteStatus(writer, JsonPropertyNames.Def, Def);
                writer.WriteEndObject();
            }
            if (Ability.IsPendulum())
            {
                writer.WritePropertyName(JsonPropertyNames.PendulumInfo);
                writer.WriteStartObject();
                writer.WriteNumber(JsonPropertyNames.Scale, PendulumScale);
                writer.WriteStringIfNotNull(JsonPropertyNames.Text, PendulumText);
                writer.WriteEndObject();
            }
            writer.WriteEndObject();
        }

        public static void Write(Utf8JsonWriter writer, Ability value)
        {
            if (value is not 0)
            {
                writer.WriteString(JsonPropertyNames.Ability, string.Join("/", Vocab.GetNames(value)));
            }
        }

        public static void WriteStatus(Utf8JsonWriter writer, string? propertyName, int value)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                writer.WritePropertyName(propertyName);
            }
            if (value is >= 0)
            {
                writer.WriteNumberValue(value);
            }
            else
            {
                writer.WriteStringValue(Vocab.Unknown);
            }
        }
    }
}
