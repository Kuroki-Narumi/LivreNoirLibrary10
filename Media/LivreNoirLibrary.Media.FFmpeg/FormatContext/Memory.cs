using System;
using System.IO;
using System.Runtime.InteropServices;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.FFmpeg
{
    internal unsafe delegate bool PacketPredicate(AVPacket* packet);

    public unsafe partial class FormatContext : DisposableBase
    {
        private Stream? _stream;
        private bool _leave_open;
        private GCHandle _stream_handle;
        private AVIOContext* _io_context;

        internal AVFormatContext* _format_context;
        private AVPacket* _packet;

        public bool IsReady => _stream_handle.IsAllocated && _io_context is not null && _format_context is not null;

        protected static void GetPacket(ref AVPacket* packet)
        {
            if (packet is not null)
            {
                ffmpeg.av_packet_unref(packet);
            }
            else
            {
                packet = ffmpeg.av_packet_alloc();
            }
        }

        protected AVPacket* GetPacket()
        {
            GetPacket(ref _packet);
            return _packet;
        }

        internal static PacketPredicate GetDefaultPacketPredicate(int streamIndex) => packet => packet->stream_index == streamIndex;

        internal bool TryReadPacket(AVCodecContext* codecContext, PacketPredicate predicate)
        {
            var fmt = _format_context;
            var packet = GetPacket();
            try
            {
                while (ffmpeg.av_read_frame(fmt, packet) is >= 0)
                {
                    if (predicate(packet))
                    {
                        ffmpeg.avcodec_send_packet(codecContext, packet).CheckError(FFmpegUtils.ThrowInvalidOperationException);
                        return true;
                    }
                    ffmpeg.av_packet_unref(packet);
                }
                return false;
            }
            finally
            {
                ffmpeg.av_packet_unref(packet);
            }
        }

        internal void EstimateKeyFrames(AVStream* stream, out long minInterval, out long maxInterval, int maxCount = 5)
        {
            var fmt = _format_context;
            var timeBase = stream->time_base;
            var packet = GetPacket();
            var keyframeCount = 0;
            var lastPts = 0L;
            minInterval = long.MaxValue;
            maxInterval = long.MinValue;
            try
            {
                while (keyframeCount < maxCount && ffmpeg.av_read_frame(fmt, packet) is >= 0)
                {
                    if (packet->stream_index == stream->index && FFmpegUtils.IsKeyFrame(packet))
                    {
                        var pts = (packet->pts == ffmpeg.AV_NOPTS_VALUE ? packet->dts : packet->pts).ToTicks(timeBase);
                        if (keyframeCount is > 0)
                        {
                            var interval = pts - lastPts;
                            if (interval > maxInterval)
                            {
                                maxInterval = interval;
                            }
                            if (interval < minInterval)
                            {
                                minInterval = interval;
                            }
                        }
                        keyframeCount++;
                        lastPts = pts;
                    }
                    ffmpeg.av_packet_unref(packet);
                }
            }
            finally
            {
                ffmpeg.av_seek_frame(_format_context, stream->index, 0, ffmpeg.AVSEEK_FLAG_BACKWARD);
            }
        }

        internal void WritePacket(AVCodecContext* codecContext, AVStream* stream)
        {
            var packet = GetPacket();
            try
            {
                while (ffmpeg.avcodec_receive_packet(codecContext, packet) is >= 0)
                {
                    packet->stream_index = stream->index;
                    ffmpeg.av_packet_rescale_ts(packet, codecContext->time_base, stream->time_base);
                    ffmpeg.av_interleaved_write_frame(_format_context, packet).CheckError(FFmpegUtils.ThrowInvalidOperationException);
                    ffmpeg.av_packet_unref(packet);
                }
            }
            finally
            {
                ffmpeg.av_packet_unref(packet);
            }
        }

        internal void DisposeBuffer()
        {
            if (_packet is not null)
            {
                var packet = _packet;
                ffmpeg.av_packet_free(&packet);
                _packet = null;
            }
        }

        protected void DisposeIO()
        {
            if (_format_context is not null)
            {
                var formatContext = _format_context;
                ffmpeg.avformat_free_context(formatContext);
                _format_context = null;
            }
            if (_io_context is not null)
            {
                var ioContext = _io_context;
                ffmpeg.av_freep(&ioContext->buffer);
                ffmpeg.avio_context_free(&ioContext);
                _io_context = null;
            }
            if (_stream_handle.IsAllocated)
            {
                _stream_handle.Free();
                _stream_handle = default;
            }
            if (_stream is not null && !_leave_open)
            {
                _stream.Dispose();
            }
            _stream = null;
        }

        protected override void DisposeUnmanaged()
        {
            DisposeBuffer();
            DisposeIO();
            base.DisposeUnmanaged();
        }
    }
}
