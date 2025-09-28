using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Bms
{
    public sealed class BgaSetting : RectElementSetting
    {
        public bool RewindMissLayer { get; set => SetValue(ref field, value); } = true;
        public double MissLayerDisplayTime { get; set => SetValue(ref field, value); } = 0.5;
    }
}
