using System;

namespace LivreNoirLibrary.Text
{
    public static class ToStringFactory<T>
    {
        public static readonly Func<T, string?> Instance = obj => obj?.ToString();
    }
}
