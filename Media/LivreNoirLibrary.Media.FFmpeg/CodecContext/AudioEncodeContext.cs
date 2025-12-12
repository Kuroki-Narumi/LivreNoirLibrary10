using System;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public unsafe class AudioEncodeContext(FormatContext formatContext) : AudioContext(formatContext), IAudioEncodeContext, IAudioBufferEncoder, IFmEncoder
    {
        /// <summary>
        /// valid only in <see cref="Setup"/>
        /// </summary>
        private AudioEncodeOptions _options;
        private bool _sent_any_frame;
        private bool _flushed;

        /// <inheritdoc cref="IAudioBufferDecoder.BufferHead"/>
        private long _buffer_head;
        /// <inheritdoc cref="IAudioBufferInternal.BufferLength"/>
        private int _buffer_length;
        /// <inheritdoc cref="IAudioBufferInternal.BufferIndex"/>
        private int _buffer_wrote;

        float* IAudioBufferInternal.BufferPointer => _buffer.Pointer;
        int IAudioBufferInternal.BufferLength => _buffer_length;
        int IAudioBufferInternal.BufferIndex { get => _buffer_wrote; set => _buffer_wrote = value; }

        AVFormatContext* IFmEncoder.FormatContext => _base_context._format_context;

        internal void Setup(AVCodecID codec, AudioEncodeOptions options)
        {
            options.Validate();
            _options = options;

            this.SetupEncoder(codec, null, out _stream, out _codec_context);
            var inCh = InputChannels;
            var inRate = InputSampleRate;
            // 変換コンテキスト
            var codecContext = _codec_context;
            _swrContext = FFmpegUtils.CreateSwrContext(
                codecContext->ch_layout, codecContext->sample_fmt, codecContext->sample_rate,
                FFmpegUtils.CreateChannelLayout(inCh), InputSampleFormat, inRate
                );
            // フレーム
            var outFrameSize = codecContext->frame_size;
            EnsureBufferSize(outFrameSize * inCh);
            _buffer_head = 0;
            _buffer_length = outFrameSize;
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
            (InputSampleRate, InputChannels, var rate, var ch, var bitRate) = _options;
            void* out_configs = null;
            int out_num_configs = 0;

            // サンプルレート
            if (ffmpeg.avcodec_get_supported_config(codecContext, codec, AVCodecConfig.AV_CODEC_CONFIG_SAMPLE_RATE, 0, &out_configs, &out_num_configs) is >= 0)
            {
                var minDif = int.MaxValue;
                var minIndex = -1;
                var rateList = (int*)out_configs;
                for (var i = 0; i < out_num_configs; i++)
                {
                    var rt = rateList[i];
                    var dif = Math.Abs(rate - rt);
                    if (dif < minDif)
                    {
                        minDif = dif;
                        minIndex = i;
                    }
                }
                if (minIndex is >= 0)
                {
                    rate = rateList[minIndex];
                }
            }
            codecContext->sample_rate = OutputSampleRate = rate;
            codecContext->time_base = new() { num = 1, den = rate };

            // チャンネルレイアウト
            var found = false;
            if (ffmpeg.avcodec_get_supported_config(codecContext, codec, AVCodecConfig.AV_CODEC_CONFIG_CHANNEL_LAYOUT, 0, &out_configs, &out_num_configs) is >= 0)
            {
                var chList = (AVChannelLayout*)out_configs;
                for (var i = 0; i < out_num_configs; i++)
                {
                    var layout = chList[i];
                    if (ch == layout.nb_channels)
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
            OutputChannels = ch;

            // サンプルフォーマット
            codecContext->sample_fmt = InputSampleFormat = InternalSampleFormat;
            if (ffmpeg.avcodec_get_supported_config(codecContext, codec, AVCodecConfig.AV_CODEC_CONFIG_SAMPLE_FORMAT, 0, &out_configs, &out_num_configs) is >= 0)
            {
                found = out_num_configs is <= 0;
                var formatList = (AVSampleFormat*)out_configs;
                for (var i = 0; i < out_num_configs; i++)
                {
                    var fmt = formatList[i];
                    if (fmt is InternalSampleFormat)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    codecContext->sample_fmt = formatList[0];
                }
            }
            OutputSampleFormat = codecContext->sample_fmt;

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
                frame->nb_samples = codecContext->frame_size;
                var outBuffer = stackalloc byte*[8];
                for (var i = 0u; i < 8u; i++)
                {
                    outBuffer[i] = frame->data[i];
                }
                var inSamples = _buffer_wrote / InputChannels;
                var outSamples = this.SwrConvertToWrite(_buffer.Pointer, inSamples, outBuffer, codecContext->frame_size);
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
