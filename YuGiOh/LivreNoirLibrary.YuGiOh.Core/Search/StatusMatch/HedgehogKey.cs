using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.YuGiOh.Search
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct HedgehogKey(MonsterType monsterType, Attribute attribute, int level) : IComparable<HedgehogKey>, IEquatable<HedgehogKey>
    {
        [FieldOffset(0)]
        internal readonly uint _value;

        [FieldOffset(0)]
        private readonly byte _mType = (byte)monsterType;
        [FieldOffset(1)]
        private readonly byte _attr = (byte)attribute;
        [FieldOffset(2)]
        private readonly short _level = (short)level;

        public MonsterType MonsterType => (MonsterType)_mType;
        public Attribute Attribute => (Attribute)_attr;
        public int Level => _level;

        public HedgehogKey(Card card) : this(card.MonsterType, card.Attribute, card.Level) { }

        public int CompareTo(HedgehogKey other)
        {
            var c = _level.CompareTo(other._level);
            if (c is not 0)
            {
                return c;
            }
            c = _attr.CompareTo(other._attr);
            if (c is not 0)
            {
                return c;
            }
            return _mType.CompareTo(other._mType);
        }

        public bool Equals(HedgehogKey other) => _value == other._value;
        public override bool Equals([NotNullWhen(true)] object? obj) => obj is HedgehogKey other && Equals(other);
        public override int GetHashCode() => _value.GetHashCode();

        public static bool operator ==(HedgehogKey left, HedgehogKey right) => left.Equals(right);
        public static bool operator !=(HedgehogKey left, HedgehogKey right) => !left.Equals(right);
        public static bool operator <(HedgehogKey left, HedgehogKey right) => left.CompareTo(right) < 0;
        public static bool operator <=(HedgehogKey left, HedgehogKey right) => left.CompareTo(right) <= 0;
        public static bool operator >(HedgehogKey left, HedgehogKey right) => left.CompareTo(right) > 0;
        public static bool operator >=(HedgehogKey left, HedgehogKey right) => left.CompareTo(right) >= 0;
    }
}
