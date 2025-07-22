using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Media.Midi;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.BM3
{
    public partial class PackOptions : ObservableObjectBase
    {
        public static readonly Rational DefaultMultiDefInterval = new(-1);

        [ObservableProperty]
        private bool _isRhythmTrack;
        [ObservableProperty]
        private Rational _rhythmMaxLength;
        [ObservableProperty]
        private double _msV = 1;
        [ObservableProperty]
        private bool _ignoreTempo = false;
        [ObservableProperty]
        private bool _portamento = false;
        [ObservableProperty]
        private bool _selectCC = false;
        [ObservableProperty]
        private HashSet<CCType> _targetCCs = [];
        [ObservableProperty]
        private Rational _afterMargin;

        [ObservableProperty]
        private string _suffix = SliceUtils.Suffix_Index1;
        [ObservableProperty]
        private bool _sort;
        [ObservableProperty]
        private SortKeyType _sortKey1 = SortKeyType.NN;
        [ObservableProperty]
        private SortKeyType _sortKey2 = SortKeyType.Vel;
        [ObservableProperty]
        private SortKeyType _sortKey3 = SortKeyType.Gate;

        [ObservableProperty]
        private string _exportFilename = PackUtils.DefaultFormat_Pack;
        [ObservableProperty]
        private int _headroom = 1;
        [ObservableProperty]
        private int _interval = 6;
        [ObservableProperty]
        private bool _cutTail = false;
        [ObservableProperty]
        private Rational _tailMargin;

        public static double CoerceMsV(double value) => Math.Clamp(value, 1, 1000);

        [JsonIgnore]
        public string SuffixWithDefault => string.IsNullOrEmpty(_suffix) ? SliceUtils.Suffix_Index1 : _suffix;

        [JsonIgnore]
        public string ExportFilenameWithDefault => string.IsNullOrEmpty(_exportFilename) ? PackUtils.DefaultFormat_Pack : _exportFilename;

        public void Load(PackOptions source)
        {
            IsRhythmTrack = source._isRhythmTrack;
            RhythmMaxLength = source._rhythmMaxLength;
            MsV = source._msV;
            IgnoreTempo = source._ignoreTempo;
            Portamento = source._portamento;
            SelectCC = source._selectCC;
            TargetCCs = [.. source._targetCCs];
            AfterMargin = source._afterMargin;
            Suffix = source._suffix;
            Sort = source._sort;
            SortKey1 = source._sortKey1;
            SortKey2 = source._sortKey2;
            SortKey3 = source._sortKey3;
            ExportFilename = source._exportFilename;
            Headroom = source._headroom;
            Interval = source._interval;
            CutTail = source._cutTail;
            TailMargin = source._tailMargin;
        }
    }
}
