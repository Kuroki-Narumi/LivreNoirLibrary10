using LivreNoirLibrary.IO;
using System;
using System.Collections.Generic;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IBarLengthCollection : IEnumerable<(short, double)>, IClear, IDumpable, ILoadable
    {
        /// <summary>
        /// Attempts to retrieve the length of the specified bar.
        /// </summary>
        /// <param name="number">The bar number to get the length.</param>
        /// <param name="value">When this method returns, contains the length of the specified bar, if defined;
        /// otherwise, default value of <see langword="double"/>.</param>
        /// <returns><see langword="true"/> if the length of the specified bar is defined; otherwise, <see langword="false"/>.</returns>
        bool TryGetValue(int number, out double value);

        /// <summary>
        /// Sets the length of the specified bar to the specified value.
        /// </summary>
        /// <param name="number">The bar number to set the length.</param>
        /// <param name="value">The length to set. If 0, remove the definition of the length of the specified bar.</param>
        /// <returns><see langword="true"/> if the length of the specified bar is changed; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        bool Set(int number, double value);

        /// <summary>
        /// Removes the definition of the length of the specified bar.
        /// </summary>
        /// <param name="number">The bar number to remove the definition.</param>
        /// <returns><see langword="true"/> if the definition of the specified bar is successfully removed; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        bool Remove(int number);

        void Insert(int number, int count);

        void Delete(int number, int count);

        void Merge(IBarLengthCollection source);

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
