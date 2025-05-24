using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LivreNoirLibrary.Text
{
    public static class StringPool
    {
        private static readonly HashSet<string> _pool = [];

        [return: NotNullIfNotNull(nameof(str))]
        public static string? Get(string? str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            if (_pool.TryGetValue(str, out var pool))
            {
                return pool;
            }
            else
            {
                _pool.Add(str);
                return str;
            }
        }
    }
}
