using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace LivreNoirLibrary.YuGiOh.Converters
{
    public class DateRangeJsonConverter : System.Text.Json.Serialization.JsonConverter<DateRange>
    {
        public override DateRange? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var text = reader.GetString().AsSpan();
                var index = 0;
                DateTime since = default, until = default;
                bool isEnabled = false, exclusive = false;
                foreach (var range in text.Split(','))
                {
                    var span = text[range].Trim();
                    switch (index)
                    {
                        case 0:
                            if (span.Length > 0 && DateTime.TryParse(span, out since))
                            {
                                index++;
                            }
                            else
                            {
                                goto OnError;
                            }
                            break;
                        case 1:
                            if (span.Length > 0)
                            {
                                if (!DateTime.TryParse(span, out until))
                                {
                                    until = DateTime.Now + TimeSpan.FromDays(365);
                                }
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
                return new(since, until, isEnabled, exclusive);
            }
        OnError:
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, DateRange value, JsonSerializerOptions options)
        {
            static string Suffix(DateRange value) => (value.IsEnabled, value.Exclusive) switch
            {
                (true, true) => ",ex",
                (true, false) => ",e",
                (false, true) => ",x",
                _ => ""
            };
            static string ToString(DateTime dt) => dt >= DateTime.Now ? "*" : dt.ToString("yyyy-MM-dd");

            var text = $"{value.Since:yyyy-MM-dd},{ToString(value.Until)}{Suffix(value)}";
            writer.WriteStringValue(text);
        }
    }
}
