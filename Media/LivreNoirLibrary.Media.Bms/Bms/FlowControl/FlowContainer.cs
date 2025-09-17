using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Media.Bms
{
    public class FlowContainer(FlowType type, int max, bool isFixed, BaseData parent) : ObjectBase
    {
        public string BmsHeader => $"{(Type is FlowType.Random ? (IsFixed ? Tags.SetRandom : Tags.Random) : (IsFixed ? Tags.SetSwitch : Tags.Switch))} {Max}";
        public string BmsFooter => Type is FlowType.Random ? Tags.EndRandom : Tags.EndSwitch;

        public FlowType Type { get; set; } = type;
        public BaseData Parent { get; } = parent;
        public int Max { get; set; } = max;
        public bool IsFixed { get; set; } = isFixed;
        public List<FlowBranch> Branches { get; } = [];
        public FlowBranch? DefaultBranch { get; set; }

        public FlowBranch GetOrCreateBranch(int condition)
        {
            if (condition is Constants.DefaultCondition)
            {
                return DefaultBranch ??= new(Parent, condition);
            }
            if (Branches.Find(b => b.Condition == condition) is not FlowBranch branch)
            {
                branch = new(Parent, condition);
                Branches.Add(branch);
            }
            return branch;
        }

        public IEnumerable<BaseData> EnumerateBranches()
        {
            foreach (var branch in Branches)
            {
                foreach (var data in branch.EachData())
                {
                    yield return data;
                }
            }
            if (DefaultBranch is { } b)
            {
                foreach (var data in b.EachData())
                {
                    yield return data;
                }
            }
        }

        public void Dump(BinaryWriter writer)
        {
            WriteNote(writer);
            writer.Write((byte)Type);
            writer.Write(Max);
            writer.Write(IsFixed);
            writer.Write(Branches.Count);
            foreach (var branch in CollectionsMarshal.AsSpan(Branches))
            {
                branch.Dump(writer);
            }
            if (DefaultBranch is { } b)
            {
                b.Dump(writer);
            }
        }

        public static FlowContainer Load(BinaryReader reader, BaseData parent)
        {
            var note = ReadNote(reader);
            var type = (FlowType)reader.ReadByte();
            var max = reader.ReadInt32();
            var isFixed = reader.ReadBoolean();
            var count = reader.ReadInt32();
            FlowContainer result = new(type, max, isFixed, parent) { Note = note };
            var branches = result.Branches;
            for (var i = 0; i < count; i++)
            {
                var branch = FlowBranch.Load(reader, parent);
                if (branch.Condition is Constants.DefaultCondition)
                {
                    result.DefaultBranch = branch;
                }
                else
                {
                    branches.Add(branch);
                }
            }
            return result;
        }

        public static void GetRandom(List<FlowContainer> flows, GetRandomState state)
        {
            var c = flows.Count;
            for (var i = 0; i < c; i++)
            {
                flows[i].GetRandom(state.Append(i));
            }
        }

        private void GetRandom(GetRandomState state)
        {
            if (!state.FixedAddress.TryGetBranchIndex(state.CurrentAddress, out var branchIndex))
            {
                branchIndex = IsFixed ? Max : state.Random(Max, Note);
            }
            foreach (var branch in CollectionsMarshal.AsSpan(Branches))
            {
                if (branch.Condition == branchIndex)
                {
                    GetRandom_AddBranch(branch, state.Append(branchIndex));
                    return;
                }
            }
            if (DefaultBranch is { } b)
            {
                GetRandom_AddBranch(b, state.Append(branchIndex));
            }
        }

        private static void GetRandom_AddBranch(FlowBranch branch, GetRandomState state)
        {
            state.BranchList.Add(branch);
            GetRandom(branch.Flows, state);
        }

        public static void SetRandom(List<FlowContainer> flows, FlowAddress currentAddress, FlowAddressList fixedAddress)
        {
            var c = flows.Count;
            for (var i = 0; i < c; i++)
            {
                flows[i].SetRandom(currentAddress.Append(i), fixedAddress);
            }
        }

        private void SetRandom(FlowAddress currentAddress, FlowAddressList fixedAddress)
        {
            if (fixedAddress.TryGetBranchIndex(currentAddress, out var branchIndex))
            {
                IsFixed = true;
                Max = branchIndex is > 0 ? branchIndex : 65536;
            }
            foreach (var branch in CollectionsMarshal.AsSpan(Branches))
            {
                SetRandom_Branch(branch, currentAddress, fixedAddress);
            }
            if (DefaultBranch is { } b)
            {
                SetRandom_Branch(b, currentAddress, fixedAddress);
            }
        }

        private static void SetRandom_Branch(FlowBranch branch, FlowAddress currentAddress, FlowAddressList fixedAddress)
        {
            SetRandom(branch.Flows, currentAddress.Append(branch.Condition), fixedAddress);
        }
    }
}
