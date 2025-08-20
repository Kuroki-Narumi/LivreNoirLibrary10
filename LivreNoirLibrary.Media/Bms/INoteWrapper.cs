using System;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public interface INoteWrapper
    {
        public BarPosition Position { get; }
        public Rational AbsolutePosition { get; }
        public Note Note { get; }
    }
}
