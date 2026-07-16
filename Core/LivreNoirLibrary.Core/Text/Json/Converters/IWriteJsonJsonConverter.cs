using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Text
{
    public class IWriteJsonJsonConverter<T> : JsonConverter<T>
        where T : IWriteJson
    {
        public sealed override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            value.WriteJson(writer, options);
        }

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
