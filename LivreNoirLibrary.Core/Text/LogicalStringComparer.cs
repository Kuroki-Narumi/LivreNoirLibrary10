using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Text
{
    public class NaturalStringComparer(bool isNullMinimum = true) : IComparer<string?>
    {
        private readonly bool _isNullMinimum = isNullMinimum;

        public int Compare(string? x, string? y) => StringExtensions.CompareByNaturalOrder(x, y, _isNullMinimum);
    }
}
