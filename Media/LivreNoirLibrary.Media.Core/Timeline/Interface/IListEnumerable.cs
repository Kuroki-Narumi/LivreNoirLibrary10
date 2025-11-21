using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media
{
    public interface IListEnumerable<TX, TValue> : IPositionRange<TX>
    {
        /// <summary>
        /// Returns an enumerable object that enumerates the the position and item list pairs.
        /// </summary>
        /// <returns>an enumerable that can be used to iterate.</returns>
        public IEnumerable<(TX, List<TValue>)> EnumerateList();

        /// <summary>
        /// Returns an enumerable object that enumerates the the position and item list pairs in reverse order.
        /// </summary>
        /// <returns>an enumerable that can be used to iterate.</returns>
        public IEnumerable<(TX, List<TValue>)> ReverseEnumerateList();
    }
}
