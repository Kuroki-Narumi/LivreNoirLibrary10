using System;
using System.Collections.Generic;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public abstract class FlowItem
    {
        public string? Note { get; set; }

        public abstract string DumpHeader { get; }
        public abstract string DumpChid { get; }

        internal void Dump(RawData.BmsTextWriter writer, BmsData root)
        {
            writer.Dump(Note);
            writer.Dump(DumpHeader);
            writer.IndentRight();
            Dump_Content(writer, root);
            writer.IndentLeft();
            Dump_Footer(writer, root);
        }

        internal virtual void Dump_Content(RawData.BmsTextWriter writer, BmsData root) { }
        internal virtual void Dump_Footer(RawData.BmsTextWriter writer, BmsData root) { }

        public void Dump(BinaryWriter writer)
        {
            writer.WriteChid(DumpChid);
            if (string.IsNullOrEmpty(Note))
            {
                writer.Write7BitEncodedInt(0);
            }
            else
            {
                writer.Write(Note);
            }
            Dump_Content(writer);
        }

        protected virtual void Dump_Content(BinaryWriter s) { }

        public abstract IEnumerable<FlowData> EachData(BmsData root);

        public static IEnumerable<FlowData> EachData(BmsData root, List<FlowContainer> flows)
        {
            for (var i = 0; i < flows.Count; i++)
            {
                foreach (var data in flows[i].EachData(root))
                {
                    yield return data;
                }
            }
        }

        protected static string? LoadNote(BinaryReader reader) => reader.ReadString().GetNullIfEmpty();

        public static int AddFlow(List<FlowContainer> flows, FlowContainer flow)
        {
            var index = flows.Count;
            flows.Add(flow);
            return index;
        }

        public static void GetRandom(BmsData root, List<FlowContainer> flows, RandomProvider random, FlowAddress currentAddress, FlowAddressList fixedAddress, List<FlowData> dataList)
        {
            for (var i = 0; i < flows.Count; i++)
            {
                flows[i].GetRandom(root, random, currentAddress.Append(i), fixedAddress, dataList);
            }
        }

        protected static void GetRandom_AddBranch(BmsData root, FlowBranch branch, RandomProvider random, FlowAddress currentAddress, FlowAddressList fixedAddress, List<FlowData> dataList)
        {
            if (root.TryGetFlowData(branch.DataId, out var data))
            {
                dataList.Add(data);
            }
            GetRandom(root, branch._flows, random, currentAddress, fixedAddress, dataList);
        }

        public static void SetRandom(List<FlowContainer> flows, FlowAddress currentAddress, FlowAddressList fixedAddress)
        {
            for (var i = 0; i < flows.Count; i++)
            {
                flows[i].SetRandom(currentAddress.Append(i), fixedAddress);
            }
        }
    }
}
