using LivreNoirLibrary.Windows.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Windows.Converters
{
    public class KeyInputJsonConverter : JsonConverter<KeyInput>
    {
        public override KeyInput Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Number:
                    var value = reader.GetInt32();
                    return new KeyInput(value);
                case JsonTokenType.String:
                    return KeyInput.Parse(reader.GetString());
            }
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, KeyInput value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.ToInt());
        }
    }
}
