using System;

namespace LivreNoirLibrary.GameOfLife
{
    public readonly record struct Position(int X, int Y) : IComparable<Position>
    {
        public int CompareTo(Position other) => Y != other.Y ? Y.CompareTo(other.Y) : X.CompareTo(other.X);
    }
}
