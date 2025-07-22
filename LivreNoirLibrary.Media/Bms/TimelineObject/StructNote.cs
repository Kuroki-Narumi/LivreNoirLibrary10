using System;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public readonly struct StructNote
    {
        private readonly NoteType _type;
        private readonly short _lane;
        private readonly Rational _value;

        internal StructNote(NoteType type, short lane, Rational value)
        {
            _type = type;
            _lane = lane;
            _value = value;
        }

        public bool Equals(NoteType type, short lane, Rational value) => type == _type && lane == _lane && value == _value;

        public override string ToString() => $"(Type:{_type}, Lane:{_lane}, Value:{_value})";
    }
}
