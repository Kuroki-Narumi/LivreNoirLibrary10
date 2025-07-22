using System;
using System.IO;

namespace LivreNoirLibrary.Media.Midi.RawData
{
    public sealed class ChannelPrefix(int value) : MetaEvent(MetaType.ChannelPrefix)
    {
        private const byte DataLength = sizeof(byte);

        private byte _channel = (byte)value;

        public int Channel { get => _channel; set => _channel = (byte)value; }

        internal static new ChannelPrefix Load(BinaryReader reader)
        {
            CheckDataLength(reader, DataLength);
            return new(reader.ReadByte());
        }

        protected override void DumpDataWithSize(BinaryWriter writer)
        {
            writer.Write(DataLength);
            writer.Write(_channel);
        }

        public override string ToString() => $"{nameof(ChannelPrefix)}{{{nameof(Channel)}={_channel}}}";
    }
}
