using System;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public interface IVideoContext
    {
        int InputWidth { get; }
        int InputHeight { get; }
        int OutputWidth { get; }
        int OutputHeight { get; }
        Rational FrameRate { get; }
    }
}
