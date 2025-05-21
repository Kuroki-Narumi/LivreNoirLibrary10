using System;
using System.IO;

namespace LivreNoirLibrary.Media.FFmpeg
{
    internal interface IAudioBufferDecoder : IAudioBuffer
    {
        /// <summary>
        /// The position(samples per channel) at the beginning of the buffer.
        /// </summary>
        long BufferHead { get; }

        /// <summary>
        /// Setup the decoder.
        /// </summary>
        /// <param name="stream">Input stream.</param>
        /// <param name="leaveOpen">Whether the decoder should close the input stream on disposal.</param>
        /// <param name="streamIndex">The index of internal stream.<br/>If &lt; 0, the decoder looks for the first available stream.</param>
        /// <param name="outSampleRate">Desired output sample rate.<br/>If &lt;= 0 or not supported value, the decoder sets it to the same as the input.</param>
        /// <param name="outChannels">Desired number of output channels.<br/>If &lt;= 0 or not supported value, the decoder sets it to the same as the input</param>
        /// <returns><see cref="bool">true</see> if the setup succeeded, otherwise <see cref="bool">false</see>.</returns>
        bool Setup(Stream stream, bool leaveOpen, int streamIndex, int outSampleRate, int outChannels);

        /// <summary>
        /// Try to seek the base stream to the specified position(approx. output samples per channel).
        /// </summary>
        /// <param name="position">Required position(samples per channel)</param>
        /// <returns>
        /// The actual position(approx. output samples per channel) after seek.
        /// </returns>
        void UnsafeSampleSeek(long position);

        /// <summary>
        /// Decode the input stream, write to buffer, and update <see cref="IAudioBuffer.BufferHead"/> and <see cref="IAudioBuffer.BufferLength"/>
        /// </summary>
        /// <returns>
        /// <see cref="bool">false</see> if successed to read. <see cref="bool">true</see> if the stream has reached its end. 
        /// </returns>
        bool UpdateBuffer();
    }
}
