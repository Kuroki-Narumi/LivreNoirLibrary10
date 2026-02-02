using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public interface IListEnumerable<TX, TValue> : IPositionRange<TX>
        where TX : struct
    {
        /// <summary>
        /// Returns an enumerable object that enumerates the the position and item list pairs.
        /// </summary>
        /// <returns>an enumerable that can be used to iterate.</returns>
        IEnumerable<(TX, List<TValue>)> EnumerateList();

        /// <summary>
        /// Returns an enumerable object that enumerates the the position and item list pairs within the specified range.
        /// </summary>
        /// <param name="range">the range of positions to iterate.</param>
        /// <returns>an enumerable that can be used to iterate within the specified range.</returns>
        IEnumerable<(TX, List<TValue>)> EnumerateList(Range<TX> range);

        /// <summary>
        /// Returns an enumerable object that enumerates the the position and item list pairs in reverse order.
        /// </summary>
        /// <returns>an enumerable that can be used to iterate.</returns>
        IEnumerable<(TX, List<TValue>)> ReverseEnumerateList();

        /// <summary>
        /// Returns an enumerable object that enumerates the the position and item list pairs within the specified range in reverse order.
        /// </summary>
        /// <param name="range">the range of positions to iterate.</param>
        /// <returns>an enumerable that can be used to iterate within the specified range.</returns>
        IEnumerable<(TX, List<TValue>)> ReverseEnumerateList(Range<TX> range);
    }
}
