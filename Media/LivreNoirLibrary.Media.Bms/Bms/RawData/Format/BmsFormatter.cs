using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Bms
{
    using ConductorDefCollection = Dictionary<DefType, OrderedDictionary<double, int>>;

    public partial class BmsFormatter(IBmsData data)
    {
        private readonly IBmsData _data = data;
        private readonly ConductorDefCollection _conductorDefs = [];
        private readonly Dictionary<IBmsDataUnit, SaveState> _states = [];
        private Encoding? _encoding = null;
        private int _radix;

        public long Prepare(IBmsDataUnit? root = null)
        {
            var data = _data;
            var conductorDefs = _conductorDefs;
            var states = _states;
            var encoding = BmsConstants.DefaultEncoding;
            try
            {
                TryEncode(data, encoding);
            }
            catch
            {
                encoding = BmsConstants.Utf8Encoding;
            }
            _encoding = encoding;

            var lnObj = data.LnObj;
            var radix = BmsConstants.Base_Legacy;
            var maxDen = 0L;
            foreach (var unit in _data.EnumerateChildren(root))
            {
                SaveState state = new(unit, lnObj, conductorDefs, ref radix);
                states.Add(unit, state);
                maxDen = Math.Max(maxDen, state.MaxDenominator);
            }
            _radix = radix;
            return maxDen;
        }

        public void ReductDenominator(long limit)
        {
            foreach (var (_, state) in _states)
            {
                state.ReductDenominator(limit);
            }
        }

        private static void TryEncode(IBmsData data, Encoding encoding)
        {
            foreach (var unit in data.EnumerateAllData())
            {
                CheckNote(unit);
                unit.MainHeaders.TryEncode(encoding);
                unit.SubHeaders.TryEncode(encoding);
                unit.DefLists.TryEncode(encoding);
                foreach (var flow in unit.Flows.AsSpan())
                {
                    CheckNote(flow);
                    foreach (var branch in flow.EnumerateBranches())
                    {
                        CheckNote(branch);
                    }
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            void CheckNote(INoteObject obj)
            {
                if (obj.Note is { } note)
                {
                    encoding.GetByteCount(note);
                }
            }
        }

        public void Format(Stream stream, IBmsDataUnit? root = null, bool indent = false)
        {
            ArgumentNullException.ThrowIfNull(_encoding);
            using BmsTextWriter writer = new(stream, indent, _encoding);
            root ??= _data.Root;
            DumpHeader(root, writer, true);
            DumpMain(root, writer, true);
            DumpFlow(root, writer, true);
        }

        private void DumpHeader(IBmsDataUnit data, BmsTextWriter writer, bool isRoot)
        {
            writer.WriteLine(data.Note);
            var radix = _radix;
            var lnObj = _data.LnObj;
            var main = data.MainHeaders;
            var sub = data.SubHeaders;
            var writeBase = isRoot && radix is > BmsConstants.Base_Default;
            var writeLnObj = isRoot && lnObj is not 0;
            if (main.Count + sub.Count is > 0 || writeBase || writeLnObj)
            {
                if (isRoot)
                {
                    writer.WriteLine(FieldSeparators.Header);
                    writer.WriteLine();
                }
                main.Dump(writer);
                if (writeLnObj)
                {
                    writer.WriteLine($"#LNOBJ {BmsUtils.ToBased(lnObj, radix)}");
                }
                sub.Dump(writer);
                if (writeBase)
                {
                    writer.WriteLine($"#BASE {radix}");
                }
                if (isRoot)
                {
                    writer.WriteLine();
                }
            }
            if (data.DefLists.HasValue || (isRoot && _conductorDefs.Count is > 0))
            {
                if (isRoot)
                {
                    writer.WriteLine(FieldSeparators.Def);
                    writer.WriteLine();
                }
                data.DefLists.Dump(writer, radix, isRoot);
                if (isRoot)
                {
                    foreach (var (type, dic) in _conductorDefs)
                    {
                        var key = type.ToString().ToUpper();
                        foreach (var (value, index) in dic)
                        {
                            writer.WriteLine($"#{key}{BmsUtils.ToBased(index, radix)} {value:0.############}");
                        }
                    }
                    writer.WriteLine();
                }
            }
        }

        private void DumpMain(IBmsDataUnit data, BmsTextWriter writer, bool isRoot)
        {
            if (isRoot)
            {
                writer.WriteLine(FieldSeparators.Data);
                writer.WriteLine();
            }
            var state = _states[data];
            var radix = _radix;
            foreach (var (number, bar) in state._bars)
            {
                var numberHead = BmsUtils.GetBarText(number);
                var head = $"{numberHead}{BmsUtils.ToBased(Channel.Bgm)}:";
                foreach (var line in bar._bgm)
                {
                    writer.Write(head);
                    line.WriteText(writer, radix);
                }
                if (bar.Length is not 1)
                {
                    writer.WriteLine($"{numberHead}{BmsUtils.ToBased(Channel.Bar)}:{bar.Length:0.############}");
                }
                foreach (var (ch, lines) in bar._channels)
                {
                    head = $"{numberHead}{BmsUtils.ToBased(ch)}:";
                    var r = BmsUtils.IsHexValue(ch) ? 16 : radix;
                    foreach (var line in lines.AsSpan())
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

        private void DumpFlow(IBmsDataUnit data, BmsTextWriter writer, bool isRoot)
        {
            if (data.ContainsFlow)
            {
                if (isRoot)
                {
                    writer.WriteLine(FieldSeparators.Flows);
                    writer.WriteLine();
                }
                foreach (var flow in data.Flows.AsSpan())
                {
                    writer.WriteLine(flow.Note);
                    writer.WriteLine(flow.BmsHeader);
                    writer.IndentRight();
                    foreach (var branch in flow.EnumerateBranches())
                    {
                        var branchData = _data.GetBranchData(branch);
                        writer.WriteLine(branch.Note);
                        writer.WriteLine(flow.GetBranchHeader(branch.Condition));
                        writer.IndentRight();
                        DumpHeader(branchData, writer, false);
                        DumpMain(branchData, writer, false);
                        DumpFlow(branchData, writer, false);
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
