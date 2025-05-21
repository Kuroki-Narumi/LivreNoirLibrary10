using System;

namespace LivreNoirLibrary.Media
{
    public interface IAudioEncoder : IAudioContext, IEncodeContext
    {
        /// <summary>
        /// Encode <paramref name="buffer"/> and write to the output stream. <br/>
        /// If the input is smaller than the frame size, no encoding may actually occur.
        /// </summary>
        /// <param name="buffer">The buffer to read.</param>
        void Write(ReadOnlySpan<float> buffer);
    }
}
