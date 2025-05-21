using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public static unsafe partial class FFmpegUtils
    {
        internal static bool IsReady(AVCodecContext* context)
        {
            return context is not null &&
                context->codec is not null &&
                context->codec_id is not AVCodecID.AV_CODEC_ID_NONE &&
                context->codec_type switch
                {
                    AVMediaType.AVMEDIA_TYPE_VIDEO => context->width is > 0 && context->height is > 0,
                    AVMediaType.AVMEDIA_TYPE_AUDIO => context->sample_rate is > 0 && context->ch_layout.nb_channels is > 0,
                    _ => true
                };
        }

        private static readonly AVRational TimeBase_US = new() { num = 1, den = ffmpeg.AV_TIME_BASE };

        internal static Rational GetDuration(AVStream* stream, AVFormatContext* formatContext)
        {
            var duration = stream->duration;
            var timeBase = stream->time_base;
            if (duration is < 0)
            {
                duration = formatContext->duration;
                timeBase = TimeBase_US;
            }
            return duration is < 0 ? Rational.Zero : duration.ToRational(timeBase);
        }

        internal static void Seek(AVStream* stream, AVFormatContext* formatContext, long timeStamp_num, long timeStamp_den)
        {
            var timeBase = stream->time_base;
            var timeStamp = timeStamp_num * timeBase.den / timeBase.num / timeStamp_den;
            ffmpeg.av_seek_frame(formatContext, stream->index, timeStamp, ffmpeg.AVSEEK_FLAG_BACKWARD).CheckError(ThrowIOException);
        }

        private static readonly Dictionary<AVCodecID, long> _max_bitrates = new()
        {
            { AVCodecID.AV_CODEC_ID_MP3   , 320000 },
            { AVCodecID.AV_CODEC_ID_VORBIS, 500000 },
            { AVCodecID.AV_CODEC_ID_AAC   , 640000 },
        };

        public static long EnsureBitrate(AVCodecID id, long value)
        {
            if (_max_bitrates.TryGetValue(id, out var limit) && value > limit)
            {
                return limit;
            }
            return value;
        }

        private static readonly Dictionary<string, string> _format_converter = new() {
            { "aac", "adts" }
        };

        public static string ConvertToOutFormatName(string formatString) => _format_converter.TryGetValue(formatString, out var actualFmt) ? actualFmt : formatString;

        public static void CheckAvailableFormats(params ReadOnlySpan<string> exts)
        {
            ExConsole.Write("Available formats:");
            foreach (var ext in exts)
            {
                OutputFormat fmt = new(ext, $"a.{ext}", null);
                if (fmt.IsValid)
                {
                    ExConsole.Write($"{ext} - {fmt.Name} ({fmt.LongName}) / {fmt.MimeType}");
                }
                else
                {
                    ExConsole.Write($"*{ext} - Not supported");
                }
            }
        }

        private static readonly AVCodecID[] _hw_candidates = [
                AVCodecID.AV_CODEC_ID_MPEG2VIDEO,
                AVCodecID.AV_CODEC_ID_MPEG4,
                AVCodecID.AV_CODEC_ID_H264,
                AVCodecID.AV_CODEC_ID_VC1,
                AVCodecID.AV_CODEC_ID_WMV3,
                AVCodecID.AV_CODEC_ID_VP9,
                AVCodecID.AV_CODEC_ID_HEVC,
                AVCodecID.AV_CODEC_ID_AV1,
            ];

        private static readonly AVHWDeviceType[] _hw_accels = [
                AVHWDeviceType.AV_HWDEVICE_TYPE_D3D11VA,
                AVHWDeviceType.AV_HWDEVICE_TYPE_DXVA2,
                AVHWDeviceType.AV_HWDEVICE_TYPE_D3D12VA,
            ];

        private static readonly (string, AVHWDeviceType)[] _hardware_decoder_suffix = [
                ("cuvid", AVHWDeviceType.AV_HWDEVICE_TYPE_CUDA), 
                ("qsv", AVHWDeviceType.AV_HWDEVICE_TYPE_QSV),
                ("vulkan", AVHWDeviceType.AV_HWDEVICE_TYPE_VULKAN),
            ];

        private static bool GetHwCore(AVHWDeviceType type, out AVBufferRef* device)
        {
            AVBufferRef* d = null;
            var ret = ffmpeg.av_hwdevice_ctx_create(&d, type, null, null, 0);
            if (ret is >= 0)
            {
                device = d;
                return true;
            }
            ffmpeg.av_buffer_unref(&d);
            device = null;
            return false;
        }

        private static bool TryGetHardwareDecoder(ref AVCodec* codec, out AVBufferRef* device, out AVHWDeviceType type)
        {
            foreach (var (suffix, t) in _hardware_decoder_suffix)
            {
                var name = $"{GetString(codec->name)}_{suffix}";
                var cdc = ffmpeg.avcodec_find_decoder_by_name(name);
                if (cdc is not null && GetHwCore(t, out device))
                {
                    codec = cdc;
                    type = t;
                    return true;
                }
            }
            if (Array.BinarySearch(_hw_candidates, codec->id) is >= 0)
            {
                foreach (var t in _hw_accels)
                {
                    if (GetHwCore(t, out device))
                    {
                        type = t;
                        return true;
                    }
                }
            }
            type = AVHWDeviceType.AV_HWDEVICE_TYPE_NONE;
            device = null;
            return false;
        }

        internal static void SetupDecoder(AVStream* stream, bool checkHardware, out AVCodecContext* codecContext)
        {
            AVCodecContext* cc = null;
            AVBufferRef* hardwareContext = null;
            try
            {
                // コーデック情報の取得
                var codecParams = stream->codecpar;
                var codec = ffmpeg.avcodec_find_decoder(codecParams->codec_id);
                if (codec is null)
                {
                    ThrowInvalidDataException($"Could not find decoder for {codecParams->codec_id}.");
                }
                // ハードウェアデコード対応の確認
                if (checkHardware && TryGetHardwareDecoder(ref codec, out hardwareContext, out var type))
                {
                    Console.WriteLine($"Use hardware codec: {GetName(codec->name)}({type})");
                }
                // コーデックコンテキストの作成
                codecContext = cc = ffmpeg.avcodec_alloc_context3(codec);
                if (codecContext is null)
                {
                    ThrowInvalidOperationException("Failed to allocate codec context.");
                }
                if (hardwareContext is not null)
                {
                    codecContext->hw_device_ctx = hardwareContext;
                    hardwareContext = null;
                }
                ffmpeg.avcodec_parameters_to_context(codecContext, codecParams).CheckError(ThrowInvalidOperationException);
                ffmpeg.avcodec_open2(codecContext, codec, null).CheckError(ThrowInvalidOperationException);
            }
            catch
            {
                if (cc is not null)
                {
                    ffmpeg.avcodec_free_context(&cc);
                    codecContext = null;
                }
                if (hardwareContext is not null)
                {
                    ffmpeg.av_buffer_unref(&hardwareContext);
                }
                throw;
            }
        }

        private static bool TryGetHardwareEncoder(AVCodecID codecId, IHardwareEncodeOptions options, out AVCodec* codec, out AVBufferRef* device)
        {
            var name = $"{ffmpeg.avcodec_get_name(codecId)}_{options.CodecSuffix}";
            var cdc = ffmpeg.avcodec_find_encoder_by_name(name);
            if (cdc is not null && GetHwCore(options.DeviceType, out device))
            {
                codec = cdc;
                return true;
            }
            codec = null;
            device = null;
            return false;
        }

        internal static void SetupEncoder<T>(this T obj, AVCodecID codecId, IHardwareEncodeOptions? hwOptions, out AVStream* stream, out AVCodecContext* codecContext)
            where T : IFmEncoder
        {
            AVCodecContext* cc = null;
            AVBufferRef* hardwareContext = null;
            AVBufferRef* framesContext = null;
            try
            {
                // コーデック情報の取得
                AVCodec* codec = null;
                // ハードウェアエンコード対応の確認
                if (hwOptions is not null)
                {
                    if (TryGetHardwareEncoder(codecId, hwOptions, out codec, out hardwareContext))
                    {
                        Console.WriteLine($"Use hardware codec: {GetName(codec->name)}({hwOptions.DeviceType})");
                    }
                    else
                    {
                        ThrowNotSupportedException($"{GetString(codec->name)}_{hwOptions.CodecSuffix} is not supported.");
                    }
                }
                else
                {
                    codec = ffmpeg.avcodec_find_encoder(codecId);
                }
                if (codec is null)
                {
                    ThrowInvalidDataException($"Could not find encoder for {codecId}.");
                }
                // コーデックコンテキストの作成
                codecContext = cc = ffmpeg.avcodec_alloc_context3(codec);
                if (codecContext is null)
                {
                    ThrowInvalidOperationException("Failed to allocate codec context.");
                }
                // コーデック情報のロード
                obj.SetupEncoder(codec, codecContext);
                if (hardwareContext is not null)
                {
                    codecContext->hw_device_ctx = hardwareContext;
                    var format = codecContext->pix_fmt;
                    codecContext->pix_fmt = hwOptions!.HwPixelFormat;
                    if (hwOptions.NeedsHardwareFrame)
                    {
                        framesContext = ffmpeg.av_hwframe_ctx_alloc(hardwareContext);
                        if (framesContext is null)
                        {
                            ThrowInvalidOperationException("Failed to allocate HW frames context.");
                        }
                        AVHWFramesContext* framesCtx = (AVHWFramesContext*)framesContext->data;
                        framesCtx->format = codecContext->pix_fmt;
                        framesCtx->sw_format = format;
                        framesCtx->width = codecContext->width;
                        framesCtx->height = codecContext->height;
                        framesCtx->initial_pool_size = 30;

                        ffmpeg.av_hwframe_ctx_init(framesContext).CheckError(ThrowInvalidOperationException);
                        codecContext->hw_frames_ctx = framesContext;
                    }
                    hardwareContext = null;
                    framesContext = null;
                }
                AVDictionary* dict = null;
                if (obj.GetEncodeOptions(out var options))
                {
                    hwOptions?.WriteOptions(options);
                    foreach (var (key, value) in options)
                    {
                        ffmpeg.av_dict_set(&dict, key, value, 0);
                    }
                }
                ffmpeg.avcodec_open2(codecContext, codec, &dict).CheckError(ThrowInvalidOperationException);
                if (dict is not null)
                {
                    ffmpeg.av_dict_free(&dict);
                }
                // ストリームの作成
                stream = ffmpeg.avformat_new_stream(obj.FormatContext, codec);
                if (stream is null)
                {
                    ThrowInvalidOperationException("Failed to allocate new stream.");
                }
                stream->sample_aspect_ratio = codecContext->sample_aspect_ratio;
                ffmpeg.avcodec_parameters_from_context(stream->codecpar, codecContext).CheckError(ThrowInvalidOperationException);
                stream->time_base = codecContext->time_base;
            }
            catch
            {
                if (cc is not null)
                {
                    ffmpeg.avcodec_free_context(&cc);
                    codecContext = null;
                }
                if (hardwareContext is not null)
                {
                    ffmpeg.av_buffer_unref(&hardwareContext);
                }
                if (framesContext is not null)
                {
                    ffmpeg.av_buffer_unref(&framesContext);
                }
                throw;
            }
        }
    }
}
