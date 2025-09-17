using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class SequentialSetOptions : ObservableObjectBase
    {
        public static readonly Rational DefaultInterval = new(1, 16);
        public static readonly Rational DefaultResolution = new(1, 192);

        public BarPosition StartPosition { get; set => SetValue(ref field, value); }
        public int Lane { get;set=> SetValue(ref field, value); }
        public List<int> IdList { get; set => SetValue(ref field, value); } = [];
        public bool AutoInterval { get; set => SetValue(ref field, value); }
        public Rational Interval { get; set => SetValue(ref field, value); } = DefaultInterval;
        public Rational Resolution { get; set => SetValue(ref field, value); } = DefaultResolution;
        [JsonIgnore]
        public string RootDirectory { get; set => SetValue(ref field, value); } = "";
    }
}
