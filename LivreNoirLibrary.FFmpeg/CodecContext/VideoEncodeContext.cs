using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public unsafe class VideoEncodeContext(FormatContext formatContext) : VideoContext(formatContext), IVideoEncoder, IFmEncoder
    {
        /// <summary>
        /// valid only in <see cref="Setup"/>
        /// </summary>
        private VideoEncodeOptions _options;
        private AVPixelFormat _in_pix_format;
        private bool _flushed;

        private Rational _next_pts;

        AVFormatContext* IFmEncoder.FormatContext => _base_context._format_context;
        protected override bool NeedsHardwareFrame => _codec_context->hw_frames_ctx is not null;

        internal void Setup(VideoEncodeOptions options)
        {
            _options = options;

            this.SetupEncoder(options.CodecOptions.Codec, options.HardwareOptions, out _stream, out _codec_context);
            var codecContext = _codec_context;
            _stream->sample_aspect_ratio = codecContext->sample_aspect_ratio;
            // フレーム
            var frame = GetFrame();
            frame->format = (int)_in_pix_format;
            frame->width = codecContext->width;
            frame->height = codecContext->height;
            frame->time_base = codecContext->time_base;
            ffmpeg.av_frame_get_buffer(frame, 0).CheckError(FFmpegUtils.ThrowInvalidOperationException);
            ffmpeg.av_frame_make_writable(frame).CheckError(FFmpegUtils.ThrowInvalidOperationException);
            if ((frame = GetHwFrame()) is not null)
            {
                frame->width = codecContext->width;
                frame->height = codecContext->height;
                frame->time_base = codecContext->time_base;
                ffmpeg.av_hwframe_get_buffer(codecContext->hw_frames_ctx, frame, 0).CheckError(FFmpegUtils.ThrowInvalidOperationException);
            }
            _next_pts = Rational.Zero;

            _options = default; // 初期化後は参照しない
        }

        void IFmEncoder.SetupEncoder(AVCodec* codec, AVCodecContext* codecContext)
        {
            var formatContext = _base_context._format_context;
            var options = _options;

            _in_width = options.InputWidth;
            _in_height = options.InputHeight;
            _frame_rate = options.FrameRate;
            codecContext->width = _out_width = options.OutputWidth;
            codecContext->height = _out_height = options.OutputHeight;
            codecContext->framerate = options.FrameRate.ToAVRational();
            codecContext->time_base = options.FrameRate.Invert().ToAVRational();
            codecContext->bit_rate = FFmpegUtils.EnsureBitrate(codec->id, options.BitRate is <= 0 ? VideoEncodeOptions.DefaultBitRate : options.BitRate);

            var codecOptions = options.CodecOptions;
            codecContext->pix_fmt = _in_pix_format = codecOptions.PixelFormat;
            codecContext->field_order = codecOptions.FieldOrder;
            codecContext->color_range = codecOptions.ColorRange;
            codecContext->colorspace = codecOptions.ColorSpace;
            codecContext->color_primaries = codecOptions.ColorPrimaries;
            codecContext->color_trc = codecOptions.ColorTransferCharacteristic;
            codecContext->chroma_sample_location = codecOptions.ChromaLocation;
            if (codecOptions.GopSize.IsPositiveThanZero())
            {
                codecContext->gop_size = (int)((double)codecOptions.GopSize * _frame_rate);
            }
            codecContext->max_b_frames = codecOptions.MaxBFrames;
            codecContext->sample_aspect_ratio = codecOptions.AspectRatio.ToAVRational();

            // generate global header when the format requires it
            if ((formatContext->oformat->flags & ffmpeg.AVFMT_GLOBALHEADER) is not 0)
            {
                codecContext->flags |= ffmpeg.AV_CODEC_FLAG_GLOBAL_HEADER;
            }
        }

        bool IFmEncoder.GetEncodeOptions([MaybeNullWhen(false)] out Dictionary<string, string?> options)
        {
            options = _options.CodecOptions.GetDictionary();
            return options is not null;
        }

        public void Write(ReadOnlySpan<byte> buffer, Rational duration = default)
        {
            lock (_lock)
            {
                var width = _in_width;
                var height = _in_height;
                var outWidth = _out_width;
                var outHeight = _out_height;
                var requiredSize = width * height * 4;
                if (buffer.Length < requiredSize)
                {
                    FFmpegUtils.ThrowIndexOutOfRangeException($"buffer is too small ({buffer.Length} / required={requiredSize})");
                }
                var frame = GetFrame();
                var hwFrame = GetHwFrame();
                var codecContext = _codec_context;
                var inFrame = hwFrame is null ? frame : hwFrame;
                var timeBase = _stream->time_base;
                codecContext->time_base = frame->time_base = timeBase;
                var sws = EnsureSwsContext(width, height, InternalPixelFormat, outWidth, outHeight, _in_pix_format);
                fixed (byte* ptr = buffer)
                {
                    var (fsl, fst, bsl, bst) = GetSlices(frame, ptr, width);
                    ffmpeg.sws_scale(sws, bsl, bst, 0, height, fsl, fst).CheckError(FFmpegUtils.ThrowInvalidOperationException);
                }
                if (hwFrame is not null)
                {
                    ffmpeg.av_hwframe_transfer_data(hwFrame, frame, 0).CheckError(FFmpegUtils.ThrowOutOfMemoryException);
                }
                inFrame->flags = 0;
                inFrame->pict_type = AVPictureType.AV_PICTURE_TYPE_NONE;
                var pts = _next_pts;
                if (duration.IsNegativeOrZero())
                {
                    duration = _frame_rate.Invert();
                }
                inFrame->pts = pts.ToTimeStamp(timeBase);
                inFrame->duration = duration.ToTimeStamp(timeBase);
                _next_pts = pts + duration;

                ffmpeg.avcodec_send_frame(codecContext, inFrame).CheckError(FFmpegUtils.ThrowInvalidOperationException);
                _base_context.WritePacket(codecContext, _stream);
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
                // コンテキストのフラッシュ
                ffmpeg.avcodec_send_frame(_codec_context, null).CheckError(FFmpegUtils.ThrowInvalidOperationException);
                _base_context.WritePacket(_codec_context, _stream);
                _flushed = true;
            }
        }
    }
}
