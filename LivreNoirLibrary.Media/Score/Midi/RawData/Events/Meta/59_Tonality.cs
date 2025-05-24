using System;
using System.IO;

namespace LivreNoirLibrary.Media.Midi.RawData
{
    public sealed class Tonality(int sf, MajorMinor mm) : MetaEvent(MetaType.Tonality)
    {
        private const byte DataLength = sizeof(sbyte) + sizeof(byte);

        private sbyte _sf = (sbyte)sf;
        private MajorMinor _mm = mm;

        public int SF { get => _sf; set => _sf = (sbyte)Math.Clamp(value, -6, 6); }
        public MajorMinor MM { get => _mm; set => _mm = value; }

        public Tonality(Media.Tonality value) : this(value.SharpFlat, value.MajorMinor) { }
        public Media.Tonality ToStruct() => new(_sf, _mm);

        internal static new Tonality Load(BinaryReader reader)
        {
            CheckDataLength(reader, DataLength);
            var sf = reader.ReadSByte();
            var mm = (MajorMinor)reader.ReadByte();
            return new(sf, mm);
        }

        protected override void DumpDataWithSize(BinaryWriter writer)
        {
            writer.Write(DataLength);
            writer.Write(_sf);
            writer.Write((byte)_mm);
        }

        public override string ToString() => $"{nameof(Tonality)}{{{Media.Tonality.GetString(_sf, _mm)}}}";
    }
}
