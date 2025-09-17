using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media.FFmpeg;

namespace LivreNoirLibrary.Media.Wave
{
    public class GenericWaveBuffer : WaveBuffer
    {
        public OutputFormat OutputFormat { get; set; }
        public long Bitrate { get; set; }
        public List<MetaTag> FormatMetaTags { get; } = [];
        public List<MetaTag> StreamMetaTags { get; } = [];

        public override void Clear()
        {
            base.Clear();
            FormatMetaTags.Clear();
            StreamMetaTags.Clear();
        }

        protected override void LoadMetaData(object source)
        {
            base.LoadMetaData(source);
            if (source is AudioDecoder c)
            {
                c.BaseContext.GetMetaTags(FormatMetaTags);
                OutputFormat = c.GetOutputFormat();
                Bitrate = c.Bitrate;
            }
            if (source is IMetaTag meta)
            {
                meta.GetMetaTags(StreamMetaTags);
            }
        }

        public void Save(string path) => General.Save(path, Dump, null, "");
        public void Dump(Stream stream)
        {
            using AudioEncoder encoder = new(stream, OutputFormat, new(SampleRate, Channels, Bitrate));
            WriteMetaTags(encoder, encoder.StreamInfo);
            encoder.Write(Data);
            encoder.Flush();
        }

        public override void WriteMetaTags(IMetaTag format, IMetaTag stream)
        {
            format.SetMetaTags(FormatMetaTags);
            stream.SetMetaTags(StreamMetaTags);
        }

        protected override void WriteJsonMetaData(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WritePropertyName("out format");
            OutputFormat.WriteJson(writer, options);

            writer.WriteNumber("bitrate(kbps)", Bitrate / 1000.0);

            writer.WritePropertyName("format meta tags");
            writer.WriteStartArray();
            foreach (var tag in CollectionsMarshal.AsSpan(FormatMetaTags))
            {
                writer.WriteStringValue(tag.ToString());
            }
            writer.WriteEndArray();

            writer.WritePropertyName("stream meta tags");
            writer.WriteStartArray();
            foreach (var tag in CollectionsMarshal.AsSpan(StreamMetaTags))
            {
                writer.WriteStringValue(tag.ToString());
            }
            writer.WriteEndArray();
        }
    }
}
