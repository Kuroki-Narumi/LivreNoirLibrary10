using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using LivreNoirLibrary.Debug;

namespace LivreNoirLibrary.Media.Bms
{
    public class FlowAddressList : List<FlowAddress>
    {
        public bool TryGetBranchIndex(FlowAddress flowAddress, out int index)
        {
            foreach (var item in CollectionsMarshal.AsSpan(this))
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

        public FlowAddress? Pop()
        {
            if (Count is > 0)
            {
                var item = this[^1];
                RemoveAt(Count - 1);
                return item;
            }
            return null;
        }

        public FlowAddressList Clone()
        {
            FlowAddressList list = [];
            list.AddRange(this);
            return list;
        }
    }

    public class FlowAddress : IEquatable<FlowAddress>
    {
        public static FlowAddress Empty { get; } = new([]);

        private readonly int[] _list;

        public int this[int index] => _list[index];
        public int Length => _list.Length;

        private FlowAddress(int[] list) => _list = list;
        public FlowAddress(int index) => _list = [index];

        public FlowAddress Append(int index) => new([.. _list, index]);

        public bool IsFlow => _list.Length % 2 is 1;
        public bool IsBranch => _list.Length % 2 is 0;

        public bool Equals(FlowAddress? other) => other is not null && _list.AsSpan().SequenceEqual(other._list);

        public override bool Equals(object? obj) => obj is FlowAddress other && Equals(other);
        public override int GetHashCode()
        {
            var code = 0;
            foreach (var index in _list)
            {
                code = HashCode.Combine(code, index.GetHashCode());
            }
            return code;
        }

        public static bool operator ==(FlowAddress left, FlowAddress right) => left.Equals(right);
        public static bool operator !=(FlowAddress left, FlowAddress right) => !left.Equals(right);

        public bool IsParentOf(FlowAddress other)
        {
            var list = other._list.AsSpan();
            var reference = _list.AsSpan();
            return reference.Length < list.Length &&
                   reference.SequenceEqual(list[0..reference.Length]);
        }

        public override string ToString()
        {
            var list = _list;
            if (list.Length is 0)
            {
                return base.ToString()!;
            }
            StringBuilder sb = new();
            sb.Append(list[0]);
            var delim = (stackalloc char[2]);
            delim[0] = ';';
            delim[1] = '-';
            for (var i = 1; i < list.Length; i++)
            {
                sb.Append(delim[i % 2]);
                sb.Append(list[1]);
            }
            return sb.ToString();
        }
    }
}
