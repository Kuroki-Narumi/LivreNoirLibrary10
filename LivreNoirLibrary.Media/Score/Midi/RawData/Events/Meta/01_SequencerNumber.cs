using System.IO;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.Midi.RawData
{
    public sealed class SequenceNumberEvent(int number) : MetaEvent(MetaType.SeqNumber)
    {
        private const byte DataLength = sizeof(ushort);

        private ushort _number = (ushort)number;

        public int Number { get => _number; set => _number = (ushort)value; }

        internal static new SequenceNumberEvent Load(BinaryReader reader)
        {
            CheckDataLength(reader, DataLength);
            return new(reader.ReadUInt16BigEndian());
        }

        protected override void DumpDataWithSize(BinaryWriter writer)
        {
            writer.Write(DataLength);
            writer.WriteBigEndian(_number);
        }

        public override string ToString() => $"{nameof(SequenceNumberEvent)}{{{nameof(Number)}={_number}}}";
    }
}
