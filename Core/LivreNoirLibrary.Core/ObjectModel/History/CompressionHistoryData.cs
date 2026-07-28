using System;
using System.IO;
using System.IO.Compression;
using System.Numerics;

namespace LivreNoirLibrary.ObjectModel
{
    public abstract class CompressionHistoryData<T>
    {
        private readonly MemoryStream? _ms;

        public CompressionHistoryData(T? source)
        {
            if (source is not null)
            {
                _ms = CreateInternalData(source);
            }
        }

        public MemoryStream CreateInternalData(T source)
        {
            MemoryStream ms = new();
            using (DeflateStream buffer = new(ms, CompressionMode.Compress, true))
            {
                Dump(buffer, source);
            }
            return ms;
        }

        protected abstract void Dump(Stream stream, T source);

        public void ConvertBack(T? target, object? state = null)
        {
            if (target is not null && _ms is { } ms)
            {
                ms.Position = 0;
                using DeflateStream buffer = new(ms, CompressionMode.Decompress, true);
                Load(buffer, target, state);
            }
        }

        protected abstract void Load(Stream stream, T target, object? state);

        public unsafe bool EqualsAll(CompressionHistoryData<T> other)
        {
            var ms1 = _ms;
            var ms2 = other._ms;
            if (ms1 is null)
            {
                return ms2 is null;
            }
            if (ms2 is null || ms1.Length != ms2.Length)
            {
                return false;
            }
            var count = Vector<byte>.Count;
            var ptr1 = stackalloc byte[count];
            var ptr2 = stackalloc byte[count];
            var span1 = new Span<byte>(ptr1, count);
            var span2 = new Span<byte>(ptr2, count);
            ms1.Position = 0;
            ms2.Position = 0;
            var read1 = ms1.Read(span1);
            var read2 = ms2.Read(span2);
            while (read1 > 0 && read2 > 0)
            {
                if (read1 != read2)
                {
                    return false;
                }
                if (read1 < count)
                {
                    span1[read1..].Clear();
                    span2[read2..].Clear();
                }
                if (*(Vector<byte>*)ptr1 != *(Vector<byte>*)ptr2)
                {
                    return false;
                }
                read1 = ms1.Read(span1);
                read2 = ms2.Read(span2);
            }
            return read1 == read2;
        }
    }
}
