using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media
{
    public static class MergedEnumerator
    {
        public static TwoMergedEnumerator<T1, T2> Create<T1, T2>(IEnumerable<(T1, T2)> left, IEnumerable<(T1, T2)> right) where T1 : IComparable<T1> => new(left, right);
    }
}
