using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.YuGiOh.Converters
{
    public sealed class ViewModelCardJsonConverter : JsonConverter<ViewModel.Card>
    {
        public override ViewModel.Card? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (JsonSerializer.Deserialize<Serializable.Card>(ref reader, options) is Serializable.Card source)
            {
                return new(source);
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, ViewModel.Card value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber(JsonPropertyNames.Id, value._id);
            writer.WriteIfNotNull(JsonPropertyNames.Name, value._name);
            writer.WriteIfNotNull(JsonPropertyNames.Ruby, value._ruby);
            writer.WriteIfNotNull(JsonPropertyNames.EnName, value._enName);
            Write(writer, value._cardType);
            writer.WriteIfNotNull(JsonPropertyNames.Text, value._text);
            writer.WriteIfTrue(JsonPropertyNames.Unusable, value._unusable);
            if (value._cardType.IsMonster())
            {
                writer.WritePropertyName(JsonPropertyNames.MonsterInfo);
                writer.WriteStartObject();
                Write(writer, value._attribute);
                Write(writer, value._monsterType);
                writer.WriteIfTrue(JsonPropertyNames.Effect, value._effect);
                Write(writer, value._ability);
                writer.WriteNumber(JsonPropertyNames.Level, value._level);
                WriteStatus(writer, JsonPropertyNames.Atk, value._atk);
                WriteStatus(writer, JsonPropertyNames.Def, value._def);
                writer.WriteEndObject();
                if (value._ability.IsPendulum())
                {
                    writer.WritePropertyName(JsonPropertyNames.PendulumInfo);
                    writer.WriteStartObject();
                    writer.WriteNumber(JsonPropertyNames.Scale, value._pendulumScale);
                    writer.WriteIfNotNull(JsonPropertyNames.Text, value._pendulumText);
                    writer.WriteEndObject();
                }
            }
            if (value._packInfo.Count is > 0)
            {
                writer.WritePropertyName(JsonPropertyNames.PackInfo);
                writer.WriteStartArray();
                foreach (var info in value._packInfo)
                {
                    writer.WriteStartObject();
                    writer.WriteString(JsonPropertyNames.ProductId, info._productId);
                    writer.WriteString(JsonPropertyNames.Number, info._number);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
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
