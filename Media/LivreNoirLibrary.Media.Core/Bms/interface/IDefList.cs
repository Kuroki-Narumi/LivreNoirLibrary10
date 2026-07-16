using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IDefList : ISafeEnumerable<(short, string)>, ICount, IClear, IDumpable, ILoadable
    {
        int MaxIndex { get; }
        IEnumerable<short> Keys { get; }
        bool ContainsKey(short key);
        bool TryGetValue(short key, [MaybeNullWhen(false)] out string value);
        bool TryGetKey(string value, out short key);
        void Set(short key, string? value);
        bool Remove(short key);
    }
}
