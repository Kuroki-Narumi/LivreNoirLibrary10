using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Files;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media.Bms.RawData;

namespace LivreNoirLibrary.Media.Bms
{
    partial class BmsData
    {
        internal BmsData() { }

        public static BmsData Create()
        {
            BmsData data = new();
            data.SetDefaultHeaders();
            return data;
        }

        public static BmsData Open(string path)
        {
            var data = General.Open(path, s =>
            {
                BmsTextReader reader = new(s);
                return reader.Parse();
            });
            if (ExtRegs.Pms.IsMatch(Path.GetExtension(path)))
            {
                data.ChartType = ChartType.Popn;
            }
            else
            {
                data.CheckIfGeneric();
            }
            return data;
        }

        public static BmsData Load(Stream stream)
        {
            BmsTextReader reader = new(stream);
            return reader.Parse();
        }

        public void Save(string path, bool ext = true, bool indent = false)
        {
            General.Save(path, stream =>
            {
                try
                {
                    using var writer = BmsTextWriter.Create(stream, indent, Constants.DefaultEncoding);
                    Dump(writer);
                }
                catch (EncoderFallbackException)
                {
                    using var writer = BmsTextWriter.Create(stream, indent, Constants.Utf8Encoding);
                    Dump(writer);
                }
            }, ext ? ExtRegs.BeMusic : null, ChartType is ChartType.Popn ? Exts.Pms : Exts.Bms);
        }

        protected override void ExtractRawDataCore()
        {
            base.ExtractRawDataCore();
            foreach (var data in CollectionsMarshal.AsSpan(_flow_data))
            {
                data.ExtractRawData(this);
            }
        }

        internal override void DumpCore(BmsTextWriter writer)
        {
            void DumpSeparator(string s)
            {
                writer.Dump(s);
                writer.DumpEmpty();
            }
            CreateRawData();
            if (!Headers.IsEmpty())
            {
                DumpSeparator(SeparatorComments.Header);
                Headers.Dump(writer);
                writer.DumpEmpty();
            }
            if (Comments.Count > 0)
            {
                DumpSeparator(SeparatorComments.Others);
                foreach (var comment in CollectionsMarshal.AsSpan(Comments))
                {
                    writer.Dump(comment);
                }
                writer.DumpEmpty();
            }
            if (!DefLists.IsEmpty())
            {
                DumpSeparator(SeparatorComments.Def);
                DefLists.Dump(writer, Base, true);
            }
            if (RawData.Count > 0)
            {
                DumpSeparator(SeparatorComments.Data);
                RawData.Dump(writer, Base, true);
            }
            if (_flows.Count is > 0)
            {
                DumpSeparator(SeparatorComments.Flows);
                foreach (var flow in CollectionsMarshal.AsSpan(_flows))
                {
                    flow.Dump(writer, this);
                }
            }
        }

        internal void DumpFlowData(int id, BmsTextWriter writer)
        {
            if (TryGetFlowData(id, out var data))
            {
                data.Dump(writer, this);
            }
        }

        public BmsData Clone()
        {
            using MemoryStream ms = new();
            WriteHistoryBuffer(ms);
            BmsData data = new() { ChartType = ChartType };
            data.LoadHistoryBuffer(ms);
            return data;
        }

        public void WriteHistoryBuffer(Stream stream)
        {
            using (BinaryWriter writer = new(stream, Encoding.UTF8, true))
            {
                stream.SetLength(0);
                DumpMain(writer);
                writer.Write(_flows.Count);
                foreach (var item in CollectionsMarshal.AsSpan(_flows))
                {
                    item.Dump(writer);
                }
                writer.Write(_flow_data.Count);
                foreach (var data in CollectionsMarshal.AsSpan(_flow_data))
                {
                    data.DumpMain(writer);
                }
                writer.Write(_flow_free_index.Count);
                foreach (var id in _flow_free_index)
                {
                    writer.Write(id);
                }
            }
            stream.Position = 0;
        }

        public void LoadHistoryBuffer(Stream stream)
        {
            ClearBarCache();
            stream.Position = 0;
            using BinaryReader reader = new(stream, Encoding.UTF8, true);
            LoadMain(reader);

            var count = reader.ReadInt32();
            var flows = _flows;
            flows.Clear();
            for (var i = 0; i < count; i++)
            {
                flows.Add(FlowContainer.AutoLoad(reader));
            }

            count = reader.ReadInt32();
            var fData = _flow_data;
            var max = fData.Count;
            for (var i = 0; i < count; i++)
            {
                FlowData data;
                if (i >= max)
                {
                    data = new(this);
                    fData.Add(data);
                }
                else
                {
                    data = fData[i];
                    data.Clear();
                }
                data.LoadMain(reader);
            }
            for (var i = max - 1; i >= count; i--)
            {
                fData.RemoveAt(i);
            }

            count = reader.ReadInt32();
            var set = _flow_free_index;
            set.Clear();
            for (var i = 0; i < count; i++)
            {
                set.Add(reader.ReadInt32());
            }
        }
    }
}
