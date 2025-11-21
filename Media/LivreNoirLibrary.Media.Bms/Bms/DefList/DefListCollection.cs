using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class DefListCollection : SortedList<DefType, IDefList>, IDefListCollection
    {
        public bool TryGetList(DefType type, [MaybeNullWhen(false)] out IDefList list)
        {
            if (TryGetValue(type, out var defList))
            {
                list = defList;
                return true;
            }
            list = null;
            return false;
        }

        public IDefList GetOrAddList(DefType type)
        {
            if (!TryGetValue(type, out var defList))
            {
                defList = new DefList();
                Add(type, defList);
            }
            return defList;
        }

        public bool RemoveList(DefType type) => Remove(type);

        public IEnumerable<(DefType, IDefList)> EnumerateList()
        {
            foreach (var (key, value) in this)
            {
                yield return (key, value);
            }
        }
    }
}
