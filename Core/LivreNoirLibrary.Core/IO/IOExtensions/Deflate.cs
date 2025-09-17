using System;
using System.IO;
using System.IO.Compression;

namespace LivreNoirLibrary.IO
{
    public static partial class IOExtensions
    {
        public static void Deflate(this Stream source, Stream destination)
        {
            using DeflateStream s = new(destination, CompressionMode.Compress, true);
            source.CopyTo(s);
        }

        public static void Deflate(this byte[] source, Stream destination)
        {
            using DeflateStream s = new(destination, CompressionMode.Compress, true);
            s.Write(source, 0, source.Length);
        }

        public static void Inflate(this Stream source, Stream destination)
        {
            using DeflateStream s = new(source, CompressionMode.Decompress, true);
            s.CopyTo(destination);
        }

        public static void Inflate(this byte[] source, Stream destination)
        {
            using MemoryStream ms = new(source);
            Inflate(ms, destination);
        }

        public static byte[] Deflate(this Stream source)
        {
            using MemoryStream d = new();
            Deflate(source, d);
            return d.ToArray();
        }

        public static byte[] Deflate(this byte[] source)
        {
            using MemoryStream d = new();
            Deflate(source, d);
            return d.ToArray();
        }

        public static byte[] Inflate(this Stream source)
        {
            using MemoryStream d = new();
            Inflate(source, d);
            return d.ToArray();
        }

        public static byte[] Inflate(this byte[] source)
        {
            using MemoryStream s = new(source);
            return Inflate(s);
        }
    }
}
