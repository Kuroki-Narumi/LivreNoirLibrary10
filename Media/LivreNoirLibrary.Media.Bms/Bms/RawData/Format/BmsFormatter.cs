using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace LivreNoirLibrary.Media.Bms
{
    using ConductorDefCollection = Dictionary<DefType, OrderedDictionary<decimal, int>>;

    public partial class BmsFormatter
    {
        private readonly ConductorDefCollection _conductor_defs = [];
        private readonly Dictionary<BaseData, SaveState> _states = [];
        private readonly Encoding _encoding;
        private readonly int _radix;

        public long MaxDenominator { get; private set; }

        public BmsFormatter(BmsData data)
        {
            var conductorDefs = _conductor_defs;
            var states = _states;
            conductorDefs.Clear();
            states.Clear();

            var encoding = Constants.DefaultEncoding;
            try
            {
                TryEncode(data, encoding);
            }
            catch
            {
                encoding = Constants.Utf8Encoding;
            }
            _encoding = encoding;

            var lnObj = data.LnObj;
            var radix = Constants.Base_Legacy;
            var maxDen = 0L;
            foreach (var d in data.EachData())
            {
                SaveState state = new(d, lnObj, conductorDefs, ref radix);
                states.Add(d, state);
                maxDen = Math.Max(maxDen, state.MaxDenominator);
            }
            _radix = radix;
        }

        public void ReductDenominator(long limit)
        {
            foreach (var (_, state) in _states)
            {
                state.ReductDenominator(limit);
            }
            MaxDenominator = limit;
        }

        public void Format(Stream stream, BmsData data, bool indent)
        {
            using BmsTextWriter writer = new(stream, indent, _radix, _encoding);
            DumpHeader(data, writer, true);
            DumpMain(data, writer, true);
            DumpFlow(data, writer, true);
        }

        private static void TryEncode(BaseData data, Encoding encoding)
        {
            if (data.Note is { } str)
            {
                encoding.GetByteCount(str);
            }
            data.Headers.TryEncode(encoding);
            data.DefLists.TryEncode(encoding);
            foreach (var flow in CollectionsMarshal.AsSpan(data.Flows))
            {
                if (flow.Note is { } s)
                {
                    encoding.GetByteCount(s);
                }
                foreach (var branch in CollectionsMarshal.AsSpan(flow.Branches))
                {
                    TryEncode(branch, encoding);
                }
            }
        }

        private void DumpHeader(BaseData data, BmsTextWriter writer, bool isRoot)
        {
            writer.WriteLine(data.Note);
            if (data.Headers.HasValue || (isRoot && writer.Radix is > Constants.Base_Default))
            {
                if (isRoot)
                {
                    writer.WriteLine(FieldSeparators.Header);
                    writer.WriteLine();
                }
                data.Headers.Dump(writer, isRoot);
            }
            if (data.DefLists.HasValue || (isRoot && _conductor_defs.Count is > 0))
            {
                if (isRoot)
                {
                    writer.WriteLine(FieldSeparators.Def);
                    writer.WriteLine();
                }
                data.DefLists.Dump(writer, isRoot);
                if (isRoot)
                {
                    var radix = writer.Radix;
                    foreach (var (type, dic) in _conductor_defs)
                    {
                        var key = type.ToString().ToUpper();
                        foreach (var (value, index) in dic)
                        {
                            writer.WriteLine($"#{key}{BmsUtils.ToBased(index, radix)} {value}");
                        }
                    }
                    writer.WriteLine();
                }
            }
        }

        private void DumpMain(BaseData data, BmsTextWriter writer, bool isRoot)
        {
            if (isRoot)
            {
                writer.WriteLine(FieldSeparators.Data);
                writer.WriteLine();
            }
            var state = _states[data];
            var radix = writer.Radix;
            foreach (var (number, bar) in state._bars)
            {
                var numberHead = BmsUtils.GetBarText(number);
                var head = $"{numberHead}{BmsUtils.ToBased(Channel.Bgm)}:";
                foreach (var line in bar._bgm)
                {
                    writer.Write(head);
                    line.WriteText(writer, radix);
                }
                if (data.Bars.TryGetValue(number, out var length) && length != Rational.One)
                {
                    writer.WriteLine($"{numberHead}{BmsUtils.ToBased(Channel.Bar)}:{(decimal)length}");
                }
                foreach (var (ch, lines) in bar._channels)
                {
                    head = $"{numberHead}{BmsUtils.ToBased(ch)}:";
                    var r = BmsUtils.IsHex(ch) ? 16 : radix;
                    foreach (var line in CollectionsMarshal.AsSpan(lines))
                    {
                        writer.Write(head);
                        line.WriteText(writer, r);
                    }
                }
                writer.WriteLine();
            }
            if (isRoot)
            {
                writer.WriteLine();
            }
        }

        private void DumpFlow(BaseData data, BmsTextWriter writer, bool isRoot)
        {
            if (data.Flows.Count is > 0)
            {
                if (isRoot)
                {
                    writer.WriteLine(FieldSeparators.Flows);
                    writer.WriteLine();
                }
                foreach (var flow in CollectionsMarshal.AsSpan(data.Flows))
                {
                    writer.WriteLine(flow.Note);
                    writer.WriteLine(flow.BmsHeader);
                    writer.IndentRight();
                    foreach (var branch in CollectionsMarshal.AsSpan(flow.Branches))
                    {
                        writer.WriteLine(branch.Note);
                        writer.WriteLine(flow.GetBranchHeader(branch));
                        writer.IndentRight();
                        DumpHeader(branch, writer, false);
                        DumpMain(branch, writer, false);
                        DumpFlow(branch, writer, false);
                        writer.IndentLeft();
                        writer.WriteLine(flow.GetBranchFooter());
                    }
                    writer.IndentLeft();
                    writer.WriteLine(flow.BmsFooter);
                }
                if (isRoot)
                {
                    writer.WriteLine();
                }
            }
        }
    }
}
