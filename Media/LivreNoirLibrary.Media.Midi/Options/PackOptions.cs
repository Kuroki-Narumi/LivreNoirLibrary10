using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Midi
{
    public partial class PackOptions : ObservableObjectBase, IOptions<PackOptions>
    {
        public static readonly Rational DefaultRythmLength = new(1, 48);
        public static readonly Rational DefaultPortamentoLength = new(1, 16);

        public bool IsRhythmTrack { get; set => SetValue(ref field, value); }
        public Rational RhythmLength { get; set => SetValue(ref field, value); } = DefaultRythmLength;
        public Rational LengthQuantize { get; set => SetValue(ref field, value); }
        public int VelQuantize { get; set => SetValue(ref field, value); }
        public double MsV { get; set => SetValue(ref field, Math.Clamp(value, 1, 1000)); } = 1;
        public bool IgnoreTempo { get; set => SetValue(ref field, value); }
        public bool Portamento { get; set => SetValue(ref field, value); }
        public Rational PortamentoLength { get; set => SetValue(ref field, value); } = DefaultPortamentoLength;
        public bool SelectCC { get; set => SetValue(ref field, value); }
        public HashSet<CCType> TargetCCs
        {
            get;
            set
            {
                field.Clear();
                field.UnionWith(value);
                this.NotifyPropertyChanged();
            }
        } = [];
        public Rational AfterMargin { get; set => SetValue(ref field, value); }

        public string Suffix { get; set => SetValue(ref field, value); } = SliceUtils.Suffix_Index1;
        public bool Sort { get; set => SetValue(ref field, value); } = true;
        public SortKeyType SortKey1 { get; set => SetValue(ref field, value); } = SortKeyType.NN;
        public SortKeyType SortKey2 { get; set => SetValue(ref field, value); } = SortKeyType.Vel;
        public SortKeyType SortKey3 { get; set => SetValue(ref field, value); } = SortKeyType.Gate;
        public bool AlignToRight { get; set => SetValue(ref field, value); }

        public string ExportFilename { get; set => SetValue(ref field, value); } = Media.PackUtils.DefaultFormat_Pack;
        public int Headroom { get; set => SetValue(ref field, value); } = 1;
        public int Interval { get; set => SetValue(ref field, value); } = 6;
        public bool CutTail { get; set => SetValue(ref field, value); }
        public Rational TailMargin { get; set => SetValue(ref field, value); }

        [JsonIgnore]
        public string SuffixWithDefault => string.IsNullOrEmpty(Suffix) ? SliceUtils.Suffix_Index1 : Suffix;
        [JsonIgnore]
        public string ExportFilenameWithDefault => string.IsNullOrEmpty(ExportFilename) ? Media.PackUtils.DefaultFormat_Pack : ExportFilename;

        public void Load(PackOptions source)
        {
            IsRhythmTrack = source.IsRhythmTrack;
            RhythmLength = source.RhythmLength;
            LengthQuantize = source.LengthQuantize;
            VelQuantize = source.VelQuantize;
            MsV = source.MsV;
            IgnoreTempo = source.IgnoreTempo;
            Portamento = source.Portamento;
            PortamentoLength = source.PortamentoLength;
            SelectCC = source.SelectCC;
            TargetCCs = source.TargetCCs;
            AfterMargin = source.AfterMargin;
            Suffix = source.Suffix;
            Sort = source.Sort;
            SortKey1 = source.SortKey1;
            SortKey2 = source.SortKey2;
            SortKey3 = source.SortKey3;
            AlignToRight = source.AlignToRight;
            ExportFilename = source.ExportFilename;
            Headroom = source.Headroom;
            Interval = source.Interval;
            CutTail = source.CutTail;
            TailMargin = source.TailMargin;
        }
    }
}
