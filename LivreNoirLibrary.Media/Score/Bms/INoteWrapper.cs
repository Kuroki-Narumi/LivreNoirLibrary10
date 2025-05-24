using System;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public interface INoteWrapper
    {
        public BarPosition Position { get; }
        public Rational ActualPosition { get; }
        public Note Note { get; }
    }
}
