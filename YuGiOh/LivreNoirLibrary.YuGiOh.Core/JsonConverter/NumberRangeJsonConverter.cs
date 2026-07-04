using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Converters
{
    public sealed class NumberRangeJsonConverter : JsonConverter<NumberRange>
    {
        public override NumberRange? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var text = reader.GetString().AsSpan();
                var index = 0;
                int lowerBound = 0, upperBound = 0;
                bool isEnabled = false, exclusive = false;
                foreach (var range in text.Split(','))
                {
                    var span = text[range].Trim();
                    switch (index)
                    {
                        case 0:
                            if (span.Length > 0 && int.TryParse(span, out lowerBound))
                            {
                                index++;
                            }
                            else
                            {
                                goto OnError;
                            }
                            break;
                        case 1:
                            if (span.Length > 0 && int.TryParse(span, out upperBound))
                            {
                                index++;
                            }
                            else
                            {
                                goto OnError;
                            }
                            break;
                        case 2:
                            isEnabled = span.Contains('e');
                            exclusive = span.Contains('x');
                            index++;
                            break;
                    }
                }
                return new(lowerBound, upperBound, isEnabled, exclusive);
            }
        OnError:
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, NumberRange value, JsonSerializerOptions options)
        {
            static string Suffix(NumberRange value) => (value.IsEnabled, value.Exclusive) switch
            {
                (true, true) => ",ex",
                (true, false) => ",e",
                (false, true) => ",x",
                _ => ""
            };

            var text = $"{value.LowerBound},{value.UpperBound}{Suffix(value)}";
            writer.WriteStringValue(text);
        }
    }
}
