using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media.FFmpeg;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Wave
{
    public sealed unsafe class WaveDecoder : WaveContext, IAudioDecodeContext, IAudioBufferDecoder
    {
        /// <summary>
        /// Byte length of the data chunk.
        /// </summary>
        private uint _data_length;
        /// <inheritdoc cref="IAudioDecoder.SampleLength"/>
        private long _out_length;
        /// <inheritdoc cref="IAudioBufferInternal.BufferHead"/>
        private long _buffer_head;
        /// <inheritdoc cref="IAudioBufferInternal.BufferLength"/>
        private int _buffer_length;
        /// <inheritdoc cref="IAudioBufferInternal.BufferIndex"/>
        private int _buffer_read;

        public long SampleLength => _out_length;
        public Rational Duration => new(_out_length, OutputSampleRate);

        float* IAudioBufferInternal.BufferPointer => _buffer.Pointer;
        long IAudioBufferDecoder.BufferHead => _buffer_head;
        int IAudioBufferInternal.BufferLength => _buffer_length;
        int IAudioBufferInternal.BufferIndex { get => _buffer_read; set => _buffer_read = value; }

        public SampleFormat SampleFormat => _format.TryGetSampleFormat(out var format) ? format : SampleFormat.Invalid;

        public long SamplePosition
        {
            get => _buffer_head + _buffer_read / OutputChannels;
            set => this.SampleSeekCore(value);
        }

        public Rational Position
        {
            get => new(SamplePosition, OutputSampleRate);
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
            var inSampleRate = InputSampleRate;
            var inChannels = InputChannels;
            if (outSampleRate is <= 0)
            {
                outSampleRate = inSampleRate;
            }
            if (outChannels is <= 0)
            {
                outChannels = inChannels;
            }
            OutputSampleRate = outSampleRate;
            OutputChannels = outChannels;
            _out_length = _data_length / _format.BlockAlign;
            SetStream(stream, leaveOpen);
            stream.Position = _data_position;
            _buffer_head = 0;
            _buffer_length = 0;
            if (outSampleRate != inSampleRate || outChannels != inChannels)
            {
                _swrContext = FFmpegUtils.CreateSwrContext(
                    outChannels, OutputSampleFormat, outSampleRate,
                    inChannels, InputSampleFormat, inSampleRate);
                _out_length = ffmpeg.av_rescale_rnd(_out_length, outSampleRate, inSampleRate, AVRounding.AV_ROUND_UP);
            }
            EnsureBufferSize((int)((long)BufferSize / _converter.BytesPerSample * outChannels / inChannels * outSampleRate / inSampleRate));
            return true;
        }
        bool IAudioBufferDecoder.Setup(Stream stream, bool leaveOpen, int streamIndex, int outSampleRate, int outChannels) => Setup(stream, leaveOpen, outSampleRate, outChannels);

        private void ReadInfo(Stream stream, BinaryReader reader)
        {
            var info = WaveInfo.Create(stream, reader);
            _data_position = info.DataPosition;
            _data_length = info.DataLength;
            var format = info.Format;
            InputSampleRate = (int)format.SampleRate;
            InputChannels = format.Channels;
            _format = format;
            _converter = format.TryGetSampleFormat(out var fmt) ? IWaveSampleConverter.GetConverter(fmt) : null;
            _chunks.Clear();
            _chunks.AddRange(info.Chunks);
        }

        public void SampleSeek(long position) => this.SampleSeekCore(position);
        public void Seek(Rational position) => this.SeekCore(position);

        void IAudioBufferDecoder.UnsafeSampleSeek(long position)
        {
            if (BaseStream is not Stream stream)
            {
                return;
            }
            var index = position * InputSampleRate / OutputSampleRate * _format.BlockAlign;
            stream.Position = _data_position + index;
            if (_swrContext is not null)
            {
                ffmpeg.swr_init(_swrContext);
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
            return _swrContext is not null ? UpdateBuffer_SwrConvert(converter, stream, streamIndex, streamRemain)
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
            using var o = ArrayPool.Rent<byte>(bytesToRead);
            var streamBuffer = o.Array;
            bytesToRead = stream.Read(streamBuffer, 0, bytesToRead);
            if (bytesToRead is 0)
            {
                return true;
            }
            var destPtr = _buffer.Pointer;
            fixed (byte* srcPtr = streamBuffer)
            {
                converter.ConvertRead(srcPtr, destPtr, bytesToRead);
            }
            _buffer_length = bytesToRead / block;
            return false;
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
            using var o1 = ArrayPool.Rent<byte>(bytesToRead);
            using var o2 = ArrayPool.Rent<float>(bytesToRead / converter.BytesPerSample);
            var streamBuffer = o1.Array;
            var convertBuffer = o2.Array;
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
    }
}
