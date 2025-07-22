
using System.IO;

namespace LivreNoirLibrary.Media.Midi.RawData
{
    public sealed class ProgramChange(int channel, int number) : ChannelEvent(channel)
    {
        public override StatusType Type => StatusType.ProgramChange;

        private byte _number = GetMax127(number);

        public int Number { get => _number; set => _number = GetMax127(value); }

        public static ProgramChange Load(int channel, BinaryReader reader) => new(channel, reader.ReadByte());
        protected override void DumpContents(BinaryWriter writer) => writer.Write(_number);

        public override string ToString() => $"{nameof(ProgramChange)}{{{nameof(Channel)}={_channel}, {nameof(Number)}={_number}}}";
    }
}
