using System;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public unsafe class AudioEncodeContext(FormatContext formatContext) : AudioContext(formatContext), IAudioEncoder, IAudioBufferEncoder, IFmEncoder
    {
        /// <summary>
        /// valid only in <see cref="Setup"/>
        /// </summary>
        private AudioEncodeOptions _options;
        private bool _sent_any_frame;
        private bool _flushed;

        /// <inheritdoc cref="IAudioBufferDecoder.BufferHead"/>
        private long _buffer_head;
        /// <inheritdoc cref="IAudioBuffer.BufferLength"/>
        private int _buffer_length;
        /// <inheritdoc cref="IAudioBuffer.BufferIndex"/>
        private int _buffer_wrote;

        /// <inheritdoc cref="AVCodecContext.frame_size"/>
        public int FrameSize => _buffer_length;

        Span<float> IAudioBuffer.Buffer => Buffer;
        int IAudioBuffer.BufferLength => _buffer_length;
        int IAudioBuffer.BufferIndex { get => _buffer_wrote; set => _buffer_wrote = value; }

        AVFormatContext* IFmEncoder.FormatContext => _base_context._format_context;

        internal void Setup(AVCodecID codec, AudioEncodeOptions options)
        {
            options.Validate();
            _options = options;

            this.SetupEncoder(codec, null, out _stream, out _codec_context);
            // 変換コンテキスト
            var codecContext = _codec_context;
            AVChannelLayout inLayout = default;
            ffmpeg.av_channel_layout_default(&inLayout, _in_channels);
            this.AllocSwrContext(codecContext->ch_layout, codecContext->sample_fmt, codecContext->sample_rate, inLayout, InternalSampleFormat, _in_rate);
            // フレーム
            var outFrameSize = codecContext->frame_size;
            var inFrameSize = (int)ffmpeg.av_rescale_rnd(outFrameSize, _in_rate, _out_rate, AVRounding.AV_ROUND_UP);
            EnsureBufferSize(inFrameSize * _in_channels);
            _buffer_head = 0;
            _buffer_length = inFrameSize;
            _buffer_wrote = 0;
            var frame = GetFrame();
            frame->nb_samples = outFrameSize;
            frame->format = (int)codecContext->sample_fmt;
            frame->time_base = codecContext->time_base;
            frame->sample_rate = codecContext->sample_rate;
            ffmpeg.av_channel_layout_copy(&frame->ch_layout, &codecContext->ch_layout).CheckError(FFmpegUtils.ThrowInvalidOperationException);
            ffmpeg.av_frame_get_buffer(frame, 0).CheckError(FFmpegUtils.ThrowInvalidOperationException);
            ffmpeg.av_frame_make_writable(frame).CheckError(FFmpegUtils.ThrowInvalidOperationException);

            _options = default; // 初期化後は参照しない
        }

        void IFmEncoder.SetupEncoder(AVCodec* codec, AVCodecContext* codecContext)
        {
            (_in_rate, _in_channels, var rate, var ch, var bitRate) = _options;
            // サンプルレート
            var rates = codec->supported_samplerates;
            if (rates is not null)
            {
                var minDif = int.MaxValue;
                var minIndex = -1;
                for (var i = 0; ; i++)
                {
                    var rt = rates[i];
                    if (rt is 0)
                    {
                        break;
                    }
                    var dif = rate > rt ? rate - rt : rt - rate;
                    if (dif < minDif)
                    {
                        minDif = dif;
                        minIndex = i;
                    }
                }
                if (minIndex is >= 0)
                {
                    rate = rates[minIndex];
                }
            }
            codecContext->sample_rate = _out_rate = rate;
            codecContext->time_base = new() { num = 1, den = rate };
            // チャンネルレイアウト
            var chls = codec->ch_layouts;
            var found = false;
            if (chls is not null)
            {
                for (var i = 0; ; i++)
                {
                    var layout = chls[i];
                    var chs = layout.nb_channels;
                    if (chs is 0)
                    {
                        break;
                    }
                    if (chs == ch)
                    {
                        ffmpeg.av_channel_layout_copy(&codecContext->ch_layout, &layout);
                        found = true;
                        break;
                    }
                }
            }
            if (!found)
            {
                ffmpeg.av_channel_layout_default(&codecContext->ch_layout, ch);
            }
            _out_channels = ch;
            // サンプルフォーマット
            if (codec->sample_fmts is not null)
            {
                codecContext->sample_fmt = codec->sample_fmts[0];
                for (var i = 0; ; i++)
                {
                    var sampleFmt = codec->sample_fmts[i];
                    if (sampleFmt is < 0)
                    {
                        break;
                    }
                    else if (sampleFmt is InternalSampleFormat)
                    {
                        codecContext->sample_fmt = InternalSampleFormat;
                        break;
                    }
                }
            }
            else 
            {
                codecContext->sample_fmt = InternalSampleFormat;
            }
            if (bitRate is <= 0)
            {
                bitRate = AudioEncodeOptions.DefaultBitrate;
            }
            // ビットレート
            codecContext->bit_rate = FFmpegUtils.EnsureBitrate(codec->id, bitRate);
            // バッファサイズ
            if (codecContext->frame_size is 0)
            {
                codecContext->frame_size = rate;
            }
        }

        public void Write(ReadOnlySpan<float> buffer) => this.WriteCore(buffer);

        bool IAudioBufferEncoder.EncodeBuffer()
        {
            lock (_lock)
            {
                var frame = GetFrame();
                var codecContext = _codec_context;
                var outBuffer = stackalloc byte*[8];
                for (var i = 0u; i < 8u; i++)
                {
                    outBuffer[i] = frame->data[i];
                }
                var inSpan = Buffer[.._buffer_wrote];
                var inSamples = _buffer_wrote / _in_channels;
                var outSamples = this.SwrConvertToWrite(inSpan, inSamples, outBuffer, codecContext->frame_size);
                if (outSamples is <= 0 && _sent_any_frame)
                {
                    return false;
                }
                var position = _buffer_head;
                frame->nb_samples = outSamples;
                frame->pts = position;
                frame->duration = outSamples;
                ffmpeg.avcodec_send_frame(codecContext, frame).CheckError(FFmpegUtils.ThrowInvalidOperationException);
                _base_context.WritePacket(codecContext, _stream);
                _buffer_head = position + outSamples;
                _buffer_wrote = 0;
                _sent_any_frame = true;
                return true;
            }
        }

        public void Flush()
        {
            if (_flushed)
            {
                return;
            }
            lock (_lock)
            {
                this.FlushCore();
                // コンテキストのフラッシュ
                ffmpeg.avcodec_send_frame(_codec_context, null).CheckError(FFmpegUtils.ThrowInvalidOperationException);
                _base_context.WritePacket(_codec_context, _stream);
                _flushed = true;
            }
        }
    }
}
