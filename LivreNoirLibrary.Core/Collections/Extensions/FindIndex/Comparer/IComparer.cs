using System;

namespace LivreNoirLibrary.Collections
{
    public interface IComparer<T1, T2>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// <see cref="bool">true</see> if the two objects are the same; otherwise <see cref="bool">false</see>.
        /// </returns>
        public bool Equals(T1 x, T2 y);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// <see cref="bool">true</see> if <paramref name="x"/> is less than <paramref name="y"/>; otherwise <see cref="bool">false</see>.
        /// </returns>
        public bool LessThan(T1 x, T2 y);

        /// <summary>
        /// Compares two objects with the reference and returns which is closer to the reference.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="z">The reference object to compare.</param>
        /// <returns>
        /// <see cref="bool">true</see> if <paramref name="x"/> is closer to <paramref name="z"/> than <paramref name="y"/>; otherwise <see cref="bool">false</see>.
        /// </returns>
        public bool IsCloser(T1 x, T1 y, T2 z);
    }
}
