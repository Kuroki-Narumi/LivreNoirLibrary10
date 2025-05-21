using System;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public interface IVideoEncoder : IVideoContext, IEncodeContext
    {
        /// <summary>
        /// Encode <paramref name="buffer"/> and write to the output stream.<br/>
        /// The encoder will set the frame duration to the specified value and advance the timestamp accordingly.
        /// </summary>
        /// <param name="buffer">The buffer to read.</param>
        /// <param name="duration">The frame duration(in second).<br/>
        /// If &lt;= 0, the encoder will estimate this from the frame rate.</param>
        /// <exception cref="ArgumentException"/>
        void Write(ReadOnlySpan<byte> buffer, Rational duration);
    }
}
