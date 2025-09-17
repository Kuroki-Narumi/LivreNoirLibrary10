using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LivreNoirLibrary.Media.Wave
{
    public class WaveInfo : IWaveMetaData
    {
        public long DataPosition { get; private set; }
        public uint DataLength { get; private set; }
        public FormatChunk Format { get; private set; }
        public List<RiffChunk> Chunks { get; } = [];

        public static WaveInfo Create(Stream stream, BinaryReader reader)
        {
            WaveInfo result = new();
            FourLetterHeader.CheckAndThrow(reader, ChunkIds.RiffHeader);
            var length = (long)reader.ReadUInt32();
            var endPos = stream.Position + length;
            FourLetterHeader.CheckAndThrow(reader, ChunkIds.DataHeader);
            var list = result.Chunks;
            list.Clear();
            while (stream.Position < endPos)
            {
                var chid = FourLetterHeader.Read(reader);
                if (chid is ChunkIds.Data)
                {
                    (result.DataPosition, result.DataLength) = reader.ReadRiffChunk<DataChunk>();
                }
                else if (chid is ChunkIds.Format)
                {
                    result.Format = reader.ReadRiffChunk<FormatChunk>();
                }
                else
                {
                    var chunk = RiffChunk.Create(chid, reader);
                    list.Add(chunk);
                }
            }
            return result;
        }

        public static bool IsSupported(string path) => IsSupported(File.OpenRead(path), false);

        public static bool IsSupported(Stream stream, bool leaveOpen = true)
        {
            using BinaryReader reader = new(stream, Encoding.UTF8, leaveOpen);
            return IsSupported(stream, reader);
        }

        public static bool IsSupported(Stream stream, BinaryReader reader)
        {
            var pos = stream.Position;
            try
            {
                if (!FourLetterHeader.Check(reader, ChunkIds.RiffHeader))
                {
                    return false;
                }
                var length = (long)reader.ReadUInt32();
                var endPos = stream.Position + length;
                if (!FourLetterHeader.Check(reader, ChunkIds.DataHeader))
                {
                    return false;
                }
                while (stream.Position < endPos)
                {
                    var chid = FourLetterHeader.Read(reader);
                    if (chid is ChunkIds.Format)
                    {
                        var format = reader.ReadRiffChunk<FormatChunk>();
                        return format.TryGetSampleFormat(out _);
                    }
                    length = reader.ReadUInt32();
                    stream.Position += length + (length % 2 is 1 ? 1 : 0);
                }
                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                stream.Position = pos;
            }
        }
    }
}
