using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media.Bms.RawData;

namespace LivreNoirLibrary.Media.Bms
{
    public abstract class FlowContainer(int max, bool isFixed, List<FlowBranch> branches) : FlowItem
    {
        protected readonly List<FlowBranch> _branches = branches;

        public int Max { get; set; } = max;
        public bool IsFixed { get; set; } = isFixed;
        public ReadOnlySpan<FlowBranch> Branches => CollectionsMarshal.AsSpan(_branches);

        public abstract string DumpFooter { get; }

        public FlowBranch? GetBranch(int index) => _branches.Find(b => b.Index == index);

        internal virtual void AddChild(FlowBranch child) => _branches.Add(child);
        internal bool RemoveChild(FlowBranch child) => _branches.Remove(child);

        public override IEnumerable<FlowData> EachData(BmsData root)
        {
            var list = _branches;
            for (var i = 0; i < list.Count; i++)
            {
                foreach (var data in list[i].EachData(root))
                {
                    yield return data;
                }
            }
        }

        internal override void Dump_Content(BmsTextWriter writer, BmsData root)
        {
            foreach (var child in Branches)
            {
                child.Dump(writer, root);
            }
        }

        internal override void Dump_Footer(BmsTextWriter writer, BmsData root)
        {
            writer.Dump(DumpFooter);
        }

        protected override void Dump_Content(BinaryWriter writer)
        {
            writer.Write(Max);
            writer.Write(IsFixed);
            writer.Write(_branches.Count);
            foreach (var child in Branches)
            {
                child.Dump(writer);
            }
        }

        protected static (int Max, bool IsFixed, List<FlowBranch> Children) LoadBasic(BinaryReader reader)
        {
            var max = reader.ReadInt32();
            var fix = reader.ReadBoolean();
            var count = reader.ReadInt32();
            List<FlowBranch> children = [];
            for (var i = 0; i < count; i++)
            {
                if (FlowBranch.AutoLoad(reader) is FlowBranch b)
                {
                    children.Add(b);
                }
            }
            return (max, fix, children);
        }

        public static FlowContainer AutoLoad(BinaryReader reader)
        {
            var chid = reader.ReadChid();
            return chid switch
            {
                FlowTexts.Chid_Random => FlowRandom.Load(reader),
                FlowTexts.Chid_Switch => FlowSwitch.Load(reader),
                _ => throw new InvalidDataException(),
            };
        }

        internal void GetRandom(BmsData root, RandomProvider random, FlowAddress currentAddress, FlowAddressList fixedAddress, List<FlowData> dataList)
        {
            if (!fixedAddress.TryGetBranchIndex(currentAddress, out var branchIndex))
            {
                branchIndex = IsFixed ? Max : random(this, Max);
            }
            GetRandom_GetBranchList(root, branchIndex, random, currentAddress.Append(branchIndex), fixedAddress, dataList);
        }

        protected abstract void GetRandom_GetBranchList(BmsData root, int branchIndex, RandomProvider random, FlowAddress currentAddress, FlowAddressList fixedAddress, List<FlowData> dataList);

        internal void SetRandom(FlowAddress currentAddress, FlowAddressList fixedAddress)
        {
            if (fixedAddress.TryGetBranchIndex(currentAddress, out var branchIndex))
            {
                IsFixed = true;
                Max = branchIndex is > 0 ? branchIndex : 65536;
            }
            foreach (var branch in Branches)
            {
                branch.SetRandom(currentAddress.Append(branch.Index), fixedAddress);
            }
        }
    }
}
