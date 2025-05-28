using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Converters
{
    public class StatusJsonConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    var str = reader.GetString();
                    if (int.TryParse(str, out var v))
                    {
                        return v;
                    }
                    else
                    {
                        return -1;
                    }
                case JsonTokenType.Number:
                    return reader.GetInt32();
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options) => ViewModelCardJsonConverter.WriteStatus(writer, null, value);
    }
}
