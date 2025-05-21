using System.Text.Json;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public readonly unsafe struct InputFormat : IJsonWriter
    {
        static InputFormat()
        {
            FFmpegUtils.Initialize();
        }

        internal readonly AVInputFormat* _format;

        internal InputFormat(AVInputFormat* format)
        {
            _format = format;
        }

        public string Name => FFmpegUtils.GetName(_format->name);
        public string LongName => FFmpegUtils.GetName(_format->long_name);
        public string MimeType => FFmpegUtils.GetName(_format->mime_type);
        public FormatFlags Flags => (FormatFlags)_format->flags;

        public override string ToString() => $"{nameof(OutputFormat)}{{Name={Name}, LongName={LongName}, MimeType={MimeType}, Flags={Flags}}}";

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("name", Name);
            writer.WriteString("long name", LongName);
            writer.WriteString("mime type", MimeType);
            writer.WriteEndObject();
        }
    }
}
