using System;
using System.IO;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.Midi.RawData
{
    public abstract class MetaEvent(MetaType type) : Event
    {
        public sealed override StatusType Status => StatusType.MetaEvent;
        public MetaType Type { get; } = type;

        internal static MetaEvent Load(BinaryReader reader)
        {
            var type = (MetaType)reader.ReadByte();
            return type switch
            {
                MetaType.SeqNumber => SequenceNumberEvent.Load(reader),
                >= MetaType.Text and <= MetaType.Device => MetaText.Load(type, reader),
                MetaType.ChannelPrefix => ChannelPrefix.Load(reader),
                MetaType.Port => Port.Load(reader),
                MetaType.TrackEnd => TrackEnd.Load(reader),
                MetaType.Tempo => Tempo.Load(reader),
                MetaType.SmpteOffset => SmpteOffset.Load(reader),
                MetaType.TimeSignature => TimeSignature.Load(reader),
                MetaType.Tonality => Tonality.Load(reader),
                MetaType.SequencerEvent => SequencerEvent.Load(reader),
                _ => throw new NotSupportedException("Invalid MetaType value"),
            };
        }

        protected static int CheckDataLength(BinaryReader reader, int expected)
        {
            var count = reader.Read7BitEncodedIntBigEndian();
            if (expected is > 0 && count != expected)
            {
                throw new InvalidDataException($"wrong data count: {count}(expected {expected})");
            }
            return count;
        }

        protected sealed override void DumpContents(BinaryWriter writer)
        {
            writer.Write((byte)Type);
            DumpDataWithSize(writer);
        }

        protected virtual void DumpDataWithSize(BinaryWriter writer) { }
    }
}
