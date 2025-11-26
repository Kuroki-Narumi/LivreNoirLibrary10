using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IDefListCollection : ICount, IClear
    {
        bool TryGetList(DefType type, [MaybeNullWhen(false)] out IDefList defList);
        IDefList GetOrAddList(DefType type);
        bool RemoveList(DefType type);
        IEnumerable<(DefType, IDefList)> EnumerateList();
    }
}
