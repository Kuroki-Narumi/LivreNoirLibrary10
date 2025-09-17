using System;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public interface IVideoContext
    {
        public int InputWidth { get; }
        public int InputHeight { get; }
        public int OutputWidth { get; }
        public int OutputHeight { get; }
        public Rational FrameRate { get; }
    }
}
