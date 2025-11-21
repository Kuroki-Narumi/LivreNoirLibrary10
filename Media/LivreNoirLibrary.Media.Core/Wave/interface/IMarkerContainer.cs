using System;

namespace LivreNoirLibrary.Media.Wave
{
    public interface IMarkerContainer
    {
        MarkerCollection Markers { get; }
        int Length { get; }
    }
}
