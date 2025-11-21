using System;

namespace LivreNoirLibrary.Media.FFmpeg
{
    internal interface IAudioBufferEncoder : IAudioBufferInternal
    {
        /// <summary>
        /// Encode buffer, write to the output stream, set <see cref="IAudioBuffer.BufferIndex"/> to 0, and clear buffer.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if successed to write. <see langword="false"/> if no samples encoded.
        /// </returns>
        bool EncodeBuffer();
    }
}
