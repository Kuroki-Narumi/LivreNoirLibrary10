using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public unsafe partial class FFmpegEncoder(Stream stream, OutputFormat format, bool leaveOpen) : FFmpegWriter(stream, format, leaveOpen), IMediaEncoder
    {
        internal List<VideoEncodeContext> _video_streams = [];
        internal List<AudioEncodeContext> _audio_streams = [];

        public FFmpegEncoder(string path) : this(General.CreateSafe(path), OutputFormat.ByFilename(path), false) { }

        public StreamInfo CreateVideoStream(VideoEncodeOptions options)
        {
            VideoEncodeContext ctx = new(this);
            ctx.Setup(options);
            var stream = ctx._stream;
            _video_streams.Add(ctx);
            return new(stream);
        }

        public StreamInfo CreateAudioStream(AVCodecID codec, AudioEncodeOptions options)
        {
            if (codec is AVCodecID.AV_CODEC_ID_NONE)
            {
                codec = _out_format->audio_codec;
            }
            AudioEncodeContext ctx = new(this);
            ctx.Setup(codec, options);
            var stream = ctx._stream;
            _audio_streams.Add(ctx);
            return new(stream);
        }

        /// <inheritdoc cref="IVideoEncoder.Write(ReadOnlySpan{byte}, Rational)"/>
        /// <param name="streamIndex">Target stream index. If &lt; 0, find the first video stream.</param>
        public void WritePixels(ReadOnlySpan<byte> buffer, Rational duration = default, int streamIndex = -1)
        {
            if (_video_streams.Count is <= 0)
            {
                FFmpegUtils.ThrowIndexOutOfRangeException($"No video streams exists.");
            }
            var context = streamIndex is < 0 ? _video_streams[0] : _video_streams.Find(s => s._stream->index == streamIndex);
            if (context is null)
            {
                FFmpegUtils.ThrowIndexOutOfRangeException($"Stream index {streamIndex} is not an video stream.");
            }
            WriteHeader();
            context?.Write(buffer, duration);
        }

        /// <inheritdoc cref="IAudioEncoder.Write"/>
        /// <param name="streamIndex">Target stream index. If &lt; 0, find the first video stream.</param>
        public void WriteSamples(ReadOnlySpan<float> buffer, int streamIndex = -1)
        {
            if (_audio_streams.Count is <= 0)
            {
                FFmpegUtils.ThrowIndexOutOfRangeException($"No audio streams exists.");
            }
            var context = streamIndex is < 0 ? _audio_streams[0] : _audio_streams.Find(s => s._stream->index == streamIndex);
            if (context is null)
            {
                FFmpegUtils.ThrowIndexOutOfRangeException($"Stream index {streamIndex} is not an audio stream.");
            }
            WriteHeader();
            context?.Write(buffer);
        }

        protected override void BeforeFlush()
        {
            foreach (var context in CollectionsMarshal.AsSpan(_video_streams))
            {
                context.Flush();
                context.Dispose();
            }
            _video_streams.Clear();

            foreach (var context in CollectionsMarshal.AsSpan(_audio_streams))
            {
                context.Flush();
                context.Dispose();
            }
            _audio_streams.Clear();
        }
    }
}
