using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Converters;
using LivreNoirLibrary.YuGiOh.Media;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    using LivreNoirLibrary.YuGiOh;

    public static class Icons
    {
        public static DrawingGroup? Unusable { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetLimitIcon(LimitCount.Unusable));
        public static DrawingGroup? Forbidden { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetLimitIcon(LimitCount.Forbidden));
        public static DrawingGroup? Limit1 { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetLimitIcon(LimitCount.Limit1));
        public static DrawingGroup? Limit2 { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetLimitIcon(LimitCount.Limit2));
        public static DrawingGroup? Unlimited { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetLimitIcon(LimitCount.Unlimited));
        public static DrawingGroup? Specified { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetLimitIcon(LimitCount.Specified));

        public static DrawingGroup NormalMonster { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetCardIcon(CardIconType.Normal));
        public static DrawingGroup EffectMonster { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetCardIcon(CardIconType.Effect));
        public static DrawingGroup FusionMonster { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetCardIcon(CardIconType.Fusion));
        public static DrawingGroup RitualMonster { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetCardIcon(CardIconType.Ritual));
        public static DrawingGroup SynchroMonster { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetCardIcon(CardIconType.Synchro));
        public static DrawingGroup XyzMonster { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetCardIcon(CardIconType.Xyz));
        public static DrawingGroup LinkMonster { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetCardIcon(CardIconType.Link));

        public static DrawingGroup NormalPendulum { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetCardIcon(CardIconType.Pendulum));
        public static DrawingGroup EffectPendulum { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetCardIcon(CardIconType.Effect_Pendulum));
        public static DrawingGroup FusionPendulum { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetCardIcon(CardIconType.Fusion_Pendulum));
        public static DrawingGroup RitualPendulum { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetCardIcon(CardIconType.Ritual_Pendulum));
        public static DrawingGroup SynchroPendulum { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetCardIcon(CardIconType.Synchro_Pendulum));
        public static DrawingGroup XyzPendulum { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetCardIcon(CardIconType.Xyz_Pendulum));

        public static DrawingGroup NormalSpell { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetCardIcon(CardIconType.Spell));
        public static DrawingGroup FieldSpell { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetCardIcon(CardIconType.Field_Spell));
        public static DrawingGroup EquipSpell { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetCardIcon(CardIconType.Equip_Spell));
        public static DrawingGroup ContinuousSpell { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetCardIcon(CardIconType.Continuous_Spell));
        public static DrawingGroup QuickSpell { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetCardIcon(CardIconType.Quick_Spell));
        public static DrawingGroup RitualSpell { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetCardIcon(CardIconType.Ritual_Spell));
        public static DrawingGroup NormalTrap { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetCardIcon(CardIconType.Trap));
        public static DrawingGroup ContinuousTrap { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetCardIcon(CardIconType.Continuous_Trap));
        public static DrawingGroup CounterTrap { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetCardIcon(CardIconType.Counter_Trap));

        public static DrawingGroup Light { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetAttributeIcon(Attribute.Light));
        public static DrawingGroup Dark { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetAttributeIcon(Attribute.Dark));
        public static DrawingGroup Water { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetAttributeIcon(Attribute.Water));
        public static DrawingGroup Fire { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetAttributeIcon(Attribute.Fire));
        public static DrawingGroup Earth { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetAttributeIcon(Attribute.Earth));
        public static DrawingGroup Wind { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetAttributeIcon(Attribute.Wind));
        public static DrawingGroup Divine { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.GetAttributeIcon(Attribute.Divine));

        public static DrawingGroup Tuner { get; } = LnIconConverter.Convert(LivreNoirLibrary.YuGiOh.Media.Icons.TunerIcon);
    }
}
