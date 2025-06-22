using System;
using System.Collections;
using System.Collections.Generic;

namespace LivreNoirLibrary.Collections
{
    public static partial class CollectionExtensions
    {
        public static T Pop<T>(this IList<T> list)
        {
            var item = list[^1];
            list.RemoveAt(list.Count - 1);
            return item;
        }
    }
}
