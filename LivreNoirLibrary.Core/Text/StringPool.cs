using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LivreNoirLibrary.Text
{
    public static class StringPool
    {
        private static readonly HashSet<string> _pool = [];

        [return: NotNullIfNotNull(nameof(str))]
        public static string? Get(string? str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            if (_pool.TryGetValue(str, out var pool))
            {
                return pool;
            }
            else
            {
                _pool.Add(str);
                return str;
            }
        }

        private static readonly Dictionary<string, byte[]> _ascii = [];

        private static byte[] AsAsciiSpanInternal(string s)
        {
            if (!_ascii.TryGetValue(s, out var ary))
            {
                ary = Encoding.ASCII.GetBytes(s);
                _ascii.Add(s, ary);
            }
            return ary;
        }

        public static byte[] AsAsciiArray(this string s) => AsAsciiSpanInternal(s);
        public static ReadOnlySpan<byte> AsAsciiSpan(this string s) => new(AsAsciiSpanInternal(s));
    }
}
