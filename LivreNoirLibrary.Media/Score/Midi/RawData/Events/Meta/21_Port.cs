using System;
using System.IO;

namespace LivreNoirLibrary.Media.Midi.RawData
{
    public sealed class Port(int value) : MetaEvent(MetaType.Port)
    {
        private const byte DataLength = sizeof(byte);

        private byte _value = (byte)value;

        public int Value { get => _value; set => _value = (byte)Math.Clamp(value, 0, 15); }

        internal static new Port Load(BinaryReader reader)
        {
            CheckDataLength(reader, DataLength);
            return new(reader.ReadByte());
        }

        protected override void DumpDataWithSize(BinaryWriter writer)
        {
            writer.Write(DataLength);
            writer.Write(_value);
        }

        public override string ToString() => $"{nameof(Port)}{{{nameof(Value)}={_value}}}";
    }
}
