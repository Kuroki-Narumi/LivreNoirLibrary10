using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using System;
using System.Drawing;

namespace LivreNoirLibrary.Windows.Controls.Bms.Elements
{
    public class SkinRoot(Skin skin) : ISkinRoot
    {
        public static SkinRoot Default { get; } = new(new());

        public Skin Skin { get; } = skin;
        public Size BaseSize { get; } = skin.BaseSize;
        public double FadeInTime { get; private set; }
        public double FadeOutTime { get; private set; }

        public virtual void DetermineExpressions(IVariableProvider? provider)
        {
            var skin = Skin;
            FadeInTime = skin.ResolveValue(skin.FadeInTime, provider, 0d);
            FadeOutTime = skin.ResolveValue(skin.FadeOutTime, provider, 0d);
        }

        public static SkinRoot Create(Skin skin) => skin switch
        {
            PlaySkin p => new PlaySkinRoot(p),
            _ => new SkinRoot(skin),
        };
    }
}
