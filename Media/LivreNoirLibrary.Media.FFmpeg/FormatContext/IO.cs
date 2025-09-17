using System;
using System.IO;
using System.Buffers;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public unsafe partial class FormatContext
    {
        public const int IOBufferSize = 32768;
        private static readonly avio_alloc_context_read_packet _read_delegate;
        private static readonly avio_alloc_context_write_packet _write_delegate;
        private static readonly avio_alloc_context_seek _seek_delegate;
        private static readonly avio_alloc_context_read_packet_func _read_func;
        private static readonly avio_alloc_context_write_packet_func _write_func;
        private static readonly avio_alloc_context_seek_func _seek_func;

        static FormatContext()
        {
            FFmpegUtils.Initialize();
            _read_delegate = new(AVIO_Read);
            _write_delegate = new(AVIO_Write);
            _seek_delegate = new(AVIO_Seek);
            _read_func = new() { Pointer = Marshal.GetFunctionPointerForDelegate(_read_delegate) };
            _write_func = new() { Pointer = Marshal.GetFunctionPointerForDelegate(_write_delegate) };
            _seek_func = new() { Pointer = Marshal.GetFunctionPointerForDelegate(_seek_delegate) };
        }

        private static int AVIO_Read(void* opaque, byte* buf, int buf_size)
        {
            if (GCHandle.FromIntPtr((nint)opaque).Target is Stream stream && stream.CanRead)
            {
                var read = stream.Read(new Span<byte>(buf, buf_size));
                return read is > 0 ? read : ffmpeg.AVERROR_EOF;
            }
            return ffmpeg.AVERROR_STREAM_NOT_FOUND;
        }

        private static int AVIO_Write(void* opaque, byte* buf, int buf_size)
        {
            if (GCHandle.FromIntPtr((nint)opaque).Target is Stream stream && stream.CanWrite)
            {
                stream.Write(new ReadOnlySpan<byte>(buf, buf_size));
                return buf_size;
            }
            return ffmpeg.AVERROR_STREAM_NOT_FOUND;
        }

        private static long AVIO_Seek(void* opaque, long offset, int whence)
        {
            if (GCHandle.FromIntPtr((nint)opaque).Target is Stream stream && stream.CanSeek)
            {
                switch (whence)
                {
                    case 0 or 1 or 2:
                        stream.Seek(offset, (SeekOrigin)whence);
                        break;
                    case ffmpeg.AVSEEK_SIZE:
                        return stream.Length;
                }
                return stream.Position;
            }
            return ffmpeg.AVERROR_STREAM_NOT_FOUND;
        }

        private AVIOContext* AllocIOContext(Stream stream, bool leaveOpen, bool outMode)
        {
            DisposeIO();
            _stream = stream;
            _leave_open = leaveOpen;
            var streamHandle = _stream_handle = GCHandle.Alloc(stream);
            var streamBuffer = (byte*)ffmpeg.av_malloc(IOBufferSize);
            var ioContext = _io_context = ffmpeg.avio_alloc_context(streamBuffer, IOBufferSize,
                outMode ? 1 : 0, (void*)(nint)streamHandle,
                stream.CanRead ? _read_func : default,
                stream.CanWrite ? _write_func : default,
                stream.CanSeek ? _seek_func : default
                );
            if (ioContext is null)
            {
                FFmpegUtils.ThrowIOException("Failed to allocate IO context.");
            }
            _io_context = ioContext;
            return ioContext;
        }
    }
}
