using System;
using System.Buffers;
using System.Text;
using System.Collections.Generic;
using System.IO;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Media.FFmpeg;
using LivreNoirLibrary.Debug;

namespace LivreNoirLibrary.Media.Wave
{
    public sealed unsafe class WaveDecoder : WaveContext, IAudioDecoder, IAudioBufferDecoder
    {
        /// <summary>
        /// Byte length of the data chunk.
        /// </summary>
        private uint _data_length;
        /// <inheritdoc cref="IAudioDecoder.SampleLength"/>
        private long _out_length;
        /// <inheritdoc cref="IAudioBuffer.BufferHead"/>
        private long _buffer_head;
        /// <inheritdoc cref="IAudioBuffer.BufferLength"/>
        private int _buffer_length;
        /// <inheritdoc cref="IAudioBuffer.BufferIndex"/>
        private int _buffer_read;

        public long SampleLength => _out_length;
        public Rational Duration => new(_out_length, _out_rate);

        Span<float> IAudioBuffer.Buffer => Buffer;
        long IAudioBufferDecoder.BufferHead => _buffer_head;
        int IAudioBuffer.BufferLength => _buffer_length;
        int IAudioBuffer.BufferIndex { get => _buffer_read; set => _buffer_read = value; }

        public SampleFormat SampleFormat => _format.TryGetSampleFormat(out var format) ? format : SampleFormat.Invalid;

        public long SamplePosition
        {
            get => _buffer_head + _buffer_read / _out_channels;
            set => this.SampleSeekCore(value);
        }

        public Rational Position
        {
            get => new(SamplePosition, _out_rate);
            set => this.SeekCore(value);
        }

        public WaveDecoder(string path, int outSampleRate = 0, int outChannels = 0)
        {
            this.Setup(path, -1, outSampleRate, outChannels);
        }

        public WaveDecoder(Stream stream, bool leaveOpen = true, int outSampleRate = 0, int outChannels = 0)
        {
            Setup(stream, leaveOpen, outSampleRate, outChannels);
        }

        private bool Setup(Stream stream, bool leaveOpen, int outSampleRate, int outChannels)
        {
            var pos = stream.Position;
            using BinaryReader reader = new(stream, Encoding.UTF8, true);
            {
                ReadInfo(stream, reader);
            }
            if (_converter is null)
            {
                stream.Position = pos;
                SetStream(null, false);
                FFmpegUtils.ThrowNotSupportedException("Sample converter not found.");
                return false;
            }
            var inSampleRate = _in_rate;
            var inChannels = _in_channels;
            if (outSampleRate is <= 0)
            {
                outSampleRate = inSampleRate;
            }
            if (outChannels is <= 0)
            {
                outChannels = inChannels;
            }
            _out_rate = outSampleRate;
            _out_channels = outChannels;
            _out_length = _data_length / _format.BlockAlign;
            SetStream(stream, leaveOpen);
            stream.Position = _data_position;
            _buffer_head = 0;
            _buffer_length = 0;
            if (outSampleRate != inSampleRate || outChannels != inChannels)
            {
                AVChannelLayout inCh = default;
                AVChannelLayout outCh = default;
                ffmpeg.av_channel_layout_default(&inCh, inChannels);
                ffmpeg.av_channel_layout_default(&outCh, outChannels);
                this.AllocSwrContext(outCh, InternalSampleFormat, outSampleRate, inCh, InternalSampleFormat, inSampleRate);
                _out_length = ffmpeg.av_rescale_rnd(_out_length, outSampleRate, inSampleRate, AVRounding.AV_ROUND_UP);
            }
            EnsureBufferSize(BufferSize / _converter.BytesPerSample * outChannels / inChannels * outSampleRate / inSampleRate);
            return true;
        }
        bool IAudioBufferDecoder.Setup(Stream stream, bool leaveOpen, int streamIndex, int outSampleRate, int outChannels) => Setup(stream, leaveOpen, outSampleRate, outChannels);

        private void ReadInfo(Stream stream, BinaryReader reader)
        {
            FourLetterHeader.CheckAndThrow(reader, ChunkIds.RiffHeader);
            var length = (long)reader.ReadUInt32();
            var endPos = stream.Position + length;
            FourLetterHeader.CheckAndThrow(reader, ChunkIds.DataHeader);
            var list = _chunks;
            list.Clear();
            while (stream.Position < endPos)
            {
                var chid = FourLetterHeader.Read(reader);
                if (chid is ChunkIds.Data)
                {
                    (_data_position, _data_length) = reader.ReadRiffChunk<DataChunk>();
                }
                else if (chid is ChunkIds.Format)
                {
                    var format = reader.ReadRiffChunk<FormatChunk>();
                    _in_rate = (int)format.SampleRate;
                    _in_channels = format.Channels;
                    _format = format;
                    _converter = format.TryGetSampleFormat(out var fmt) ? IWaveSampleConverter.GetConverter(fmt) : null;
                }
                else
                {
                    var chunk = RiffChunk.Create(chid, reader);
                    list.Add(chunk);
                }
            }
        }

