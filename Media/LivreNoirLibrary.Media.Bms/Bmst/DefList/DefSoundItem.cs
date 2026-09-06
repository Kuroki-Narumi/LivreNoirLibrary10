using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Media.Bmst
{
    public readonly struct DefSoundItem
    {
        public string? Path { get; init; }
        public int RefIndex { get; init; }
        public double Start { get; init;  }
        public double Length { get; init; }
        public double Volume { get; init; }
        public double Pan { get; init; }
        public double Pitch { get; init; }
    }
}
