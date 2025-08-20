using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Lbm
{
    public abstract class ObjectJsonConverterBase<T> : JsonConverter<T>
        where T : ObjectBase
    {
        public const string PropertyName_Note = "note";

        public sealed override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.StartObject)
            {
                var obj = CreateInstance();

            }
            return ThrowUnexpectedToken(reader.TokenType, JsonTokenType.StartObject);
        }

        public static T ThrowUnexpectedToken(JsonTokenType given, JsonTokenType expected)
        {
            throw new JsonException($"unexpected token: {given}(expected: {expected})");
        }

        protected abstract T CreateInstance();

        public sealed override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteIfNotNull(PropertyName_Note, value.Note);
            WriteContents(writer, value, options);
            writer.WriteEndObject();
        }

        protected abstract void WriteContents(Utf8JsonWriter writer, T value, JsonSerializerOptions options);
    }
}
