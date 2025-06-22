using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Input;
using System.Windows.Media;
using LivreNoirLibrary.Windows.Input;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class LaneInfoJsonConverter : JsonConverter<LaneInfo>
    {
        public const string SeparatorName = "Separator";
        public const string Prop_Name = "name";
        public const string Prop_Lane = "lane";
        public const string Prop_Width = "width";
        public const string Prop_Back = "back";
        public const string Prop_Note = "note";
        public const string Prop_Long = "long";
        public const string Prop_Key = "key";

        public override LaneInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.StartObject)
            {
                var name = "";
                int lane = 0, width = LaneInfo.DefaultLaneWidth;
                Color back = default, note = default, @long = default;
                var key = Key.None;
                reader.Read();
                while (reader.TokenType is not JsonTokenType.EndObject)
                {
                    var propName = GetStringValue(ref reader, JsonTokenType.PropertyName);
                    switch (propName)
                    {
                        case Prop_Name:
                            name = GetStringValue(ref reader)!;
                            break;
                        case Prop_Lane:
                            lane = GetNumberValue(ref reader);
                            break;
                        case Prop_Width:
                            width = GetNumberValue(ref reader);
                            break;
                        case Prop_Back:
                            back = GetStringValue(ref reader)!.ToColor();
                            break;
                        case Prop_Note:
                            note = GetStringValue(ref reader)!.ToColor();
                            break;
                        case Prop_Long:
                            @long = GetStringValue(ref reader)!.ToColor();
                            break;
                        case Prop_Key:
                            key = KeyInput.GetKey(GetStringValue(ref reader)!);
                            break;
                        default:
                            reader.Skip();
                            break;
                    }
                }
                return new(name, lane, width, back, note, @long, key);
            }
            // オブジェクト開始以外のトークンは全てセパレータとみなす
            else
            {
                return LaneInfo.Separator;
            }
        }

        private static string? GetStringValue(ref Utf8JsonReader reader, JsonTokenType type = JsonTokenType.String)
        {
            while (reader.TokenType is JsonTokenType.Comment)
            {
                reader.Read();
            }
            if (reader.TokenType != type)
            {
                ThrowJsonException();
            }
            var value = reader.GetString();
            reader.Read();
            return value;
        }

        private static int GetNumberValue(ref Utf8JsonReader reader)
        {
            while (reader.TokenType is JsonTokenType.Comment)
            {
                reader.Read();
            }
            if (reader.TokenType is not JsonTokenType.Number)
            {
                ThrowJsonException();
            }
            var value = reader.GetInt32();
            reader.Read();
            return value;
        }

        private static void ThrowJsonException() => throw new JsonException();

        public override void Write(Utf8JsonWriter writer, LaneInfo value, JsonSerializerOptions options)
        {
            if (value.IsSeparator)
            {
                writer.WriteStringValue(SeparatorName);
            }
            else
            {
                writer.WriteStartObject();
                if (!string.IsNullOrEmpty(value.Name))
                {
                    writer.WriteString(Prop_Name, value.Name);
                }
                if (value.Lane is not 0)
                {
                    writer.WriteNumber(Prop_Lane, value.Lane);
                }
                writer.WriteNumber(Prop_Width, value.Width);
                if (value.BackColor != default)
                {
                    writer.WriteString(Prop_Back, value.BackColor.GetColorCode());
                }
                if (value.NoteColor != default)
                {
                    writer.WriteString(Prop_Note, value.NoteColor.GetColorCode());
                }
                if (value.LongColor != default)
                {
                    writer.WriteString(Prop_Long, value.LongColor.GetColorCode());
                }
                if (value.Key is not Key.None)
                {
                    writer.WriteString(Prop_Key, value.Key.ToString());
                }
                writer.WriteEndObject();
            }
        }
    }
}
