using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics;
using System.Text;

namespace LivreNoirLibrary.SandBox.Test
{
    interface IBitmap
    {
        int Width { get; }
        int Height { get; }
        Span<Vector128<float>> Pixels { get; }

        // pixel = [B, G, R, A]
        ref Vector128<float> Pixel(int x, int y) => ref Pixels[x + y * Width];
    }

    readonly struct AffineMatrix
    {
        public float M11 { get; }
        public float M21 { get; }
        public float M12 { get; }
        public float M22 { get; }
        public float OX { get; }
        public float OY { get; }
    }

    static class BitmapOperations
    {

    }
}
