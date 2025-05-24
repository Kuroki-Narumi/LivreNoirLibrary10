using System;
using System.Collections.Generic;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media.Bms.RawData;

namespace LivreNoirLibrary.Media.Bms
{
    public sealed class FlowCase : FlowBranch, IDumpable<FlowCase>
    {
        public bool Skip { get; set; } = true;
        public bool IsDefault => Index is FlowTexts.DefaultIndex;

        public override string DumpHeader => Index is FlowTexts.DefaultIndex ? FlowTexts.Default : $"{FlowTexts.Case} {Index}";
        public override string DumpChid => FlowTexts.Chid_Case;

        private FlowCase(int index, int dataId, List<FlowContainer> flows) : base(index, dataId, flows) { }
        internal static FlowCase Create(int index, int dataId) => new(index, dataId, []);

        internal override void Dump_Content(BmsTextWriter writer, BmsData root)
        {
            if (Skip)
            {
                writer.Dump(FlowTexts.Skip);
            }
        }

        protected override void Dump_Content(BinaryWriter writer)
        {
            base.Dump_Content(writer);
            writer.Write(Skip);
        }

        public static FlowCase Load(BinaryReader reader)
        {
            var note = LoadNote(reader);
            var (index, dataId, flows) = LoadBasic(reader);
            var skip = reader.ReadBoolean();

            return new(index, dataId, flows)
            {
                Note = note,
                Skip = skip,
            };
        }
    }
}
