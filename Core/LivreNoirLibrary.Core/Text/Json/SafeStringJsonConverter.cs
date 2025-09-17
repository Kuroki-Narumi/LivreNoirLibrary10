using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Text
{
    public class SafeStringJsonConverter : JsonConverter<string>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => ReadCore(ref reader);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string? ReadCore(ref Utf8JsonReader reader) => reader.TokenType switch
        {
            JsonTokenType.Null => null,
            JsonTokenType.String => reader.GetString(),
            >= JsonTokenType.Number and <= JsonTokenType.False =>
                reader.HasValueSequence ? Encoding.UTF8.GetString(reader.ValueSequence) : Encoding.UTF8.GetString(reader.ValueSpan),
            _ => throw new JsonException($"cannot convert to string. (token: {reader.TokenType})"),
        };

        public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options) => writer.WriteStringValue(value);
    }
}
