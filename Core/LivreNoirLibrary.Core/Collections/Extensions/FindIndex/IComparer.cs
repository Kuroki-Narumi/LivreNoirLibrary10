using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Collections
{
    public interface IComparer<T1, T2>
    {
        /// <inheritdoc cref="System.Collections.IComparer.Compare"/>
        public int Compare(T1 x, T2 y);

        /// <summary>
        /// Compares two objects with the reference and returns which is closer to the reference.
        /// </summary>
        /// <param name="x">The first object to compare. <paramref name="x"/> must be &lt; <paramref name="y"/>.</param>
        /// <param name="y">The second object to compare. <paramref name="x"/> must be &lt; <paramref name="y"/>.</param>
        /// <param name="z">The reference object to compare.</param>
        /// <returns>
        /// <see cref="bool">true</see> if <paramref name="x"/> is closer to <paramref name="z"/> than <paramref name="y"/>; otherwise <see cref="bool">false</see>.
        /// </returns>
        public bool IsXCloserThanY(T1 x, T1 y, T2 z);
    }
}
