using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Media.FFmpeg
{
    public static unsafe partial class FFmpegUtils
    {
        private static readonly byte[] _error_buffer = new byte[ffmpeg.AV_ERROR_MAX_STRING_SIZE];
        private static Dictionary<int, string>? _n2name;

        private static bool TryGetN2Name(int n, [MaybeNullWhen(false)] out string name)
        {
            if (_n2name is null)
            {
                Dictionary<int, string> dic = [];
                foreach (var info in typeof(ffmpeg).GetFields())
                {
                    var nn = info.Name;
                    if (nn.StartsWith("AVERROR_"))
                    {
                        dic.Add((int)info.GetValue(null)!, nn);
                    }
                }
                _n2name = dic;
            }
            return _n2name.TryGetValue(n, out name);
        }

        public static int CheckError(this int n, Action<string?> action)
        {
            if (n is < 0)
            {
                Array.Clear(_error_buffer);
                string? message;
                fixed (byte* ptr = _error_buffer)
                {
                    ffmpeg.av_strerror(n, ptr, ffmpeg.AV_ERROR_MAX_STRING_SIZE);
                    message = GetString(ptr);
                }
                var code = TryGetN2Name(n, out var nn) ? $"{n}({nn})" : n.ToString();
                Console.WriteLine($"FFmpeg Error Code{code}: {message} in {Environment.StackTrace}");
                action($"code{code}: {message}");
            }
            return n;
        }

        public static void ThrowIOException(string? message) => throw new IOException(message);
        public static void ThrowOutOfMemoryException(string? message) => throw new OutOfMemoryException(message);
        public static void ThrowInvalidDataException(string? message) => throw new InvalidDataException(message);
        public static void ThrowInvalidOperationException(string? message) => throw new InvalidOperationException(message);
        public static void ThrowNotSupportedException(string? message) => throw new NotSupportedException(message);
        public static void ThrowIndexOutOfRangeException(string? message) => throw new IndexOutOfRangeException(message);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CheckIndexRange<T>(in Span<T> span, int offset, int count) => CheckIndexRange((ReadOnlySpan<T>)span, offset, count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CheckIndexRange<T>(in ReadOnlySpan<T> span, int offset, int count)
        {
            if (offset + count > span.Length)
            {
                ThrowIndexOutOfRangeException($"buffer length is too small: {span.Length} (required {offset + count})");
            }
        }
    }
}
