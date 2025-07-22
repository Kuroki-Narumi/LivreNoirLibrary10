using System;
using System.IO;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.Midi.RawData
{
    public sealed class SequencerEvent(byte[] data) : MetaEvent(MetaType.SequencerEvent)
    {
        public byte[] Data { get; set; } = data;

        internal static new SequencerEvent Load(BinaryReader reader) => new(ReadWithSize(reader));
        protected override void DumpDataWithSize(BinaryWriter writer) => WriteWithSize(writer, Data);

        public override string ToString() => $"{nameof(SequencerEvent)}{{{nameof(Data)}={BitConverter.ToString(Data)}}}";
    }
}
