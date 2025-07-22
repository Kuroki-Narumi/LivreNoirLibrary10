
using System.IO;

namespace LivreNoirLibrary.Media.Midi.RawData
{
    public sealed class TrackEnd : MetaEvent
    {
        private const byte DataLength = 0;

        public static TrackEnd Instance { get; } = new();

        private TrackEnd() : base(MetaType.TrackEnd) { }

        internal static new TrackEnd Load(BinaryReader reader)
        {
            CheckDataLength(reader, DataLength);
            return Instance;
        }

        protected override void DumpDataWithSize(BinaryWriter writer)
        {
            writer.Write(DataLength);
        }

        public override string ToString() => $"{nameof(TrackEnd)}";
    }
}
