using System;

namespace LivreNoirLibrary.Media.Bms
{
    public readonly struct TempoInfo<T>(T tempo, T since, T until, bool isLast = false)
    {
        /// <summary>
        /// tempo value in bpm (beats per second)
        /// </summary>
        public T Tempo { get; } = tempo;
        /// <summary>
        /// elapsed time this <see cref="TempoInfo{T}"/> applies.
        /// </summary>
        public T Since { get; } = since;
        /// <summary>
        /// elapsed time the next <see cref="TempoInfo{T}"/> will apply.
        /// </summary>
        public T Until { get; } = until;
        /// <summary>
        /// true if this <see cref="TempoInfo{T}"/> is the last one of the song.
        /// </summary>
        public bool IsLast { get; } = isLast;
    }
}
