using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media.FFmpeg;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Wave
{
    public abstract unsafe partial class WaveContext : DisposableBase, IAudioContext, ISwrContext
    {
        public const AVSampleFormat InternalSampleFormat = AVSampleFormat.AV_SAMPLE_FMT_FLT;
        public const int BufferSize = 32768;

        private Stream? _stream;
        private bool _leave_open;
        /// <summary>
        /// Byte offset where the data chunk begins.
        /// </summary>
        protected long _data_position;

        internal IWaveSampleConverter? _converter;
        protected FormatChunk _format;
        protected readonly List<RiffChunk> _chunks = [];

        protected int _in_rate;
        protected int _in_channels;
        protected int _out_rate;
        protected int _out_channels;
        protected SwrContext* _swr_context;
        private readonly UnmanagedArray<float> _buffer = new();

        public Stream BaseStream => _stream!;
        public int InputSampleRate => _in_rate;
        public int InputChannels => _in_channels;
        public int OutputSampleRate => _out_rate;
        public int OutputChannels => _out_channels;

        internal Span<float> Buffer => _buffer;
        SwrContext* ISwrContext.SwrContext { get => _swr_context; set => _swr_context = value; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Span<float> EnsureBufferSize(int size)
        {
            _buffer.EnsureSize(size);
            return _buffer;
        }
        Span<float> ISwrContext.GetConvertBuffer(int samplePerChannel) => EnsureBufferSize(samplePerChannel * _out_channels);

        protected void SetStream(Stream? stream, bool leaveOpen)
        {
            DisposeStream();
            _stream = stream;
            _leave_open = leaveOpen;
        }

        private void DisposeStream()
        {
            if (_stream is not null && !_leave_open)
            {
                _stream.Dispose();
                _stream = null;
            }
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            DisposeStream();
        }

        protected override void DisposeUnmanaged()
        {
            _buffer.Dispose();
            this.DisposeSwrContext();
            base.DisposeUnmanaged();
        }
    }
}
