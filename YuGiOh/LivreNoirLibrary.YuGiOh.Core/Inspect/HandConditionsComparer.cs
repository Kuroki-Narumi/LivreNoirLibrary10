using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Inspect
{
    public class HandConditionsComparer : IComparer<HandConditions>
    {
        public static HandConditionsComparer Default { get; } = new();

        public int Compare(HandConditions? x, HandConditions? y)
        {
            if (x is null)
            {
                return y is null ? 0 : -1;
            }
            if (y is null)
            {
                return 1;
            }
            var c = x.GroupId.CompareTo(y.GroupId);
            if (c is not 0)
            {
                return c;
            }
            c = y.Value1.CompareTo(x.Value1);
            if (c is not 0)
            {
                return c;
            }
            return y.Value2.CompareTo(x.Value2);
        }
    }
}
