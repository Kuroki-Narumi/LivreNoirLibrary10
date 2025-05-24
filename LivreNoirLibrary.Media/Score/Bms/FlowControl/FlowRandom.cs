using System;
using System.Collections.Generic;
using System.IO;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.Bms
{
    public sealed class FlowRandom : FlowContainer, IDumpable<FlowRandom>
    {
        public override string DumpHeader => $"{(IsFixed ? FlowTexts.SetRandom : FlowTexts.Random)} {Max}";
        public override string DumpFooter => FlowTexts.EndRandom;
        public override string DumpChid => FlowTexts.Chid_Random;

        private FlowRandom(int max, bool isFixed, List<FlowBranch> children) : base(max, isFixed, children) { }
        internal static FlowRandom Create(int max, bool isFixed) => new(max, isFixed, []);

        public static FlowRandom Load(BinaryReader reader)
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
            foreach (var branch in Branches)
            {
                if (branch.Index == branchIndex)
                {
                    GetRandom_AddBranch(root, branch, random, currentAddress, fixedAddress, dataList);
                }
                else if (branch is FlowIf f && f.TryGetElse(branchIndex, out var @else))
                {
                    GetRandom_AddBranch(root, @else, random, currentAddress, fixedAddress, dataList);
                }
            }
        }
    }
}
