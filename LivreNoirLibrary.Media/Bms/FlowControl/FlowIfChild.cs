using System;
using System.Collections.Generic;
using System.IO;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.Bms
{
    public sealed class FlowIfChild : FlowBranch, IDumpable<FlowIfChild>
    {
        public override string DumpHeader => Index is FlowTexts.DefaultIndex ? FlowTexts.Else : $"{FlowTexts.ElseIf} {Index}";
        public override string DumpChid => FlowTexts.Chid_IfChild;

        private FlowIfChild(int index, int dataId, List<FlowContainer> flows) : base(index, dataId, flows) { }
        internal static FlowIfChild Create(int index, int dataId) => new(index, dataId, []);

        public static FlowIfChild Load(BinaryReader reader)
        {
            var note = LoadNote(reader);
            var (index, dataId, flows) = LoadBasic(reader);

            return new(index, dataId, flows)
            {
                Note = note,
            };
        }
    }
}
