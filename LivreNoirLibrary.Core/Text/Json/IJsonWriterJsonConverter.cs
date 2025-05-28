using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Text
{
    public class IJsonWriterJsonConverter : JsonConverter<IJsonWriter>
    {
        public override IJsonWriter? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, IJsonWriter value, JsonSerializerOptions options)
        {
            value.WriteJson(writer, options);
        }
    }
}
