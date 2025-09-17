using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        public static Span<T> AsSpan<T>(this List<T> list, int start = 0, int length = -1)
        {
            if (length < 0)
            {
                length = list.Count - start;
            }
            return CollectionsMarshal.AsSpan(list).Slice(start, length);
        }

        public static T Pop<T>(this IList<T> list)
        {
            var item = list[^1];
            list.RemoveAt(list.Count - 1);
            return item;
        }
    }
}
