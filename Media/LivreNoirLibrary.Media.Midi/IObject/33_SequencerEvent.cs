using System;
using System.IO;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public sealed class SequencerEvent(byte[] data) : DataObjectBase(ObjectType.SequencerEvent, data), IMetaObject, IObject<SequencerEvent>
    {
        public MetaType Type => MetaType.SequencerEvent;
        public override string ObjectName => nameof(SequencerEvent);

        public SequencerEvent(Events.SequencerEvent source) : this(source.Data) { }

        public static SequencerEvent Load(BinaryReader reader) => new(LoadData(reader));

        public SequencerEvent Clone() => new([.. _data]);
        IObject IObject.Clone() => Clone();

        public void ExtendToEvent(RawTimeline timeline, int channel, long tick, Rational pos, long ticksPerWholeNote)
        {
            timeline.Add(tick, new Events.SequencerEvent(_data));
        }

        public int CompareTo(IObject? other) => IObject.CompareBase(this, other);
    }
}
