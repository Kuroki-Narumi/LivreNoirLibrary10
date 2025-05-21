using System;
using System.Collections.Generic;
using System.IO;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public unsafe class VideoDecoder : VideoContext, IVideoDecoder
    {
        private long _keyframe_min_interval = -1;
        private long _keyframe_max_interval = -1;
        private readonly List<(long, Rational)> _pos_cache = [];

        public Rational Duration { get; private set; }
        public long TotalFrame => (Duration * _frame_rate).Ceiling();
        public long TotalTicks => Duration.ToTicks();

        public long MaxKeyframeInterval => _keyframe_max_interval;
        public long MinKeyframeInterval => _keyframe_min_interval;

        public VideoDecoder(string path, VideoDecodeOptions options = default) : this(File.OpenRead(path), false, options) { }
        public VideoDecoder(Stream stream, bool leaveOpen = true, VideoDecodeOptions options = default) : base(new())
        {
            Open(stream, leaveOpen, options.StreamIndex, options.Width, options.Height);
        }

        protected override void DisposeUnmanaged()
        {
            base.DisposeUnmanaged();
            _base_context.Dispose();
        }

        private void Open(Stream stream, bool leaveOpen, int streamIndex, int outWidth, int outHeight)
        {
            var baseContext = _base_context;
            baseContext.OpenRead(stream, leaveOpen);
            if (baseContext.TryGetStream(out var avs, AVMediaType.AVMEDIA_TYPE_VIDEO, streamIndex))
            {
                InitializeRead(avs, outWidth, outHeight);
                return;
            }
            FFmpegUtils.ThrowInvalidDataException("No video stream found.");
        }

        private void InitializeRead(AVStream* stream, int dstWidth, int dstHeight)
        {
            FFmpegUtils.SetupDecoder(stream, true, out var context);
            _stream = stream;
            _codec_context = context;
            // 画面サイズ
            var srcWidth = context->width;
            var srcHeight = context->height;
            if (dstWidth is <= 0)
            {
                dstWidth = srcWidth;
            }
            if (dstHeight is <= 0)
            {
                dstHeight = srcHeight;
            }
            _in_width = srcWidth;
            _in_height = srcHeight;
            _out_width = dstWidth;
            _out_height = dstHeight;
            // 長さ
            Duration = FFmpegUtils.GetDuration(stream, _base_context._format_context);
            // フレームレート
            _frame_rate = stream->r_frame_rate.ToRational();
            // キーフレームリストの取得
            _base_context.EstimateKeyFrames(stream, out _keyframe_min_interval, out _keyframe_max_interval);
            _pos_cache.Clear();
        }

        private void SeekCore(long posNum, long posDen)
        {
            lock (_lock)
            {
                var formatContext = _base_context._format_context;
                var stream = _stream;
                if (formatContext is not null && stream is not null)
                {
                    FFmpegUtils.Seek(stream, formatContext, posNum, posDen);
                    ffmpeg.avcodec_flush_buffers(_codec_context);
                    _pos_cache.Clear();
                }
            }
        }

        public void SeekByTick(long ticks)
        {
            lock (_lock)
            {
                var len = TotalTicks;
                if (ticks is < 0)
                {
                    ticks += len;
                }
                if (ticks is < 0 || ticks > len)
                {
                    throw new ArgumentOutOfRangeException(nameof(ticks), $"value is out of range: {ticks} / max={len}");
                }
                SeekCore(ticks, TimeSpan.TicksPerSecond);
            }
        }

        public void Seek(Rational position)
        {
            lock (_lock)
            {
                var len = Duration;
                if (position.IsNegative())
                {
                    position += len;
                }
                if (position.IsNegative() || position > len)
                {
                    throw new ArgumentOutOfRangeException(nameof(position), $"value is out of range: {position} / max={len}");
                }
                SeekCore(position.Numerator, position.Denominator);
            }
        }

        public bool DecodeFrame(Rational requiredTime)
        {
            lock (_lock)
            {
                var pts = 0L;
                var duration = Rational.Zero;
                bool PacketPredicate(AVPacket* packet)
                {
                    if (packet->stream_index != _stream->index)
                    {
                        return false;
                    }
                    var timeBase = _stream->time_base;
                    var time = packet->dts.ToRational(timeBase);
                    /*
                     * Dts(秒) = dts * timeBase.num / timeBase.den
                     * Req(秒) = requiredTime.Numerator / requiredTime.Denominator
                     * Dts >= Req
                     * <=> dts * timeBase.num / timeBase.den >= requiredTime.Numerator / requiredTime.Denominator
                     * <=> dts * timeBase.num * requiredTime.Denominator >= timeBase.den * requiredTime.Numerator
                     */
                    // ハードウェアデコードが有効、パケットがキーフレーム、または要求デコード時刻が指定時刻以上
                    if (IsHardwareAccelerated || FFmpegUtils.IsKeyFrame(packet) || time >= requiredTime)
                    {
                        pts = packet->pts;
                        duration = packet->duration.ToRational(timeBase);
                        return true;
                    }
                    return false;
                }
                if (_base_context.TryReadPacket(_codec_context, PacketPredicate))
                {
                    _pos_cache.Add((pts, duration));
                    return true;
                }
                return false;
            }
        }

        public bool GetFrame(Span<byte> buffer, out Rational pts, out Rational duration)
        {
            lock (_lock)
            {
                var width = _out_width;
                var height = _out_height;
                var requiredSize = width * height * 4;
                if (buffer.Length < requiredSize)
                {
                    FFmpegUtils.ThrowIndexOutOfRangeException($"buffer is too small ({buffer.Length} / required={requiredSize})");
                }
                var timeBase = _stream->time_base;
                var frame = GetFrame();
                var hwFrame = GetHwFrame();
                var outFrame = hwFrame is null ? frame : hwFrame;
                if (ffmpeg.avcodec_receive_frame(_codec_context, outFrame) is >= 0)
                {
                    if (hwFrame is not null)
                    {
                        ffmpeg.av_hwframe_transfer_data(frame, hwFrame, 0).CheckError(FFmpegUtils.ThrowOutOfMemoryException);
                    }
                    var inHeight = frame->height;
                    var sws = EnsureSwsContext(frame->width, inHeight, (AVPixelFormat)frame->format, width, height, InternalPixelFormat);
                    fixed (byte* ptr = buffer)
                    {
                        var (fsl, fst, bsl, bst) = GetSlices(frame, ptr, width);
                        ffmpeg.sws_scale(sws, fsl, fst, 0, inHeight, bsl, bst).CheckError(FFmpegUtils.ThrowInvalidOperationException);
                    }
                    var rawPts = outFrame->pts;
                    var index = _pos_cache.FindIndex(pr => pr.Item1 == rawPts);
                    duration = _pos_cache[index].Item2;
                    _pos_cache.RemoveAt(index);
                    if (rawPts is >= 0)
                    {
                        pts = rawPts.ToRational(timeBase);
                    }
                    else
                    {
                        pts = frame->best_effort_timestamp.ToRational(timeBase);
                    }
                    return true;
                }
                pts = Rational.MinusOne;
                duration = Rational.Zero;
                return false;
            }
        }
    }
}