        public void SampleSeek(long position) => this.SampleSeekCore(position);
        public void Seek(Rational position) => this.SeekCore(position);

        void IAudioBufferDecoder.UnsafeSampleSeek(long position)
        {
            if (BaseStream is not Stream stream)
            {
                return;
            }
            var index = position * _in_rate / _out_rate * _format.BlockAlign;
            stream.Position = _data_position + index;
            if (_swr_context is not null)
            {
                ffmpeg.swr_init(_swr_context);
            }
            _buffer_head = position;
            _buffer_length = 0;
        }

        public int Read(Span<float> buffer) => this.ReadCore(buffer);

        /// <inheritdoc cref="IAudioBufferDecoder.UpdateBuffer"/>
        private bool UpdateBuffer()
        {
            if (_converter is not IWaveSampleConverter converter || BaseStream is not Stream stream)
            {
                _buffer_length = 0;
                return true;
            }
            var streamIndex = stream.Position - _data_position;
            var streamRemain = (int)(_data_length - streamIndex);
            return _swr_context is not null ? UpdateBuffer_SwrConvert(converter, stream, streamIndex, streamRemain)
                                            : UpdateBuffer_NoConvert(converter, stream, streamIndex, streamRemain);
        }
        bool IAudioBufferDecoder.UpdateBuffer() => UpdateBuffer();

        /// <inheritdoc cref="IAudioBufferDecoder.UpdateBuffer"/>
        private bool UpdateBuffer_NoConvert(IWaveSampleConverter converter, Stream stream, long streamIndex, int streamRemain)
        {
            var block = _format.BlockAlign;
            _buffer_head = streamIndex / block;
            _buffer_length = 0;
            if (streamRemain is <= 0)
            {
                return true;
            }
            var bytesToRead = Math.Min(BufferSize / block * block, streamRemain);
            var streamBuffer = ArrayPool<byte>.Shared.Rent(bytesToRead);
            try
            {
                bytesToRead = stream.Read(streamBuffer, 0, bytesToRead);
                if (bytesToRead is 0)
                {
                    return true;
                }
                fixed (byte* srcPtr = streamBuffer)
                fixed (float* dstPtr = Buffer)
                {
                    converter.ConvertRead(srcPtr, dstPtr, bytesToRead);
                }
                _buffer_length = bytesToRead / block;
                return false;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(streamBuffer);
            }
        }

        /// <inheritdoc cref="IAudioBufferDecoder.UpdateBuffer"/>
        private bool UpdateBuffer_SwrConvert(IWaveSampleConverter converter, Stream stream, long streamIndex, int streamRemain)
        {
            if (streamRemain is <= 0)
            {
                var rest = this.SwrConvertToRead(null, 0);
                _buffer_head = _out_length - rest;
                _buffer_length = rest;
                return rest is <= 0;
            }
            _buffer_head += _buffer_length;
            _buffer_length = 0;
            var block = _format.BlockAlign;
            var bytesToRead = Math.Min(BufferSize / block * block, streamRemain);
            var streamBuffer = ArrayPool<byte>.Shared.Rent(bytesToRead);
            var convertBuffer = ArrayPool<float>.Shared.Rent(bytesToRead / converter.BytesPerSample);
            try
            {
                bytesToRead = stream.Read(streamBuffer, 0, bytesToRead);
                if (bytesToRead is 0)
                {
                    return true;
                }
                fixed (byte* srcPtr = streamBuffer)
                fixed (float* convPtr = convertBuffer)
                {
                    converter.ConvertRead(srcPtr, convPtr, bytesToRead);
                    var convBytePtr = (byte*)convPtr;
                    var outSamples = this.SwrConvertToRead(&convBytePtr, bytesToRead / block);
                    _buffer_length = outSamples;
                    return false;
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(streamBuffer);
                ArrayPool<float>.Shared.Return(convertBuffer);
            }
        }

        public static bool IsSupported(string path) => IsSupported(File.OpenRead(path), false);
        public static bool IsSupported(Stream stream, bool leaveOpen = true)
        {
            using BinaryReader reader = new(stream, Encoding.UTF8, leaveOpen);
            return IsSupported(stream, reader);
        }

        private static bool IsSupported(Stream stream, BinaryReader reader)
        {
            var pos = stream.Position;
            try
            {
                if (!FourLetterHeader.Check(reader, ChunkIds.RiffHeader))
                {
                    return false;
                }
                var length = (long)reader.ReadUInt32();
                var endPos = stream.Position + length;
                if (!FourLetterHeader.Check(reader, ChunkIds.DataHeader))
                {
                    return false;
                }
                while (stream.Position < endPos)
                {
                    var chid = FourLetterHeader.Read(reader);
                    if (chid is ChunkIds.Format)
                    {
                        var format = reader.ReadRiffChunk<FormatChunk>();
                        return format.TryGetSampleFormat(out _);
                    }
                    length = reader.ReadUInt32();
                    stream.Position += length + (length % 2 is 1 ? 1 : 0);
                }
                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                stream.Position = pos;
            }
        }
    }
}
