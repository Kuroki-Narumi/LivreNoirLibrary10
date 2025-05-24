using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Bms
{
    public class DefIndexCollection : Dictionary<DefType, HashSet<short>>
    {
        public void Add(DefType type, int id) => this.GetOrAdd(type).Add((short)id);

        public int MaxCount
        {
            get
            {
                var count = 0;
                foreach (var (_, set) in this)
                {
                    var c = set.Count;
                    if (c > count)
                    {
                        count = c;
                    }
                }
                return count;
            }
        }

        public bool IsEffective()
        {
            foreach (var (_, set) in this)
            {
                if (set.Count is > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
