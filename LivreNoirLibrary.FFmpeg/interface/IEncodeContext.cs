using System;

namespace LivreNoirLibrary.Media
{
    public interface IEncodeContext
    {
        /// <summary>
        /// Finish encoding and send a flush packet to the encoder.
        /// </summary>
        void Flush();
    }
}
