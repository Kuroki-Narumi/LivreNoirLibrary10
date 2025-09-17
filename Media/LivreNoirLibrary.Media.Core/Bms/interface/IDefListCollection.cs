using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IDefListCollection
    {
        public bool Contains(DefType type);
        public bool TryGetList(DefType type, [MaybeNullWhen(false)] out IDefList defList);
        public IDefList GetOrAddList(DefType type);
        public bool RemoveList(DefType type);
        public IEnumerable<(DefType, IDefList)> EnumerateList();
    }
}
