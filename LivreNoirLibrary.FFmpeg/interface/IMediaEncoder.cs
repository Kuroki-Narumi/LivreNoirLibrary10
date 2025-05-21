using System;

namespace LivreNoirLibrary.Media
{
    public interface IMediaEncoder
    {
        /// <summary>
        /// Finish initializing and write header to the output stream.
        /// </summary>
        void WriteHeader();
        /// <summary>
        /// Finish encoding and write footer to the output stream.
        /// </summary>
        void WriteTrailer();
    }
}
