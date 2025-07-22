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

        [ObservableProperty]
        private BarPosition _position;
        [ObservableProperty]
        private int _lane;
        [ObservableProperty]
        private List<int> _idList = [];
        [ObservableProperty]
        private bool _intervalAuto;
        [ObservableProperty]
        private Rational _interval = DefaultInterval;
        [ObservableProperty]
        private Rational _resolution = DefaultResolution;
        [JsonIgnore]
        [ObservableProperty]
        private string _rootDirectory = "";
    }
}
