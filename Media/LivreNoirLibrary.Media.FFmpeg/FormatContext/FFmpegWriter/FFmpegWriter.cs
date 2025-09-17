using System;
using System.IO;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public unsafe partial class FFmpegWriter : FormatContext, IMediaEncoder
    {
        internal readonly AVOutputFormat* _out_format;
        private bool _header_wrote;
        private bool _footer_wrote;

        public FFmpegWriter(Stream stream, OutputFormat format, bool leaveOpen)
        {
            _out_format = format._format;
            OpenWrite(_out_format, stream, leaveOpen);
        }

        public override void VerifyAccess()
        {
            if (_footer_wrote)
            {
                throw new InvalidOperationException("Stream has been flushed.");
            }
            base.VerifyAccess();
        }

        public void WriteHeader()
        {
            VerifyAccess();
            if (!_header_wrote)
            {
                ffmpeg.avformat_write_header(_format_context, null).CheckError(FFmpegUtils.ThrowInvalidOperationException);
                _header_wrote = true;
            }
        }

        protected virtual void BeforeFlush() { }

        public void WriteTrailer()
        {
            if (!_footer_wrote)
            {
                WriteHeader();
                BeforeFlush();
                ffmpeg.av_write_trailer(_format_context).CheckError(FFmpegUtils.ThrowInvalidOperationException);
                _footer_wrote = true;
            }
        }

        protected override void DisposeUnmanaged()
        {
            WriteTrailer();
            base.DisposeUnmanaged();
        }
    }
}
