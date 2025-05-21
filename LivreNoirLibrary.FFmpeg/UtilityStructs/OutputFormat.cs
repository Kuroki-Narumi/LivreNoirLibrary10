
using LivreNoirLibrary.Text;
using System.Text.Json;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public readonly unsafe struct OutputFormat : IJsonWriter
    {
        static OutputFormat()
        {
            FFmpegUtils.Initialize();
        }

        public static OutputFormat ByFormat(string formatString) => new(FFmpegUtils.ConvertToOutFormatName(formatString), null, null);
        public static OutputFormat ByFilename(string filename) => new(null, filename, null);
        public static OutputFormat ByMimeType(string mimeType) => new(null, null, mimeType);
        public static OutputFormat ByInputFormat(InputFormat input) => new(FFmpegUtils.ConvertToOutFormatName(input.Name));
        internal static OutputFormat ByInputFormat(AVInputFormat* input) => new(FFmpegUtils.ConvertToOutFormatName(FFmpegUtils.GetName(input->name)));

        internal readonly AVOutputFormat* _format;

        internal OutputFormat(AVOutputFormat* format)
        {
            _format = format;
        }

        public OutputFormat(string? formatString, string? filename = null, string? mimeType = null) : this(ffmpeg.av_guess_format(formatString, filename, mimeType)) { }

        public bool IsValid => _format is not null;
        public string Name => IsValid ? FFmpegUtils.GetName(_format->name) : FFmpegUtils.NA;
        public string LongName => IsValid ? FFmpegUtils.GetName(_format->long_name) : FFmpegUtils.NA;
        public string MimeType => IsValid ? FFmpegUtils.GetName(_format->mime_type) : FFmpegUtils.NA;
        public AVCodecID AudioCodec => IsValid ? _format->audio_codec : 0;
        public AVCodecID VideoCodec => IsValid ? _format->video_codec : 0;
        public AVCodecID SubtitleCodec => IsValid ? _format->subtitle_codec : 0;
        public FormatFlags Flags => IsValid ? (FormatFlags)_format->flags : 0;

        public override string ToString() => 
            $"{nameof(OutputFormat)}{{Name={Name}, LongName={LongName}, MimeType={MimeType}, AudioCodec={AudioCodec}, VideoCodec={VideoCodec}, SubtitleCodec={SubtitleCodec}, Flags={Flags}}}";

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
