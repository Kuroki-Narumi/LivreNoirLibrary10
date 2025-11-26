using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class SequentialSetOptions : ObservableObjectBase
    {
        public const double DefaultInterval = 1d / 16d;
        public const int DefaultResolution = 192;

        public BarPosition StartPosition { get; set => SetValue(ref field, value); }
        public Channel Lane { get;set=> SetValue(ref field, value); }
        public List<int> IdList { get; set => SetValue(ref field, value); } = [];
        public bool AutoInterval { get; set => SetValue(ref field, value); }
        public double Interval { get; set => SetValue(ref field, value); } = DefaultInterval;
        public int Resolution { get; set => SetValue(ref field, value); } = DefaultResolution;
        [JsonIgnore]
        public string RootDirectory { get; set => SetValue(ref field, value); } = "";
    }
}
