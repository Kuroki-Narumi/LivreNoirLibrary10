using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System;

namespace LivreNoirLibrary.Windows.Controls.Bms.Elements
{
    public class PlaySkinRoot(PlaySkin skin) : SkinRoot(skin), IPlaySkinRoot
    {
        private readonly PlaySkin _skin = skin;

        public double LoadTime { get; private set; }
        public double ReadyTime { get; private set; }
        public double MarginTime { get; private set; }

        public override void DetermineExpressions(IVariableProvider? provider)
        {
            base.DetermineExpressions(provider);
            var skin = _skin;
            LoadTime = skin.ResolveValue(skin.LoadTime, provider, 0d);
            ReadyTime = skin.ResolveValue(skin.ReadyTime, provider, 0d);
            MarginTime = skin.ResolveValue(skin.MarginTime, provider, 0d);
        }
    }
}
