using System;
using System.IO;

namespace LivreNoirLibrary.Media.Midi.RawData
{
    public sealed class ChannelPressure(int channel, int velocity) : ChannelEvent(channel)
    {
        public override StatusType Type => StatusType.ChannelPressure;

        private byte _vel = GetMax127(velocity);

        public int Velocity { get => _vel; set => _vel = GetMax127(value); }

        public static ChannelPressure Load(int channel, BinaryReader reader) => new(channel, reader.ReadByte());
        protected override void DumpContents(BinaryWriter writer) => writer.Write(_vel);

        public override string ToString() => $"{nameof(ChannelPressure)}{{{nameof(Channel)}={_channel}, {nameof(Velocity)}={_vel}}}";
    }
}
