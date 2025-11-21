using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;
using System;
using System.IO;
using System.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class BmsExtensions
    {
        extension (IBmsData root)
        {
            public string GetExtension() => root.ChartType switch
            {
                ChartType.Popn => Filters.Pms_Save,
                ChartType.Keyboard => Filters.Bmg_Save,
                _ => Filters.Bms_Save,
            };
        }

        extension (IBmsDataUnit data)
        {
            public void Clear()
            {
                data.Note = null;
                data.MainHeaders.Clear();
                data.SubHeaders.Clear();
                data.DefLists.Clear();
                data.BarDefs.Clear();
                data.Timeline.Clear();
                data.Flows.Clear();
            }

            public void Merge(IBmsDataUnit source)
            {
                data.Note += source.Note;
                data.MainHeaders.AddRange(source.MainHeaders);
                data.SubHeaders.Merge(source.SubHeaders);
                data.DefLists.Merge(source.DefLists);
                data.BarDefs.Merge(source.BarDefs);
                source.Timeline.CopyTo(data.Timeline);
            }

            public void MergeFlows(IBmsDataUnit source)
            {
                data.Flows.AddRange(source.Flows);
            }

            public void DumpMain(BinaryWriter writer)
            {
                writer.WriteNullable(data.Note);
                data.MainHeaders.Dump(writer);
                data.SubHeaders.Dump(writer);
                data.DefLists.Dump(writer);
                data.BarDefs.Dump(writer);
                data.Timeline.Dump(writer);
                writer.Write(data.Flows.Count);
                foreach (var flow in data.Flows.AsSpan())
                {
                    flow.Dump(writer);
                }
            }

            public void LoadMain(BinaryReader reader, IBmsData root)
            {
                data.Note = reader.ReadStringOrNull();
                data.MainHeaders.ProcessLoad(reader);
                data.SubHeaders.ProcessLoad(reader);
                data.DefLists.ProcessLoad(reader);
                data.BarDefs.ProcessLoad(reader);
                data.Timeline.ProcessLoad(reader);
                var flows = data.Flows;
                var count = reader.ReadInt32();
                for (var i = 0; i < count; i++)
                {
                    if (i < flows.Count)
                    {
                        flows[i].ProcessLoad(reader);
                    }
                    else
                    {
                        var flow = new FlowContainer();
                        flow.ProcessLoad(reader);
                        flows.Add(flow);
                    }
                }
                if (flows.Count > count)
                {
                    flows.RemoveRange(count, flows.Count - count);
                }
            }
        }
    }
}
