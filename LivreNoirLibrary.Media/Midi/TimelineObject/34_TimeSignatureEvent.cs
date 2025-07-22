using System;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public sealed class TimeSignatureEvent(TimeSignature value) : SingleValueEvent<TimeSignature>(value), IMetaObject, ICloneable<TimeSignatureEvent>, IDumpable<TimeSignatureEvent>
    {
        MetaType IMetaObject.Type => MetaType.TimeSignature;
        public override ObjectType ObjectType => ObjectType.TimeSignature;
        public override string ObjectName => nameof(TimeSignature);

        public TimeSignatureEvent Clone() => new(_value);
        IObject IObject.Clone() => Clone();
        public override void Dump(BinaryWriter writer) => _value.Dump(writer);
        public static TimeSignatureEvent Load(BinaryReader reader) => new(TimeSignature.Load(reader));

        public override void ExtendToEvent(RawData.RawTimeline timeline, int channel, long tick, Rational pos, long ticksPerWholeNote)
        {
            timeline.Add(tick, new RawData.TimeSignature(_value));
        }

        public int CompareTo(IObject? other) => IObject.CompareBase(this, other);
    }
}
