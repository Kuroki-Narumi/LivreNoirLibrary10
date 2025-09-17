using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IHeaderCollection
    {
        public List<(string Key, string Value)> SubHeaders { get; }

        public bool TryGetNumber(HeaderType type, out double value);
        public bool TryGetEnum<T>(HeaderType type, out T value) where T : struct, Enum;
        public bool TryGetText(HeaderType type, [MaybeNullWhen(false)] out string value);

        public void Set(HeaderType type, double value);
        public void Set<T>(HeaderType type, T value) where T : struct, Enum;
        public void Set(HeaderType type, string value);

        public bool Remove(HeaderType type);
    }
}
