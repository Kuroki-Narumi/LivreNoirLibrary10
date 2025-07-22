using System;
using System.IO;

namespace LivreNoirLibrary.Media.Midi.RawData
{
    public sealed class PitchBend(int channel, int value) : ChannelEvent(channel)
    {
        public const int MinimumValue = -8192;
        public const int MaximumValue = 8191;

        public override StatusType Type => StatusType.PitchBend;

        private short _value = Coerce(value);

        public int Value { get => _value; set => _value = Coerce(value); }

        public static PitchBend Load(int channel, BinaryReader reader)
        {
            var lsb = reader.ReadByte() & 0x7F;
            var msb = (reader.ReadByte() & 0x7F) << 7;
            var value = lsb + msb + MinimumValue;
            return new(channel, value);
        }

        protected override void DumpContents(BinaryWriter writer)
        {
            var value = _value - MinimumValue;
            writer.Write((byte)(value & 0x7F));
            writer.Write((byte)((value >> 7) & 0x7F));
        }

        public static short Coerce(int value) => (short)Math.Clamp(value, MinimumValue, MaximumValue);

        public override string ToString() => $"{nameof(PitchBend)}{{{nameof(Channel)}={_channel}, {nameof(Value)}={_value}}}";
    }
}
