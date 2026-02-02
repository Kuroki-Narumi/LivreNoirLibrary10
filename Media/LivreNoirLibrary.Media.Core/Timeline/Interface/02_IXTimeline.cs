using System;
using System.Collections;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public interface IXTimeline<TX, TValue> : ITimeline<TX>, IEnumerable<(TX, TValue)>
        where TX : struct
    {
        /// <summary>
        /// Returns an enumerable object that enumerates the the position and item pairs within the specified range.
        /// </summary>
        /// <param name="range">the range of positions to iterate.</param>
        /// <returns>an enumerable that can be used to iterate within the specified range.</returns>
        IEnumerable<(TX, TValue)> Range(Range<TX> range);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
