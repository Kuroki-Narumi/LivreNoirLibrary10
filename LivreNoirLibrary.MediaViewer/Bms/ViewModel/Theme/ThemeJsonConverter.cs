using System;
using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class ThemeJsonConverter : JsonConverter<Theme>
    {
        public const string Prop_CommonColors = "common_colors";
        public const string Prop_Conductor = "conductor";
        public const string Prop_Meta = "meta";
        public const string Prop_Key = "key";
        public const string Prop_Bgm = "bgm";
        public const string Prop_LaneOrder = "order";
        public const string Prop_SeparatorWidth = "separator_width";

        public override Theme? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (JsonSerializer.Deserialize<SerializableTheme>(ref reader, options) is SerializableTheme theme)
            {
                return new(theme);
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Theme value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            WriteCore(writer, Prop_CommonColors, value._commonColors.ToSerializable(), options);
            WriteCore(writer, Prop_Conductor, value._conductor, options);
            WriteCore(writer, Prop_Meta, value._meta, options);
            WriteCore(writer, Prop_Key, value._key, options);
            writer.WritePropertyName(Prop_Bgm);
            JsonSerializer.Serialize(writer, value._bgmLane, options);
            writer.WriteNumber(Prop_SeparatorWidth, value._separatorWidth);
        }

        private static void WriteCore<T>(Utf8JsonWriter writer, string propName, T value, JsonSerializerOptions options)
            where T : ICollection
        {
            if (value.Count is > 0)
            {
                writer.WritePropertyName(propName);
                JsonSerializer.Serialize(writer, value, options);
            }
        }
    }
}
