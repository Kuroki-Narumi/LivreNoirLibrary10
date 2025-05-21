using System;
using System.IO;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public sealed unsafe class MetaEditor : FFmpegEncoder
    {
        private readonly FormatContext _source;
        private readonly bool _leave_open;

        public MetaEditor(string outPath, string inPath) : this(General.CreateSafe(outPath), OpenRead(inPath), false, false) { }
        public MetaEditor(string outPath, FormatContext source, bool leaveOpen = true) : this(General.CreateSafe(outPath), source, false, leaveOpen) { }
        public MetaEditor(Stream outStream, FormatContext source, bool targetLeaveOpen = true, bool sourceLeaveOpen = true) : 
            base(outStream, OutputFormat.ByInputFormat(source._format_context->iformat), targetLeaveOpen)
        {
            if (!source.IsReady)
            {
                FFmpegUtils.ThrowInvalidOperationException("source context is not ready.");
            }
            _source = source;
            _leave_open = sourceLeaveOpen;

            var context = _format_context;
            foreach (var info in source.EnumStreamInfo())
            {
                var ins = info._stream;
                var outs = ffmpeg.avformat_new_stream(context, null);
                if (outs is null)
                {
                    FFmpegUtils.ThrowInvalidOperationException("failed to create new stream.");
                }
                ffmpeg.avcodec_parameters_copy(outs->codecpar, ins->codecpar).CheckError(FFmpegUtils.ThrowInvalidOperationException);
                outs->codecpar->codec_tag = 0;
            }
            LoadSourceMetaTags();
        }

        public void LoadSourceMetaTags()
        {
            var source = _source;
            this.ClearMetaTags();
            source.CopyMetaTags(this);
            var streams = _format_context->streams;
            foreach (var info in source.EnumStreamInfo())
            {
                var outStream = streams[info.Index];
                ffmpeg.av_dict_free(&outStream->metadata);
                ffmpeg.av_dict_copy(&outStream->metadata, info._stream->metadata, 0);
            }
        }

        protected override void DisposeManaged()
        {
            if (!_leave_open)
            {
                _source.Dispose();
            }
            base.DisposeManaged();
        }

        protected override void BeforeFlush()
        {
            var packet = GetPacket();
            var inContext = _source._format_context;
            var outContext = _format_context;
            // ストリームコピー
            while (ffmpeg.av_read_frame(inContext, packet) is >= 0)
            {
                var index = packet->stream_index;
                var inStream = inContext->streams[index];
                var outStream = outContext->streams[index];
                // タイムスタンプを調整
                packet->pts = ffmpeg.av_rescale_q_rnd(packet->pts, inStream->time_base, outStream->time_base, AVRounding.AV_ROUND_NEAR_INF | AVRounding.AV_ROUND_PASS_MINMAX);
                packet->dts = ffmpeg.av_rescale_q_rnd(packet->dts, inStream->time_base, outStream->time_base, AVRounding.AV_ROUND_NEAR_INF | AVRounding.AV_ROUND_PASS_MINMAX);
                packet->duration = ffmpeg.av_rescale_q(packet->duration, inStream->time_base, outStream->time_base);
                packet->pos = -1;
                // パケットをそのまま書き込む
                ffmpeg.av_interleaved_write_frame(outContext, packet).CheckError(FFmpegUtils.ThrowInvalidOperationException);
                ffmpeg.av_packet_unref(packet);
            }
        }
    }
}
