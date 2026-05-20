using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public class BmsData : IBmsData, IFile<BmsData>
    {
        public static BmsData Create()
        {
            BmsData data = new();
            data.Root.MainHeaders.SetDefault();
            return data;
        }

        private readonly List<BaseData> _dataList = [new()];
        private readonly SortedSet<int> _freeDataIndex = [];

        public ChartType ChartType { get; set; }
        public int LnObj { get; set; }

        public IBmsDataUnit Root => _dataList[0];

        public void Clear()
        {
            var list = _dataList;
            var free = _freeDataIndex;
            var c = list.Count;
            list[0].Clear();
            for (var i = 1; i < c; i++)
            {
                list[i].Clear();
                free.Add(i);
            }
            LnObj = 0;
        }

        public IBmsDataUnit GetBranchData(FlowBranch branch)
        {
            var list = _dataList;
            if (branch == FlowBranch.Root)
            {
                return list[0];
            }
            var index = branch.DataIndex;
            if ((uint)(index - 1) <= (uint)(list.Count - 1))
            {
                return list[index];
            }
            var free = _freeDataIndex;
            if (free.Count is > 0)
            {
                index = free.Min;
                free.Remove(index);
            }
            else
            {
                index = list.Count;
                list.Add(new());
            }
            branch.DataIndex = index;
            return list[index];
        }

        public bool TryGetBranch(FlowAddress address, [MaybeNullWhen(false)]out FlowContainer flow, [MaybeNullWhen(false)]out IBmsDataUnit data)
        {
            var span = address.AsSpan();
            data = Root;
            var max = (span.Length / 2) * 2;
            flow = null;
            for (var i = 0; i < max; i++)
            {
                var flows = data.Flows;
                var flowIndex = span[i];
                if (flowIndex >= flows.Count)
                {
                    return false;
                }
                flow = flows[flowIndex];
                i++;
                if (flow.GetBranch(span[i]) is { } branch)
                {
                    data = GetBranchData(branch);
                }
                else
                {
                    return false;
                }
            }
            return data is not null;
        }

        public bool InsulateBranch(FlowBranch branch)
        {
            var list = _dataList;
            var index = branch.DataIndex;
            if ((uint)(index - 1) <= (uint)(list.Count - 1))
            {
                list[index].Clear();
                branch.DataIndex = -1;
                _freeDataIndex.Add(index);
                return true;
            }
            return false;
        }

        public void WriteHistoryData(Stream stream)
        {
            using BinaryWriter writer = new(stream, Encoding.UTF8, true);

            writer.Write((byte)ChartType);
            writer.Write((short)LnObj);

            var free = _freeDataIndex;
            writer.Write(free.Count);
            foreach (var i in free)
            {
                writer.Write(i);
            }

            var list = _dataList;
            writer.Write(list.Count);
            foreach (var data in list.AsSpan())
            {
                data.DumpMain(writer);
            }
        }

        public void ReadHistoryData(Stream stream)
        {
            using BinaryReader reader = new(stream, Encoding.UTF8, true);

            ChartType = (ChartType)reader.ReadByte();
            LnObj = reader.ReadInt16();

            var free = _freeDataIndex;
            free.Clear();
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                free.Add(reader.ReadInt32());
            }

            var list = _dataList;
            count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                BaseData data;
                if (i >= list.Count)
                {
                    data = new();
                    list.Add(data);
                }
                else
                {
                    data = list[i];
                }
                data.LoadMain(reader, this);
            }
            var c = list.Count;
            for (var i = count; i < c; i++)
            {
                list[i].Clear();
                free.Add(i);
            }
        }

        public static BmsData Open(string path)
        {
            var data = General.Open(path, Load);
            var ext = Path.GetExtension(path);
            if (ExtRegs.Pms.IsMatch(ext))
            {
                data.ChartType = ChartType.Popn;
            }
            else if (ExtRegs.Bmg.IsMatch(ext))
            {
                data.ChartType = ChartType.Keyboard;
            }
            else
            {
                data.ChartType = ChartType.Beat;
            }
            return data;
        }

        public static BmsData Load(Stream stream)
        {
            BmsData result = new();
            BmsParser parser = new(stream);
            parser.Parse(result);
            return result;
        }

        public void Save(string path) => Save(path, false, true);
        public void Save(string path, bool indent = false, bool ext = true) => General.Save(path, s => Dump(s, indent), ext ? ExtRegs.BeMusic : null, Exts.Bms);

        public void Dump(Stream stream, bool indent)
        {
            BmsFormatter formatter = new(this);
            formatter.Prepare(Root);
            formatter.Format(stream, null, indent);
        }
    }
}
