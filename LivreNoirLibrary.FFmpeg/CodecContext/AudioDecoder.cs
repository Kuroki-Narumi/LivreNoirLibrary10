using System;
using FFmpeg.AutoGen;
using System.IO;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public unsafe class AudioDecoder : AudioContext, IAudioDecoder, IAudioBufferDecoder
    {
        /// <inheritdoc cref="IAudioDecoder.SampleLength"/>
        private long _out_length;
        /// <inheritdoc cref="IAudioBufferDecoder.BufferHead"/>
        private long _buffer_head;
        /// <inheritdoc cref="IAudioBuffer.BufferLength"/>
        private int _buffer_length;
        /// <inheritdoc cref="IAudioBuffer.BufferIndex"/>
        private int _buffer_read;
        /// <summary>
        /// The pts(samples per channel) of the first packet in the stream.
        /// </summary>
        private long _pts_offset;

        public long SampleLength => _out_length;
        public Rational Duration { get; private set; }

        Span<float> IAudioBuffer.Buffer => Buffer;
        long IAudioBufferDecoder.BufferHead => _buffer_head;
        int IAudioBuffer.BufferLength => _buffer_length;
        int IAudioBuffer.BufferIndex { get => _buffer_read; set => _buffer_read = value; }

        public long SamplePosition
        {
            get => _buffer_head - _pts_offset + _buffer_read / _out_channels;
            set => this.SampleSeekCore(value);
        }

        /// <summary>
        /// The position(in seconds) of the frame that the decoder will return by reading method.
        /// </summary>
        public Rational Position
        {
            get => new(SamplePosition, _out_rate);
            set => this.SeekCore(value);
        }

        public AudioDecoder(string path, AudioDecodeOptions options = default) : base(new())
        {
            this.Setup(path, options.StreamIndex, options.SampleRate, options.Channels);
        }

        public AudioDecoder(Stream stream, bool leaveOpen = true, AudioDecodeOptions options = default) : base(new())
        {
            Setup(stream, leaveOpen, options.StreamIndex, options.SampleRate, options.Channels);
        }

        public OutputFormat GetOutputFormat() => OutputFormat.ByInputFormat(_base_context._format_context->iformat);

        protected override void DisposeUnmanaged()
        {
            base.DisposeUnmanaged();
            _base_context.Dispose();
        }

        private bool Setup(Stream stream, bool leaveOpen, int streamIndex, int outSampleRate, int outChannels)
        {
            var baseContext = _base_context;
            baseContext.OpenRead(stream, leaveOpen);
            if (!baseContext.TryGetStream(out var avs, AVMediaType.AVMEDIA_TYPE_AUDIO, streamIndex))
            {
                FFmpegUtils.ThrowInvalidDataException("No audio stream found.");
                return false;
            }
            FFmpegUtils.SetupDecoder(avs, false, out var context);
            _stream = avs;
            _codec_context = context;
            // サンプルレートとチャンネル数
            var inSampleRate = _in_rate = context->sample_rate;
            if (outSampleRate is <= 0)
            {
                outSampleRate = inSampleRate;
            }
            var inChannel = context->ch_layout;
            _in_channels = inChannel.nb_channels;
            if (outChannels is <= 0)
            {
                outChannels = _in_channels;
            }
            _out_rate = outSampleRate;
            _out_channels = outChannels;
            Duration = FFmpegUtils.GetDuration(avs, _base_context._format_context);
            _out_length = Duration.Ceiling(_out_rate);
            _buffer_head = 0;
            _buffer_length = 0;
            // サンプル変換コンテキスト
            AVChannelLayout dstChannel = default;
            ffmpeg.av_channel_layout_default(&dstChannel, outChannels);
            this.AllocSwrContext(dstChannel, InternalSampleFormat, outSampleRate, inChannel, context->sample_fmt, inSampleRate);
            UpdateBuffer();
            _pts_offset = _buffer_head;
            return true;
        }
        bool IAudioBufferDecoder.Setup(Stream stream, bool leaveOpen, int streamIndex, int outSampleRate, int outChannels) => Setup(stream, leaveOpen, streamIndex, outSampleRate, outChannels);

        public void SampleSeek(long position) => this.SampleSeekCore(position);
        public void Seek(Rational position) => this.SeekCore(position);

        private void UnsafeSampleSeekCore(long position)
        {
            FFmpegUtils.Seek(_stream, _base_context._format_context, position, _out_rate);
            ffmpeg.swr_init(_swr_context).CheckError(FFmpegUtils.ThrowInvalidOperationException);
            ffmpeg.avcodec_flush_buffers(_codec_context);
        }

        void IAudioBufferDecoder.UnsafeSampleSeek(long position)
        {
            lock (_lock)
            {
                position += _pts_offset;
                var channels = _out_channels;
                UnsafeSampleSeekCore(position);
                UpdateBuffer(position);
                _buffer_read = (int)(position - _buffer_head) * channels;
            }
        }

        public int Read(Span<float> buffer) => this.ReadCore(buffer);

        /// <inheritdoc cref="IAudioBufferDecoder.UpdateBuffer"/>
        private bool UpdateBuffer(long a = -1)
        {
            lock (_lock)
            {
                var last_a = -1L;
                var codecContext = _codec_context;
                var swrContext = _swr_context;
                var channels = _out_channels;
                var timeBase = _stream->time_base;
                var needConvert = timeBase.den != _out_rate;
                var frame = GetFrame();
                frame->nb_samples = 0;
                var srcBuffer = stackalloc byte*[8];
                var predicate = FormatContext.GetDefaultPacketPredicate(_stream->index);
                // 未受領のフレームまたはパケットが存在する限り
                while (ffmpeg.avcodec_receive_frame(codecContext, frame) is >= 0 ||
                       _base_context.TryReadPacket(codecContext, predicate))
                {
                    var srcSamples = frame->nb_samples;
                    if (srcSamples is > 0)
                    {
                        for (var i = 0u; i < 8u; i++)
                        {
                            srcBuffer[i] = frame->data[i];
                        }
                        _buffer_length = this.SwrConvertToRead(srcBuffer, srcSamples);
                        _buffer_head = needConvert ? frame->pts * _out_rate * timeBase.num / timeBase.den : frame->pts;
                        if (a is >= 0)
                        {
                            var duration = needConvert ? frame->duration * _out_rate * timeBase.num / timeBase.den : frame->duration;
                            if (_buffer_head > a)
                            {
                                if (last_a != a)
                                {
                                    last_a = a;
                                    UnsafeSampleSeekCore(a - duration);
                                    continue;
                                }
                            }
                            else if (_buffer_head + duration < a)
                            {
                                continue;
                            }
                        }
                        return false;
                    }
                }
                // 変換コンテキストのバッファが残っている場合
                var rest = this.SwrConvertToRead(null, 0);
                _buffer_head = _out_length - rest;
                _buffer_length = rest;
                return rest is <= 0;
            }
        }
        bool IAudioBufferDecoder.UpdateBuffer() => UpdateBuffer();
    }
}
