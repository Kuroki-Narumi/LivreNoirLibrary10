using System;

namespace LivreNoirLibrary.Media.FFmpeg
{
    internal interface IAudioBufferEncoder : IAudioBuffer
    {
        /// <summary>
        /// Encode buffer, write to the output stream, set <see cref="IAudioBuffer.BufferIndex"/> to 0, and clear buffer.
        /// </summary>
        /// <returns>
        /// <see cref="bool">true</see> if successed to write. <see cref="bool">false</see> if no samples encoded.
        /// </returns>
        bool EncodeBuffer();
    }
}
