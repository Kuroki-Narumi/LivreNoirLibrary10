using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public static class FlowAddressList
    {
        public static bool TryGetBranchIndex(this List<FlowAddress> list, FlowAddress flowAddress, out int index)
        {
            foreach (var item in list.AsSpan())
            {
                if (flowAddress.IsParentOf(item))
                {
                    index = item[flowAddress.Length];
                    return true;
                }
            }
            index = -1;
            return false;
        }

        public static List<FlowAddress> Clone(this List<FlowAddress> list) => [.. list];
    }
}
