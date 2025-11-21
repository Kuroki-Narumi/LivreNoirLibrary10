using System;
using System.IO;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public interface IPositionOperator<T> : IComparer<T, T>
    {
        abstract static T Zero { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">The first object to operate.</param>
        /// <param name="y">The second object to operate.</param>
        /// <returns>
        /// a <typeparamref name="T"/> value of addition <paramref name="x"/> and <paramref name="y"/>.
        /// </returns>
        abstract static T Add(T x, T y);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">The first object to operate.</param>
        /// <param name="y">The second object to operate.</param>
        /// <returns>
        /// a <typeparamref name="T"/> value of subtraction <paramref name="y"/> from <paramref name="x"/>.
        /// </returns>
        abstract static T Subtract(T x, T y);

        abstract static void Write(BinaryWriter writer, T value);

        abstract static T Read(BinaryReader reader);
    }
}
