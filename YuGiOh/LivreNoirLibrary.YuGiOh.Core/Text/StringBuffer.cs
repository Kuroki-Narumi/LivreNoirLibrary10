using System;
using System.Threading;

namespace LivreNoirLibrary.YuGiOh
{
    public static class StringBuffer
    {
        const int BufferLength = 1024;

        private static readonly ThreadLocal<char[]> _buffers = new(() => new char[BufferLength]);

        public static Span<char> Get() => _buffers.Value!;
    }
}
