using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class MultiDefOptions : IndexesOptionsBase
    {
        public int MinimumInterval { get; set => SetValue(ref field, value); }
        public double Threshold { get; set => SetValue(ref field, value); } = -24;
        public int MaxCount { get; set => SetValue(ref field, Math.Clamp(value, 1, 16)); } = 16;
        public bool InsertDefIndex { get; set => SetValue(ref field, value); } = true;
        public int DefStart { get; set => SetValue(ref field, value); } = 1;
    }

    public class MultiDefInfo(int maxIndex, Dictionary<int, string> additionalDefs, List<(Note, int)> replace, DefIndexMap? map)
    {
        private readonly List<(Note, int)> _replace = replace;

        public int MaxIndex { get; } = maxIndex;
        public Dictionary<int, string> AdditionalDefs { get; } = additionalDefs;
        public ReadOnlySpan<(Note, int)> ReplaceList => _replace.AsSpan();
        public DefIndexMap? DefMap { get; } = map;
    }
}
