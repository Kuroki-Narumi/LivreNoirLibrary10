using LivreNoirLibrary.Files;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public sealed class BmsData : BaseData, IRootData, IStreamLoadable<BmsData>
    {
        public ChartType ChartType { get; set; } = ChartType.Beat;
        public BarLengthCache BarLengthCache { get; } = new();

        public static BmsData Create()
        {
            BmsData data = new();
            data.Headers.SetDefault();
            return data;
        }

        internal override void ClearBarLengthCache(int number) => BarLengthCache.Clear(number);
        internal override Rational GetHead(int number, IBarPositionProvider provider) => BarLengthCache.GetHead(number, provider);
        internal override Rational GetAbsolutePosition(BarPosition position, IBarPositionProvider provider) => BarLengthCache.GetAbsolutePosition(position, provider);
        internal override BarPosition GetBarPosition(Rational absolutePosition, IBarPositionProvider provider) => BarLengthCache.GetBarPosition(absolutePosition, provider);
        internal override IEnumerable<BarInfo> EnumerateBars(int first, int last, IBarPositionProvider provider) => BarLengthCache.EnumerateBars(first, last, provider);

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
                data.ChartType = ChartType.Generic;
            }
            else
            {
                data.ChartType = ChartType.Beat;
            }
            return data;
        }

        public string GetExtension() => ChartType switch
        {
            ChartType.Popn => Filters.Pms_Save,
            ChartType.Generic => Filters.Bmg_Save,
            _ => Filters.Bms_Save,
        };

        public static BmsData Load(Stream stream)
        {
            BmsParser parser = new();
            return parser.Parse(stream);
        }

        public void Save(string path, bool indent = false, bool ext = true) => General.Save(path, s => Dump(s, indent), ext ? ExtRegs.BeMusic : null, Exts.Bms);

        public void Dump(Stream stream, bool indent)
        {
            BmsFormatter formatter = new(this);
            formatter.Format(stream, this, indent);
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
            }
            stream.Position = 0;
        }

        public void LoadHistoryBuffer(Stream stream)
        {
            BarLengthCache.Clear();
            stream.Position = 0;
            using BinaryReader reader = new(stream, Encoding.UTF8, true);
            LoadMain(reader);
        }
    }
}
