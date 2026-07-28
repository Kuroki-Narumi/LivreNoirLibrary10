using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.YuGiOh.Search
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct StatusKey(MonsterType monsterType, Attribute attribute, int level, int atk, int def) : IEquatable<StatusKey>
    {
        [FieldOffset(0)]
        internal readonly ulong _value;

        [FieldOffset(0)]
        private readonly byte _mType = (byte)monsterType;
        [FieldOffset(1)]
        private readonly byte _attr = (byte)attribute;
        [FieldOffset(2)]
        private readonly short _level = (short)level;
        [FieldOffset(4)]
        private readonly short _atk = (short)atk;
        [FieldOffset(6)]
        private readonly short _def = (short)def;

        public MonsterType MonsterType => (MonsterType)_mType;
        public Attribute Attribute => (Attribute)_attr;
        public int Level => _level;
        public int Atk => _atk;
        public int Def => _def;

        public StatusKey(Card card) : this(card.MonsterType, card.Attribute, card.Level, card.Atk, card.Def) { }

        public bool Equals(StatusKey other) => _value == other._value;
        public override bool Equals([NotNullWhen(true)] object? obj) => obj is StatusKey other && Equals(other);
        public override int GetHashCode() => _value.GetHashCode();

        public static bool operator ==(StatusKey left, StatusKey right) => left.Equals(right);
        public static bool operator !=(StatusKey left, StatusKey right) => !left.Equals(right);

        public override string ToString()
        {
            using var o = ObjectPool.RentStringBuilder(out var sb);
            if (MonsterType is not 0)
            {
                sb.Append(Vocab.GetName(MonsterType));
                sb.Append('/');
            }
            if (Attribute is not 0)
            {
                sb.Append(Vocab.GetShortName(Attribute));
                sb.Append('/');
            }
            sb.Append('★');
            sb.Append(Vocab.GetStatusText(Level));
            sb.Append("/攻");
            sb.Append(Vocab.GetStatusText(Atk));
            sb.Append("/守");
            sb.Append(Vocab.GetStatusText(Def));
            return sb.ToString();
        }

        public bool IsMatch(StatusKey other, [NotNullWhen(true)] out string? matchText)
        {
            var match = 0;
            matchText = null;
            if (_mType == other._mType)
            {
                matchText = MonsterType.GetName();
                match++;
            }
            if (_attr == other._attr)
            {
                matchText = Attribute.GetName();
                match++;
            }
            if (_level == other._level)
            {
                matchText = $"★{Level}";
                match++;
            }
            if (_atk == other._atk)
            {
                matchText = $"{Vocab.Atk}{Vocab.GetStatusText(Atk)}";
                match++;
            }
            if (_def == other._def)
            {
                matchText = $"{Vocab.Def}{Vocab.GetStatusText(Def)}";
                match++;
            }
            return match is 1;
        }
    }
}
