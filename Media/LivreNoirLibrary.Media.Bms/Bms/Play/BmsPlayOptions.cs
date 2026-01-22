using LivreNoirLibrary.ObjectModel;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media.Bms.Play
{
    public class BmsPlayOptions : ObservableObjectBase, IHighSpeedProvider, IBgaVisibilityProvider, IJudgeProvider
    {
        public float MasterVolume { get; set => SetValue(ref field, value); } = 0.7f;
        public float KeyVolume { get; set => SetValue(ref field, value); } = 1;
        public float BgmVolume { get; set => SetValue(ref field, value); } = 1;

        public double HighSpeed { get; set => SetValue(ref field, value); } = 2.5;
        [JsonConverter(typeof(JsonStringEnumConverter<HsCorrectionMode>))]
        public HsCorrectionMode HsCorrectionMode { get; set => SetValue(ref field, value); } = HsCorrectionMode.MainTimeBpm;
        public double HighSpeedReference { get; set => SetValue(ref field, value); } = 120;
        public double HighSpeedCorrection { get; set => SetValue(ref field, value); } = 1;

        [JsonConverter(typeof(JsonStringEnumConverter<BgaVisibility>))]
        public BgaVisibility BgaVisibility
        {
            get;
            set => SetValue(ref field, value, [nameof(ShowBgaBase), nameof(ShowBgaLayer), nameof(ShowBgaLayer2), nameof(ShowBgaMissLayer), nameof(HideBgaOnMiss)]);
        } = BgaVisibility.Default;

        [JsonIgnore]
        public bool ShowBgaBase { get => this.GetShowFlag(BgaVisibility.Base); set => this.SetShowFlag(BgaVisibility.Base, value); }
        [JsonIgnore]
        public bool ShowBgaLayer { get => this.GetShowFlag(BgaVisibility.Layer1); set => this.SetShowFlag(BgaVisibility.Layer1, value); }
        [JsonIgnore]
        public bool ShowBgaLayer2 { get => this.GetShowFlag(BgaVisibility.Layer2); set => this.SetShowFlag(BgaVisibility.Layer2, value); }
        [JsonIgnore]
        public bool ShowBgaMissLayer { get => this.GetShowFlag(BgaVisibility.Miss); set => this.SetShowFlag(BgaVisibility.Miss, value); }
        [JsonIgnore]
        public bool HideBgaOnMiss { get => this.GetShowFlag(BgaVisibility.HideOnMiss); set => this.SetShowFlag(BgaVisibility.HideOnMiss, value); }

        public double MissLayerDisplayTime { get; set => SetValue(ref field, value); } = 0.5;
        public double JudgeDisplayTime { get; set => SetValue(ref field, value); } = 1;

        [JsonIgnore]
        public JudgeDefinitionCollection Judges { get; set => SetValue(ref field, value); } = JudgeDefinitions.Beat_Easy;
        [JsonIgnore]
        public ScoreDefinition ScoreDefinition { get; set => SetValue(ref field, value); } = ScoreDefinitions.Beat_Default;
        [JsonIgnore]
        public GaugeDefinition GaugeDefinition { get; set => SetValue(ref field, value); } = GaugeDefinitions.Beat_Normal;
        [JsonIgnore]
        public double GaugeGainBase { get; set => SetValue(ref field, value); }
    }
}
