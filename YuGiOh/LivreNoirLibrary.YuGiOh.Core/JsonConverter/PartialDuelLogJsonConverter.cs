using LivreNoirLibrary.YuGiOh.MasterDuel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Converters
{
    public class PartialDuelLogJsonConverter : JsonConverter<DuelLog>
    {
        public override DuelLog? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (JsonSerializer.Deserialize<DuelLog>(ref reader, options) is { } log)
            {
                return log;
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, DuelLog value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(JsonPropertyNames.Log_User);
            JsonSerializer.Serialize(writer, value.UserTags, options);
            writer.WriteString(JsonPropertyNames.Log_Rank, value.Rank.ToString());
            writer.WriteEndObject();
        }
    }
}
