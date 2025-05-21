using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Text;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Wave
{
    public static class ChunkIds
    {
        public const int Length = 4;

        public const string RiffHeader = "RIFF";
        public const string DataHeader = "WAVE";

        public const string Format = "fmt ";
        public const string Fact = "fact";
        public const string Data = "data";
        public const string Sampler = "smpl";
        public const string Struct = "strc";
        public const string Inst = "inst";
        public const string Acid = "acid";
        public const string BExt = "bext";
        public const string Cue = "cue ";
        public const string TList = "tlst";
        public const string PlayList = "plst";

        public const string ID3 = "id3 ";

        public const string LIST = "LIST";
        public const string Associated = "adtl";
        public const string LTxt = "ltxt";
        public const string Label = "labl";
        public const string FILE = "file";
        public const string Note = "note";

        public const string Info = "INFO";
        public const string IARL = "IARL";
        public const string IArtist = "IART";
        public const string ICommision = "ICMS";
        public const string IComment = "ICMT";
        public const string ICopyright = "ICOP";
        public const string IDate = "ICRD";
        public const string ICropped = "ICRP";
        public const string IDimension = "IDIM";
        public const string IDPI = "IDPI";
        public const string IEngineer = "IENG";
        public const string IGenre = "IGNR";
        public const string IKey = "IKEY";
        public const string ILightness = "ILGT";
        public const string IMedium = "IMED";
        public const string IName = "INAM";
        public const string IPalette = "IPLT";
        public const string IPRD = "IPRD";
        public const string ISubject = "ISBJ";
        public const string ISoft = "ISFT";
        public const string ISource = "ISRC";
        public const string ISurface = "ISRF";
        public const string ITech = "ITCH";

        private static readonly Lock _lock = new();
        private static readonly byte[] _chid_buffer = new byte[Length];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Read(BinaryReader reader)
        {
            lock (_lock)
            {
                var count = reader.Read(_chid_buffer, 0, Length);
                if (count is not Length)
                {
                    throw new EndOfStreamException("Cannot read 4 bytes.");
                }
                return Encoding.ASCII.GetString(_chid_buffer);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Check(BinaryReader reader, string chid)
        {
            lock (_lock)
            {
                var count = reader.Read(_chid_buffer, 0, Length);
                return count is Length && _chid_buffer.AsSpan().SequenceEqual(StringPool.AsAsciiSpan(chid));
            }
        }

        public static void CheckAndThrow(BinaryReader reader, string chid)
        {
            if (!Check(reader, chid))
            {
                throw new InvalidDataException($"Header pattern mismatched (\"{Encoding.ASCII.GetString(_chid_buffer)}\" expected \"{chid}\")");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(BinaryWriter writer, string chid) => writer.Write(StringPool.AsAsciiArray(chid));
    }
}
