using System.Text.Json;
using System.Collections.Generic;
using System.IO;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public partial class ID3
    {
        public class Text : Frame
        {
            public byte Encoding { get; set; }

            protected override uint GetFrameLength() => (uint)(Data.Length + 1);

            protected override void DumpExt(BinaryWriter s)
            {
                s.Write(Encoding);
            }

            protected override void WriteJsonExt(Utf8JsonWriter writer, JsonSerializerOptions options)
            {
                writer.WriteNumber("encoding", Encoding);
            }
        }
    }
}
