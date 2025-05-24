using System;
using System.Collections.Generic;
using System.IO;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Text;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media.FFmpeg;

namespace LivreNoirLibrary.Media.Wave
{
    public unsafe class WaveEncoder : WaveContext, IAudioEncoder, IAudioBufferEncoder, IMediaEncoder
    {
        private BinaryWriter? _writer;

        private bool _header_wrote;
        /// <summary>
        /// Byte offset where the Riff data begins.
        /// </summary>
        private long _header_position;
        private long _fact_position = -1;
        private bool _flushed;

        /// <inheritdoc cref="IAudioBuffer.BufferLength"/>
        private int _buffer_length;
        /// <inheritdoc cref="IAudioBuffer.BufferIndex"/>
        private int _buffer_wrote;

        Span<float> IAudioBuffer.Buffer => Buffer;
        int IAudioBuffer.BufferLength => _buffer_length;
        int IAudioBuffer.BufferIndex { get => _buffer_wrote; set => _buffer_wrote = value; }

        public WaveEncoder(string path, WaveEncodeOptions options) : this(General.CreateSafe(path), options, false) { }
        public WaveEncoder(Stream stream, WaveEncodeOptions options, bool leaveOpen = true)
        {
            SetStream(stream, leaveOpen);
            Setup(stream, options);
        }

        private void Setup(Stream stream, WaveEncodeOptions options)
        {
            _writer?.Dispose();
            options.Validate();
            _writer = new(stream, Encoding.UTF8, true);
            var (inRate, inChannels, outRate, outChannels, format) = options;
            _in_rate = inRate;
            _in_channels = inChannels;
            _out_rate = outRate;
            _out_channels = outChannels;
            _format = FormatChunk.Create(outRate, outChannels, format);
            var inFrameSize = BufferSize / FormatChunk.CalculateBlockAlign(inChannels, SampleFormat.Float32);
            EnsureBufferSize(inFrameSize * inChannels);
            _buffer_length = inFrameSize;
            _buffer_wrote = 0;
            if (outRate != inRate || outChannels != inChannels)
            {
                AVChannelLayout inCh = default;
                AVChannelLayout outCh = default;
                ffmpeg.av_channel_layout_default(&inCh, inChannels);
                ffmpeg.av_channel_layout_default(&outCh, outChannels);
                this.AllocSwrContext(outCh, InternalSampleFormat, outRate, inCh, InternalSampleFormat, inRate);
            }
            _converter = IWaveSampleConverter.GetConverter(format);
        }

        public void WriteHeader()
        {
            if (_header_wrote || _writer is not BinaryWriter writer)
            {
                return;
            }
            var stream = writer.BaseStream;
            FourLetterHeader.Write(writer, ChunkIds.RiffHeader);
            writer.Write(0u);
            _header_position = stream.Position;
            FourLetterHeader.Write(writer, ChunkIds.DataHeader);
            _format.Dump(writer);
            if (this.TryGetChunk<Chunks.Fact>(out var fact))
            {
                _fact_position = fact.Dump(writer);
            }
            FourLetterHeader.Write(writer, ChunkIds.Data);
            writer.Write(0u);
            _data_position = stream.Position;
            _header_wrote = true;
        }

        public void Flush()
        {
            if (_flushed || _writer is not BinaryWriter writer)
            {
                return;
            }
            WriteHeader();
            this.FlushCore();
            var stream = writer.BaseStream;
            // Dataチャンクのサイズ
            var pos = stream.Position;
            var dataLength = pos - _data_position;
            stream.Position = _data_position - sizeof(uint);
            writer.Write((uint)dataLength);
            if (_fact_position is >= 0)
            {
                stream.Position = _fact_position;
                writer.Write((uint)(dataLength / _format.BlockAlign));
            }
            stream.Position = pos;
            // パディング
            if (dataLength % 2 is 1)
            {
                writer.Write((byte)0);
            }
            // 残りのチャンク
            foreach (var chunk in CollectionsMarshal.AsSpan(_chunks))
            {
                if (chunk is Chunks.Fact)
                {
                    continue;
                }
                if (chunk is Chunks.Acid acid)
                {
                    acid.SetTempo(acid.Tempo, (double)dataLength / _format.BytesPerSecond);
                }
                writer.WriteRiffChunk(chunk);
            }
            // 総データサイズ
            pos = stream.Position;
            dataLength = pos - _header_position;
            stream.Position = _header_position - sizeof(uint);
            writer.Write((uint)dataLength);
            stream.Position = pos;
            _flushed = true;
        }
        public void WriteTrailer() => Flush();

