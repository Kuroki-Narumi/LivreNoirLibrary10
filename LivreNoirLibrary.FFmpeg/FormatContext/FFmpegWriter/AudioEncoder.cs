using System;
using System.IO;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public sealed unsafe class AudioEncoder : FFmpegWriter, IAudioEncoder
    {
        private readonly AudioEncodeContext _context;
        private readonly StreamInfo _streamInfo;

        public int InputSampleRate => _context.InputSampleRate;
        public int InputChannels => _context.InputChannels;
        public int OutputSampleRate => _context.OutputSampleRate;
        public int OutputChannels => _context.OutputChannels;
        public StreamInfo StreamInfo => _streamInfo;

        public AudioEncoder(string path, AudioEncodeOptions options) : this(General.CreateSafe(path), OutputFormat.ByFilename(path), options, false) { }
        public AudioEncoder(Stream stream, OutputFormat format, AudioEncodeOptions options, bool leaveOpen = true) : base(stream, format, leaveOpen)
        {
            AudioEncodeContext ctx = new(this);
            ctx.Setup(format.AudioCodec, options);
            _context = ctx;
            _streamInfo = new(ctx._stream);
        }

        public void Write(ReadOnlySpan<float> buffer)
        {
            WriteHeader();
            _context.Write(buffer);
        }

        protected override void BeforeFlush()
        {
            WriteHeader();
            _context.Flush();
        }

        public void Flush() => WriteTrailer();
    }
}
