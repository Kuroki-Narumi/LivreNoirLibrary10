using System;
using System.IO;
using LivreNoirLibrary.Media.Midi.RawData;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public abstract class SingleValueEvent<T>(T value)
        where T : notnull
    {
        protected T _value = value;

        public abstract ObjectType ObjectType { get; }
        public abstract string ObjectName { get; }
        public virtual string ContentString => _value.ToString()!;
        public T Value { get => _value; set => _value = CoerceValue(value); }

        protected virtual T CoerceValue(T value) => value;

        public abstract void Dump(BinaryWriter writer);
        public abstract void ExtendToEvent(RawTimeline timeline, int channel, long tick, Rational pos, long ticksPerWholeNote);
    }
}