        public void Write(ReadOnlySpan<float> buffer)
        {
            if (_flushed)
            {
                FFmpegUtils.ThrowInvalidOperationException("stream is alreadey flushd.");
            }
            WriteHeader();
            this.WriteCore(buffer);
        }

        /// <inheritdoc cref="IAudioBufferEncoder.EncodeBuffer"/>
        private bool EncodeBuffer()
        {
            if (_converter is not IWaveSampleConverter converter || BaseStream is not Stream stream)
            {
                return false;
            }
            return _swr_context is not null ? EncodeBuffer_SwrConvert(converter, stream)
                                            : EncodeBuffer_NoConvert(converter, stream);
        }
        bool IAudioBufferEncoder.EncodeBuffer() => EncodeBuffer();

        /// <inheritdoc cref="IAudioBufferEncoder.EncodeBuffer"/>
        private bool EncodeBuffer_NoConvert(IWaveSampleConverter converter, Stream stream)
        {
            var totalSamples = _buffer_wrote;
            if (totalSamples is <= 0)
            {
                return false;
            }
            var bytesToWrite = totalSamples * converter.BytesPerSample;
            var streamBuffer = ArrayPool<byte>.Shared.Rent(bytesToWrite);
            try
            {
                fixed (byte* dstPtr = streamBuffer)
                fixed (float* srcPtr = Buffer)
                {
                    converter.ConvertWrite(srcPtr, dstPtr, totalSamples);
                }
                stream.Write(streamBuffer, 0, bytesToWrite);
                return true;
            }
            finally
            {
                _buffer_wrote = 0;
                ArrayPool<byte>.Shared.Return(streamBuffer);
            }
        }

        /// <inheritdoc cref="IAudioBufferEncoder.EncodeBuffer"/>
        private bool EncodeBuffer_SwrConvert(IWaveSampleConverter converter, Stream stream)
        {
            var convertSize = _buffer_length * _out_rate / _in_rate + 1;
            var streamBuffer = ArrayPool<byte>.Shared.Rent(BufferSize);
            var convertBuffer = ArrayPool<float>.Shared.Rent(convertSize * _out_channels);
            try
            {
                fixed (byte* dstPtr = streamBuffer)
                fixed (float* convPtr = convertBuffer)
                {
                    var convBytePtr = (byte*)convPtr;
                    var span = Buffer[.._buffer_wrote];
                    var samples = _buffer_wrote / _in_channels;
                    var outSamples = this.SwrConvertToWrite(span, samples, &convBytePtr, convertSize);
                    if (outSamples is <= 0)
                    {
                        return false;
                    }
                    var totalSamples = outSamples * _out_channels;
                    converter.ConvertWrite(convPtr, dstPtr, totalSamples);
                    stream.Write(streamBuffer, 0, totalSamples * converter.BytesPerSample);
                    return true;
                }
            }
            finally
            {
                _buffer_wrote = 0;
                ArrayPool<float>.Shared.Return(convertBuffer);
                ArrayPool<byte>.Shared.Return(streamBuffer);
            }
        }

        protected override void DisposeManaged()
        {
            _writer?.Dispose();
            base.DisposeManaged();
        }

        protected override void DisposeUnmanaged()
        {
            Flush();
            base.DisposeUnmanaged();
        }
    }
}
