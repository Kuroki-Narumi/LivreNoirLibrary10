using System;
using System.IO;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public unsafe partial class FormatContext
    {
        internal FormatContext() { }

        public static FormatContext OpenRead(string path)
        {
            FormatContext fmt = new();
            fmt.OpenRead(File.OpenRead(path), false);
            return fmt;
        }

        internal void OpenRead(Stream stream, bool leaveOpen)
        {
            try
            {
                var ioContext = AllocIOContext(stream, leaveOpen, false);
                // フォーマットコンテキストの作成
                var formatContext = ffmpeg.avformat_alloc_context();
                formatContext->pb = ioContext;
                // 入力ストリームのオープン
                ffmpeg.avformat_open_input(&formatContext, null, null, null).CheckError(FFmpegUtils.ThrowIOException);
                // 内部ストリームの取得
                ffmpeg.avformat_find_stream_info(formatContext, null).CheckError(FFmpegUtils.ThrowInvalidDataException);
                _format_context = formatContext;
            }
            catch
            {
                DisposeIO();
                throw;
            }
        }

        internal void OpenWrite(AVOutputFormat* format, Stream stream, bool leaveOpen)
        {
            try
            {
                var ioContext = AllocIOContext(stream, leaveOpen, true);
                // フォーマットコンテキストの作成
                AVFormatContext* formatContext;
                ffmpeg.avformat_alloc_output_context2(&formatContext, format, null, null)
                      .CheckError(FFmpegUtils.ThrowInvalidOperationException);
                formatContext->pb = ioContext;
                _format_context = formatContext;
            }
            catch
            {
                DisposeIO();
                throw;
            }
        }
    }
}
