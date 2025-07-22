using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media.Bms.RawData;

namespace LivreNoirLibrary.Media.Bms
{
    public sealed class FlowIf : FlowBranch, IDumpable<FlowIf>
    {
        private readonly List<FlowIfChild> _elseIfs;

        public ReadOnlySpan<FlowIfChild> ElseIfs => CollectionsMarshal.AsSpan(_elseIfs);
        public FlowIfChild? Else { get; internal set; }

        public override string DumpHeader => $"{FlowTexts.If} {Index}";
        public override string DumpChid => FlowTexts.Chid_If;

        private FlowIf(int index, int dataId, List<FlowContainer> flows, List<FlowIfChild> elseifs, FlowIfChild? @else) : base(index, dataId, flows)
        {
            _elseIfs = elseifs;
            Else = @else;
        }
        internal static FlowIf Create(int index, int dataId) => new(index, dataId, [], [], null);

        internal void AddElseIf(FlowIfChild child) => _elseIfs.Add(child);
        internal bool RemoveElseIf(FlowIfChild child) => _elseIfs.Remove(child);

        public override IEnumerable<FlowData> EachData(BmsData root)
        {
            foreach (var data in base.EachData(root))
            {
                yield return data;
            }
            var list = _elseIfs;
            for (var i = 0; i < list.Count; i++)
            {
                foreach (var d in list[i].EachData(root))
                {
                    yield return d;
                }
            }
            if (Else is not null)
            {
                foreach (var d in Else.EachData(root))
                {
                    yield return d;
                }
            }
        }

        public bool TryGetElse(int index, [MaybeNullWhen(false)] out FlowIfChild branch)
        {
            foreach (var elif in ElseIfs)
            {
                if (elif.Index == index)
                {
                    branch = elif;
                    return true;
                }
            }
            branch = Else;
            return branch is not null;
        }

        internal override void Dump_Footer(BmsTextWriter writer, BmsData root)
        {
            foreach (var elif in ElseIfs)
            {
                elif.Dump(writer, root);
            }
            Else?.Dump(writer, root);
            writer.Dump(FlowTexts.EndIf);
        }

        protected override void Dump_Content(BinaryWriter writer)
        {
            base.Dump_Content(writer);
            writer.Write(_elseIfs.Count);
            foreach (var elif in ElseIfs)
            {
                elif.Dump(writer);
            }
            if (Else is not null)
            {
                Else.Dump(writer);
            }
            else
            {
                writer.WriteChid(FlowTexts.Chid_IfNone);
            }
        }

        public static FlowIf Load(BinaryReader reader)
        {
            var note = LoadNote(reader);
            var (index, dataId, flows) = LoadBasic(reader);
            var count = reader.ReadInt32();
            List<FlowIfChild> elseifs = [];
            for (var i = 0; i < count; i++)
            {
                if (AutoLoad(reader) is FlowIfChild c)
                {
                    elseifs.Add(c);
                }
            }
            var @else = AutoLoad(reader) as FlowIfChild;

            return new(index, dataId, flows, elseifs, @else)
            {
                Note = note,
            };
        }

        internal override void SetRandom(FlowAddress currentAddress, FlowAddressList fixedAddress)
        {
            base.SetRandom(currentAddress, fixedAddress);
            foreach (var sub in ElseIfs)
            {
                sub.SetRandom(currentAddress, fixedAddress);
            }
            Else?.SetRandom(currentAddress, fixedAddress);
        }
    }
}
