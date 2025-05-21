using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Text
{
    public class RationalJsonConverter : JsonConverter<Rational>
    {
        public override Rational Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.String)
            {
                var text = reader.GetString();
                if (Rational.TryParse(text, out var value))
                {
                    return value;
                }
            }
            else if (reader.TokenType is JsonTokenType.Number)
            {
                var v = reader.GetDecimal();
                try
                {
                    var value = Rational.ConvertBySBT(v);
                    return value;
                }
                catch { }
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Rational value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
