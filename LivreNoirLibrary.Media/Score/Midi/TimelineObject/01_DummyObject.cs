using System;
using System.IO;
using LivreNoirLibrary.Media.Midi.RawData;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public sealed class DummyObject : IObject
    {
        public ObjectType ObjectType => ObjectType.None;
        public string ObjectName => nameof(DummyObject);
        public string ContentString => "";
        public IObject Clone() => new DummyObject();
        public int CompareTo(IObject? other) => IObject.CompareBase(this, other);
        public void Dump(BinaryWriter writer) { }
        public void ExtendToEvent(RawTimeline timeline, int channel, long tick, Rational pos, long ticksPerWholeNote) { }
    }
}
