using System;
using System.IO;

namespace LivreNoirLibrary.Media.Midi.RawData
{
    public sealed class TimeSignature(int numerator = 4, int denExp = 2, int clock = 24, int beat32 = 8) : MetaEvent(MetaType.TimeSignature)
    {
        private const byte DataLength = sizeof(byte) * 4;

        private byte _num = (byte)numerator;
        private byte _den = (byte)denExp;
        private byte _clock = (byte)clock;
        private byte _b32 = (byte)beat32;

        public int Numerator { get => _num; set => _num = (byte)value; }
        public int DenExp { get => _den; set => _den = (byte)value; }
        public int Clock { get => _clock; set => _clock = (byte)value; }
        public int Beat32 { get => _b32; set => _b32 = (byte)value; }

        public int Denominator { get => GetDen(_den); set => _den = GetDenExp(value); }

        public TimeSignature(Media.TimeSignature value) : this(value.Numerator, GetDenExp(value.Denominator)) { }
        public Media.TimeSignature ToStruct() => new(_num, GetDen(_den));

        internal new static TimeSignature Load(BinaryReader reader)
        {
            CheckDataLength(reader, DataLength);
            var num = reader.ReadByte();
            var den = reader.ReadByte();
            var clock = reader.ReadByte();
            var b32 = reader.ReadByte();
            return new(num, den, clock, b32);
        }

        protected override void DumpDataWithSize(BinaryWriter writer)
        {
            writer.Write(DataLength);
            writer.Write(_num);
            writer.Write(_den);
            writer.Write(_clock);
            writer.Write(_b32);
        }

        public static int GetDen(byte exp) => unchecked(1 << exp);
        public static byte GetDenExp(int value) => (byte)int.Log2(value);

        public override string ToString() => $"{nameof(TimeSignature)}{{{_num}/{GetDen(_den)}, {nameof(Clock)}={_clock}, {nameof(Beat32)}={_b32}}}";
    }
}
