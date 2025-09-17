using System;

namespace LivreNoirLibrary.Media.Wave
{
    public interface IMarker
    {
        public MarkerCollection Markers { get; }
        public int Length { get; }
    }
}
