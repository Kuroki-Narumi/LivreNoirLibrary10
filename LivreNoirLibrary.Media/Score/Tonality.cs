using System;
using System.IO;
using LivreNoirLibrary.IO;
using static LivreNoirLibrary.Media.Score.KeyNames;

namespace LivreNoirLibrary.Media.Score
{
    public readonly struct Tonality(int sf, MajorMinor mm) : IDumpable<Tonality>
    {
        private readonly sbyte _sf = (sbyte)Math.Clamp(sf, -7, 7);
        private readonly MajorMinor _mm = mm;

        public sbyte SharpFlat => _sf;
        public MajorMinor MajorMinor => _mm;

        public void Dump(BinaryWriter writer)
        {
            writer.Write(_sf);
            writer.Write((byte)_mm);
        }

        public static Tonality Load(BinaryReader reader)
        {
            var sf = reader.ReadSByte();
            var mm = reader.ReadByte();
            return new(sf, (MajorMinor)mm);
        }

        public override string ToString() => GetString(_sf, _mm);

        public static string GetString(int sf, MajorMinor mm)
        {
            string s = sf >= 0 ? SharpSign[sf + (int)mm * 3] : FlatSign[(12 + sf + (int)mm * 3) % 12];
            return $"{s} {mm}";
        }

        private static readonly string[] SharpSign = [C, G, D, A, E, B, FSharp, CSharp, GSharp, DSharp, ASharp, F];
        private static readonly string[] FlatSign = [C, G, D, A, E, CFlat, GFlat, DFlat, AFlat, EFlat, BFlat, F];
    }
}
