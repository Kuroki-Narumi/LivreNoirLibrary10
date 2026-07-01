using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.YuGiOh.Converters
{
    public sealed class ViewModelCardJsonConverter : JsonConverter<Data.Card>
    {
        public override Data.Card? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (JsonSerializer.Deserialize<Serializable.Card>(ref reader, options) is Serializable.Card source)
            {
                return new(source);
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Data.Card value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber(JsonPropertyNames.Id, value.Id);
            writer.WriteStringIfNotNull(JsonPropertyNames.Name, value.Name);
            writer.WriteStringIfNotNull(JsonPropertyNames.Ruby, value.Ruby);
            writer.WriteStringIfNotNull(JsonPropertyNames.EnName, value.EnName);
            Write(writer, value.CardType);
            writer.WriteStringIfNotNull(JsonPropertyNames.Text, value.Text);
            writer.WriteBooleanIfTrue(JsonPropertyNames.Unusable, value.Unusable);
            if (value.CardType.IsMonster())
            {
                writer.WritePropertyName(JsonPropertyNames.MonsterInfo);
                writer.WriteStartObject();
                Write(writer, value.Attribute);
                Write(writer, value.MonsterType);
                writer.WriteBooleanIfTrue(JsonPropertyNames.HasEffect, value.HasEffect);
                Write(writer, value.Ability);
                writer.WriteNumber(JsonPropertyNames.Level, value.Level);
                WriteStatus(writer, JsonPropertyNames.Atk, value.Atk);
                WriteStatus(writer, JsonPropertyNames.Def, value.Def);
                writer.WriteEndObject();
            }
            if (value.Ability.IsPendulum())
            {
                writer.WritePropertyName(JsonPropertyNames.PendulumInfo);
                writer.WriteStartObject();
                writer.WriteNumber(JsonPropertyNames.Scale, value.PendulumScale);
                writer.WriteStringIfNotNull(JsonPropertyNames.Text, value.PendulumText);
                writer.WriteEndObject();
            }
            writer.WriteEndObject();
        }

        public static void Write(Utf8JsonWriter writer, CardType value)
        {
            writer.WriteString(JsonPropertyNames.Type, Vocab.GetName(value));
        }

        public static void Write(Utf8JsonWriter writer, Attribute value)
        {
            writer.WriteString(JsonPropertyNames.Attribute, Vocab.GetName(value));
        }

        public static void Write(Utf8JsonWriter writer, MonsterType value)
        {
            writer.WriteString(JsonPropertyNames.Type, Vocab.GetName(value));
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
