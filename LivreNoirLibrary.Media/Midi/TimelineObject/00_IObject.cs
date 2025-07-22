using System;
using System.IO;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public interface IObject : IComparable<IObject>
    {
        public ObjectType ObjectType { get; }
        public string ObjectName { get; }
        public string ContentString { get; }
        public IObject Clone();
        public void Dump(BinaryWriter writer);
        public void ExtendToEvent(RawData.RawTimeline timeline, int channel, long tick, Rational pos, long ticksPerWholeNote);

        public static Rational GetPosition(long tick, long ticksPerWholeNote) => new(tick, ticksPerWholeNote);
        public static long GetTick(Rational position, long ticksPerWholeNote) => position.Numerator * ticksPerWholeNote / position.Denominator;
        public static int CompareBase(IObject left, IObject? right) => right is not null ? left.ObjectType.CompareTo(right.ObjectType) : 1;
    }

    public static class IObjectExtensions
    {
        public static string GetIdentifier(this IObject obj) => $"{obj.ObjectName}{obj.ContentString}";
    }
}
