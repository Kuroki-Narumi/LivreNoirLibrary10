using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media.Bms.RawData;
using LivreNoirLibrary.Debug;

namespace LivreNoirLibrary.Media.Bms
{
    public abstract class FlowBranch(int index, int dataId, List<FlowContainer> flows) : FlowItem
    {
        internal readonly List<FlowContainer> _flows = flows;

        public int Index { get; set; } = index;
        public int DataId { get; } = dataId;
        public ReadOnlySpan<FlowContainer> Flows => CollectionsMarshal.AsSpan(_flows);

        internal int AddFlow(FlowContainer flow) => AddFlow(_flows, flow);
        internal bool RemoveFlow(FlowContainer flow) => _flows.Remove(flow);

        public override IEnumerable<FlowData> EachData(BmsData root)
        {
            if (root.TryGetFlowData(DataId, out var data))
            {
                yield return data;
            }
            foreach (var d in EachData(root, _flows))
            {
                yield return d;
            }
        }

        internal override void Dump_Content(BmsTextWriter writer, BmsData root)
        {
            root.DumpFlowData(DataId, writer);
            foreach (var flow in Flows)
            {
                flow.Dump(writer, root);
            }
        }

        protected override void Dump_Content(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(DataId);
            writer.Write(_flows.Count);
            foreach (var flow in Flows)
            {
                flow.Dump(writer);
            }
        }

        protected static (int Index, int DataId, List<FlowContainer> Flows) LoadBasic(BinaryReader reader)
        {
            var index = reader.ReadInt32();
            var dataId = reader.ReadInt32();
            var count = reader.ReadInt32();
            List<FlowContainer> flows = [];
            for (var i = 0; i < count; i++)
            {
                flows.Add(FlowContainer.AutoLoad(reader));
            }
            return (index, dataId, flows);
        }

        public static FlowBranch? AutoLoad(BinaryReader reader)
        {
            var chid = reader.ReadChid();
            return chid switch
            {
                FlowTexts.Chid_If => FlowIf.Load(reader),
                FlowTexts.Chid_IfChild => FlowIfChild.Load(reader),
                FlowTexts.Chid_IfNone => null,
                FlowTexts.Chid_Case => FlowCase.Load(reader),
                _ => throw new InvalidDataException(),
            };
        }

        internal virtual void SetRandom(FlowAddress currentAddress, FlowAddressList fixedAddress) => SetRandom(_flows, currentAddress, fixedAddress);
    }
}
