using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public abstract unsafe class AudioContext(FormatContext baseContext) : CodecContext(baseContext), ISwrContext, IAudioContext
    {
        public const AVSampleFormat InternalSampleFormat = AVSampleFormat.AV_SAMPLE_FMT_FLT;

        protected int _in_rate;
        protected int _in_channels;
        protected int _out_rate;
        protected int _out_channels;

        protected SwrContext* _swr_context;
        private readonly UnmanagedArray<float> _buffer = new();

        public int InputSampleRate => _in_rate;
        public int InputChannels => _in_channels;
        public int OutputSampleRate => _out_rate;
        public int OutputChannels => _out_channels;

        SwrContext* ISwrContext.SwrContext { get => _swr_context; set => _swr_context = value; }
        internal Span<float> Buffer => _buffer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Span<float> EnsureBufferSize(int size)
        {
            _buffer.EnsureSize(size);
            return _buffer;
        }
        Span<float> ISwrContext.GetConvertBuffer(int samplePerChannel) => EnsureBufferSize(samplePerChannel * _out_channels);

        protected override void DisposeUnmanaged()
        {
            this.DisposeSwrContext();
            _buffer.Dispose();
            base.DisposeUnmanaged();
        }
    }
}
