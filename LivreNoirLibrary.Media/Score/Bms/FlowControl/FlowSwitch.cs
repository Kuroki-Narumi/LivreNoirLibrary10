using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.Bms
{
    public sealed class FlowSwitch : FlowContainer, IDumpable<FlowSwitch>
    {
        public override string DumpHeader => $"{(IsFixed ? FlowTexts.SetSwitch : FlowTexts.Switch)} {Max}";
        public override string DumpFooter => FlowTexts.EndSwitch;
        public override string DumpChid => FlowTexts.Chid_Switch;

        private FlowSwitch(int max, bool isFixed, List<FlowBranch> children) : base(max, isFixed, children) { }
        internal static FlowSwitch Create(int max, bool isFixed) => new(max, isFixed, []);

        public bool ContainsDefault => _branches.Any(c => c.Index is FlowTexts.DefaultIndex);

        public bool TryGetDefault([MaybeNullWhen(false)] out FlowCase branch)
        {
            foreach (var child in Branches)
            {
                if (child is FlowCase c && c.IsDefault)
                {
                    branch = c;
                    return true;
                }
            }
            branch = null;
            return false;
        }

        internal override void AddChild(FlowBranch item)
        {
            var branches = _branches;
            if (branches.Count is 0 || 
                item.Index is FlowTexts.DefaultIndex || 
                branches[^1].Index is not FlowTexts.DefaultIndex)
            {
                branches.Add(item);
            }
            else
            {
                branches.Insert(branches.Count - 1, item);
            }
        }

        public static FlowSwitch Load(BinaryReader reader)
        {
            var note = LoadNote(reader);
            var (max, isFixed, children) = LoadBasic(reader);

            return new(max, isFixed, children)
            {
                Note = note,
            };
        }

        protected override void GetRandom_GetBranchList(BmsData root, int branchIndex, RandomProvider random, FlowAddress currentAddress, FlowAddressList fixedAddress, List<FlowData> dataList)
        {
            var through = false;
            foreach (var branch in Branches)
            {
                if (through || branch.Index == branchIndex)
                {
                    GetRandom_AddBranch(root, branch, random, currentAddress, fixedAddress, dataList);
                    if (branch is FlowCase { Skip: true })
                    {
                        break;
                    }
                    else
                    {
                        through = true;
                    }
                }
            }
            if (!through && TryGetDefault(out var c))
            {
                GetRandom_AddBranch(root, c, random, currentAddress, fixedAddress, dataList);
            }
        }
    }
}
