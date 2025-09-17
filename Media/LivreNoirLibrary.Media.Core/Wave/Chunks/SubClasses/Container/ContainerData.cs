using System.Text.Json;
using System.Collections.Generic;
using System.IO;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public abstract class ContainerData
    {
        public int Id { get; set; }

        public virtual void Dump(BinaryWriter s)
        {
            s.Write(Id);
        }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            WriteJsonContent(writer, options);
            writer.WriteEndObject();
        }

        protected virtual void WriteJsonContent(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteNumber("id", Id);
        }
    }
}
