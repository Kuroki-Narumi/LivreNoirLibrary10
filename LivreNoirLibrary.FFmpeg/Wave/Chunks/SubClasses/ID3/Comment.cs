using System.IO;
using System.Text.Json;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public partial class ID3
    {
        public class Comment : Frame
        {
            public byte Encoding { get; set; }
            public string Language { get; set; } = "";

            protected override uint GetFrameLength() => (uint)(Data.Length + 4);

            protected override void DumpExt(BinaryWriter s)
            {
                s.Write(Encoding);
                s.WriteASCII(Language, 3);
            }

            protected override void WriteJsonExt(Utf8JsonWriter writer, JsonSerializerOptions options)
            {
                writer.WriteNumber("encoding", Encoding);
                writer.WriteString("language", Language);
            }
        }
    }
}
