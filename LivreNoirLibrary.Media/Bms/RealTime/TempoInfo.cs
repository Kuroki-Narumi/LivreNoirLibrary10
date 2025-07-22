using System;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public readonly struct TempoInfo(Rational tempo, Rational since, Rational until, bool isLast = false)
    {
        /// <summary>
        /// tempo value in bpm (beats per second)
        /// </summary>
        public readonly Rational Tempo = tempo;
        /// <summary>
        /// elapsed time this <see cref="TempoInfo"/> applies.
        /// </summary>
        public readonly Rational Since = since;
        /// <summary>
        /// elapsed time the next <see cref="TempoInfo"/> will apply.
        /// </summary>
        public readonly Rational Until = until;
        /// <summary>
        /// true if this <see cref="TempoInfo"/> is the last one of the song.
        /// </summary>
        public readonly bool IsLast = isLast;
    }
}
