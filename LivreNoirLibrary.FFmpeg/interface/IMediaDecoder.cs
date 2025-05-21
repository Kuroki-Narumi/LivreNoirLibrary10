using System;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public interface IMediaDecoder
    {
        /// <summary>
        /// Approximate output duration(in seconds).
        /// </summary>
        Rational Duration { get; }

        /// <summary>
        /// Try to seek the base stream to the specified position(in seconds).
        /// </summary>
        /// <param name="position">Required position(in seconds)</param>
        void Seek(Rational position);
    }
}
