using System;
using System.IO;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public interface IPositionOperator<T>
    {
        public static abstract T Zero { get; }

        /// <inheritdoc cref="IComparer{T1,T2}.Equals"/>
        public bool Equals(T x, T y);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">The first object to operate.</param>
        /// <param name="y">The second object to operate.</param>
        /// <returns>
        /// a <typeparamref name="T"/> value of addition <paramref name="x"/> and <paramref name="y"/>.
        /// </returns>
        public T Add(T x, T y);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">The first object to operate.</param>
        /// <param name="y">The second object to operate.</param>
        /// <returns>
        /// a <typeparamref name="T"/> value of subtraction <paramref name="y"/> from <paramref name="x"/>.
        /// </returns>
        public T Subtract(T x, T y);

        public void Write(BinaryWriter writer, T value);
        public T Read(BinaryReader reader);
    }
}
