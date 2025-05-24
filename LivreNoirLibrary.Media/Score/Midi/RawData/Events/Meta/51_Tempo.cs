using System;
using System.IO;

namespace LivreNoirLibrary.Media.Midi.RawData
{
    public sealed class Tempo(int value) : MetaEvent(MetaType.Tempo)
    {
        private const byte DataLength = sizeof(byte) * 3;

        public const int MinValue = 1;
        public const int MaxValue = 0xFFFFFF;
        public const int DefaultValue = 500_000;

        private int _value = value;

        public int Value { get => _value; set => _value = Math.Clamp(value, MinValue, MaxValue); }

        internal static new Tempo Load(BinaryReader reader)
        {
            CheckDataLength(reader, DataLength);
            var value = (reader.ReadByte() << 16) + (reader.ReadByte() << 8) + reader.ReadByte();
            return new(value);
        }

        protected override void DumpDataWithSize(BinaryWriter writer)
        {
            writer.Write(DataLength);
            writer.Write((byte)(_value >> 16));
            writer.Write((byte)(_value >> 8));
            writer.Write((byte)_value);
        }

        public override string ToString() => $"{nameof(Tempo)}{{{nameof(Value)}={_value}}}";
    }
}
