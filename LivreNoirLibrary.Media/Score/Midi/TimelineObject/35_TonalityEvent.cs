using System;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public sealed class TonalityEvent(Tonality value) : SingleValueEvent<Tonality>(value), IMetaObject, ICloneable<TonalityEvent>, IDumpable<TonalityEvent>
    {
        MetaType IMetaObject.Type => MetaType.Tonality;
        public override ObjectType ObjectType => ObjectType.Tonality;
        public override string ObjectName => nameof(Tonality);

        public TonalityEvent Clone() => new(_value);
        IObject IObject.Clone() => Clone();
        public override void Dump(BinaryWriter writer) => _value.Dump(writer);
        public static TonalityEvent Load(BinaryReader reader) => new(Tonality.Load(reader));

        public override void ExtendToEvent(RawData.RawTimeline timeline, int channel, long tick, Rational pos, long ticksPerWholeNote)
        {
            timeline.Add(tick, new RawData.Tonality(_value));
        }

        public int CompareTo(IObject? other) => IObject.CompareBase(this, other);
    }
}
