using System;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public readonly record struct StructNote(NoteType Type, short Lane, short Value)
    {
        public bool Equals(NoteType type, short lane, short value) => type == Type && lane == Lane && value == Value;
    }
}
