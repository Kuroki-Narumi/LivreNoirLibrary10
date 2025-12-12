using System;
using System.Runtime.CompilerServices;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public abstract unsafe class AudioContext(FormatContext baseContext) : CodecContext(baseContext), ISwrContext, IAudioContext
    {
        public const AVSampleFormat InternalSampleFormat = AVSampleFormat.AV_SAMPLE_FMT_FLT;

        protected readonly UnmanagedArray<float> _buffer = new();
        protected SwrContext* _swrContext;

        public int InputSampleRate { get; protected set; }
        public AVSampleFormat InputSampleFormat { get; protected set; }
        public int InputChannels { get; protected set; }
        public int OutputSampleRate { get; protected set; }
        public AVSampleFormat OutputSampleFormat { get; protected set; }
        public int OutputChannels { get; protected set; }

        SwrContext* ISwrContext.SwrContext { get => _swrContext; set => _swrContext = value; }
        float* ISwrContext.GetConvertBuffer(int samplePerChannel) => EnsureBufferSize(samplePerChannel * OutputChannels);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected float* EnsureBufferSize(int size)
        {
            _buffer.EnsureSize(size);
            return _buffer.Pointer;
        }

        protected override void DisposeManaged()
        {
            _buffer.Dispose();
            base.DisposeManaged();
        }

        protected override void DisposeUnmanaged()
        {
            this.DisposeSwrContext();
            base.DisposeUnmanaged();
        }
    }
}
