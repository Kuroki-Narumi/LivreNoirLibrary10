using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public readonly struct GetRandomState
    {
        public readonly RandomProvider Random;
        public readonly FlowAddressList FixedAddress;
        public readonly FlowAddress CurrentAddress;
        public readonly List<FlowBranch> BranchList;

        public GetRandomState(RandomProvider provider, FlowAddressList? fixedAddress = null)
        {
            Random = provider;
            FixedAddress = fixedAddress ?? [];
            CurrentAddress = FlowAddress.Empty;
            BranchList = [];
        }

        public GetRandomState(GetRandomState previous, int appendIndex)
        {
            Random = previous.Random;
            FixedAddress = previous.FixedAddress;
            CurrentAddress = previous.CurrentAddress.Append(appendIndex);
            BranchList = previous.BranchList;
        }

        public GetRandomState Append(int index) => new(this, index);
    }
}
