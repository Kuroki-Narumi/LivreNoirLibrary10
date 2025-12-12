using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Input;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows.Input;

namespace LivreNoirLibrary.Windows.Media.Bms
{
    public abstract class LaneInfoJsonConverter : JsonConverter<LaneInfo>
    {
        public const string SeparatorName = "Separator";
        public const string Prop_Name = "name";
        public const string Prop_Channel = "channel";
        public const string Prop_Width = "width";
        public const string Prop_Back = "back";
        public const string Prop_Note = "note";
        public const string Prop_Long = "long";
        public const string Prop_Key = "key";

        public override LaneInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.StartObject)
            {
                LaneInfo result = new();
                using (var doc = JsonDocument.ParseValue(ref reader))
                {
                    foreach (var prop in doc.RootElement.EnumerateObject())
                    {
                        var value = prop.Value;
                        switch (prop.Name)
                        {
                            case Prop_Name:
                                result.Name = value.GetString() ?? "";
                                break;
                            case Prop_Channel:
                                switch (value.ValueKind)
                                {
                                    case JsonValueKind.String:
                                        var str = value.GetString()!;
                                        result.Channel = Enum.TryParse<Channel>(str, out var ch) ? ch : BmsUtils.ToChannel(str);
                                        break;
                                    case JsonValueKind.Number:
                                        result.Channel = (Channel)value.GetInt32();
                                        break;
                                }
                                break;
                            case Prop_Width:
                                result.Width = prop.Value.GetInt32();
                                break;
                            case Prop_Back:
                                result.BackColor = prop.Value.GetString()!.ToColor();
                                break;
                            case Prop_Note:
                                result.NoteColor = prop.Value.GetString()!.ToColor();
                                break;
                            case Prop_Long:
                                result.LongColor = prop.Value.GetString()!.ToColor();
                                break;
                            case Prop_Key:
                                switch (value.ValueKind)
                                {
                                    case JsonValueKind.String:
                                        result.Key = KeyInput.GetKey(value.GetString()!);
                                        break;
                                    case JsonValueKind.Number:
                                        result.Key = (Key)value.GetInt32();
                                        break;
                                }
                                break;
                        }
                    }
                }
                return result;
            }
            // オブジェクト開始以外のトークンは全てセパレータとみなす
            else
            {
                return LaneInfo.Separator;
            }
        }

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
                writer.WriteString(Prop_Channel, BmsUtils.ToBased(value.Channel));
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
                if (value.Key is not 0)
                {
                    writer.WriteString(Prop_Key, KeyInput.GetKeyName(value.Key));
                }
                writer.WriteEndObject();
            }
        }
    }
}
