using System;

namespace LivreNoirLibrary.Media.Bms.Play
{
    public interface IBgaVisibilityProvider
    {
        BgaVisibility BgaVisibility { get; set; }
        bool ShowBgaBase { get => this.GetShowFlag(BgaVisibility.Base); set => this.SetShowFlag(BgaVisibility.Base, value); }
        bool ShowBgaLayer { get => this.GetShowFlag(BgaVisibility.Layer1); set => this.SetShowFlag(BgaVisibility.Layer1, value); }
        bool ShowBgaLayer2 { get => this.GetShowFlag(BgaVisibility.Layer2); set => this.SetShowFlag(BgaVisibility.Layer2, value); }
        bool ShowBgaMissLayer { get => this.GetShowFlag(BgaVisibility.Miss); set => this.SetShowFlag(BgaVisibility.Miss, value); }
        bool HideBgaOnMiss { get => this.GetShowFlag(BgaVisibility.HideOnMiss); set => this.SetShowFlag(BgaVisibility.HideOnMiss, value); }
        double MissLayerDisplayTime { get; set; }
    }
}
