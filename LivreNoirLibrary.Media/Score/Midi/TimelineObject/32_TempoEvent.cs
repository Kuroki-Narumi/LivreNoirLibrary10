using System;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media.Midi.RawData;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public sealed class TempoEvent(int value) : SingleValueEvent<int>(value), IMetaObject, ICloneable<TempoEvent>, IDumpable<TempoEvent>
    {
        MetaType IMetaObject.Type => MetaType.Tempo;
        public override ObjectType ObjectType => ObjectType.Tempo;
        public override string ObjectName => "Tempo";
        public override string ContentString => MediaUtils.MicroSeconds2Bpm(_value).ToString("0.000");

        protected override int CoerceValue(int value) => Math.Clamp(value, Tempo.MinValue, Tempo.MaxValue);

        public TempoEvent Clone() => new(_value);
        IObject IObject.Clone() => Clone();
        public override void Dump(BinaryWriter writer) => writer.Write(_value);
        public static TempoEvent Load(BinaryReader reader) => new(reader.ReadInt32());

        public override void ExtendToEvent(RawTimeline timeline, int channel, long tick, Rational pos, long ticksPerWholeNote)
        {
            timeline.Add(tick, new Tempo(_value));
        }

        public int CompareTo(IObject? other) => IObject.CompareBase(this, other);
    }
}
