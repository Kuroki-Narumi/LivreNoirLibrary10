using System;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Midi
{
    public interface IObject : IComparable<IObject>
    {
        ObjectType ObjectType { get; }
        string ObjectName { get; }
        string ContentString { get; }
        IObject Clone();
        void Dump(BinaryWriter writer);
        void ExtendToEvent(RawTimeline timeline, int channel, long tick, Rational pos, long ticksPerWholeNote);

        static Rational GetPosition(long tick, long ticksPerWholeNote) => new(tick, ticksPerWholeNote);
        static long GetTick(Rational position, long ticksPerWholeNote) => position.Numerator * ticksPerWholeNote / position.Denominator;
        static int CompareBase(IObject left, IObject? right) => right is not null ? left.ObjectType.CompareTo(right.ObjectType) : 1;
    }

    public interface IObject<T> : IObject, ICloneable<T>, IDumpable, ILoadable<T>
        where T : IObject<T>
    {

    }
}
