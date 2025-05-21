using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        public static IEnumerable<T[]> EachSlice<T>(this T[] array, int n)
        {
            if (n is <= 0) { yield break; }
            var len = array.Length;
            var buffer = new T[n];
            var max = len / n * n;
            for (var i = 0; i < max; i += n)
            {
                Array.Copy(array, i, buffer, 0, n);
                yield return buffer;
            }
            if (max < len)
            {
                yield return array[max..len];
            }
        }

        public static IEnumerable<T[]> EachGroup<T>(this T[] array, int n)
        {
            if (n is <= 0) { return []; }
            var llen = (array.Length + n - 1) / n;
            return EachSlice(array, llen);
        }

        public static IEnumerable<T[]> EachSlice<T>(this List<T> list, int n)
        {
            if (n is <= 0) { yield break; }
            var len = list.Count;
            var buffer = new T[n];
            var max = len / n * n;
            for (var i = 0; i < max; i += n)
            {
                CollectionsMarshal.AsSpan(list)[i..(i + n)].CopyTo(buffer.AsSpan());
                yield return buffer;
            }
            if (max < len)
            {
                yield return CollectionsMarshal.AsSpan(list)[max..len].ToArray();
            }
        }

        public static IEnumerable<T[]> EachSlice<T>(this IList<T> list, int n)
        {
            if (n is <= 0) { yield break; }
            var len = list.Count;
            var buffer = new T[n];
            var max = len / n * n;
            for (var i = 0; i < max; i += n)
            {
                Each_CopyToBuffer(list, buffer, i, n);
                yield return buffer;
            }
            if (max < len)
            {
                n = len - max;
                buffer = new T[n];
                Each_CopyToBuffer(list, buffer, max, n);
                yield return buffer;
            }
        }

        private static void Each_CopyToBuffer<T>(IList<T> source, T[] target, int i, int n)
        {
            for (var j = 0; j < n; j++)
            {
                target[j] = source[i + j];
            }
        }

        public static IEnumerable<T[]> EachGroup<T>(this List<T> list, int n)
        {
            if (n is <= 0) { return []; }
            var llen = (list.Count + n - 1) / n;
            return EachSlice(list, llen);
        }

        public static IEnumerable<T[]> EachGroup<T>(this IList<T> list, int n)
        {
            if (n is <= 0) { return []; }
            var llen = (list.Count + n - 1) / n;
            return EachSlice(list, llen);
        }
    }
}
