using System;
using LivreNoirLibrary.Media.FFmpeg;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Windows.Media
{
    public readonly ref struct VideoFrameInfo(Span<byte> buffer, Rational pts)
    {
        public readonly Span<byte> Buffer = buffer;
        public readonly long Tick = pts.ToTicks();
    }
}
