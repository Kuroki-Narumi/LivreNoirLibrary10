using System;
using System.Windows.Media;

namespace LivreNoirLibrary.YuGiOh.Controls
{
    public static partial class Icons
    {
        public static DrawingImage? LimitIcon_Unusable => GetLimitIcon(LimitNumber.Unusable);
        public static DrawingImage? LimitIcon_Forbidden => GetLimitIcon(LimitNumber.Forbidden);
        public static DrawingImage? LimitIcon_Limit1 => GetLimitIcon(LimitNumber.Limit1);
        public static DrawingImage? LimitIcon_Limit2 => GetLimitIcon(LimitNumber.Limit2);
        public static DrawingImage? LimitIcon_Specified => GetLimitIcon(LimitNumber.Specified);

        public static DrawingImage CardIcon_Normal => GetCardIcon(CardIconType.Normal);
        public static DrawingImage CardIcon_Token => GetCardIcon(CardIconType.Token);
        public static DrawingImage CardIcon_Effect => GetCardIcon(CardIconType.Effect);
        public static DrawingImage CardIcon_Fusion => GetCardIcon(CardIconType.Fusion);
        public static DrawingImage CardIcon_Ritual => GetCardIcon(CardIconType.Ritual);
        public static DrawingImage CardIcon_Synchro => GetCardIcon(CardIconType.Synchro);
        public static DrawingImage CardIcon_Xyz => GetCardIcon(CardIconType.Xyz);
        public static DrawingImage CardIcon_Link => GetCardIcon(CardIconType.Link);
        public static DrawingImage CardIcon_Pendulum => GetCardIcon(CardIconType.Pendulum);
        public static DrawingImage CardIcon_P_Effect => GetCardIcon(CardIconType.Effect_Pendulum);
        public static DrawingImage CardIcon_P_Fusion => GetCardIcon(CardIconType.Fusion_Pendulum);
        public static DrawingImage CardIcon_P_Ritual => GetCardIcon(CardIconType.Ritual_Pendulum);
        public static DrawingImage CardIcon_P_Synchro => GetCardIcon(CardIconType.Synchro_Pendulum);
        public static DrawingImage CardIcon_P_Xyz => GetCardIcon(CardIconType.Xyz_Pendulum);
        public static DrawingImage CardIcon_Spell => GetCardIcon(CardIconType.Spell);
        public static DrawingImage CardIcon_S_Field => GetCardIcon(CardIconType.Field_Spell);
        public static DrawingImage CardIcon_S_Equip => GetCardIcon(CardIconType.Equip_Spell);
        public static DrawingImage CardIcon_S_Continuous => GetCardIcon(CardIconType.Continuous_Spell);
        public static DrawingImage CardIcon_S_Quick => GetCardIcon(CardIconType.Quick_Spell);
        public static DrawingImage CardIcon_S_Ritual => GetCardIcon(CardIconType.Ritual_Spell);
        public static DrawingImage CardIcon_Trap => GetCardIcon(CardIconType.Trap);
        public static DrawingImage CardIcon_T_Continuous => GetCardIcon(CardIconType.Continuous_Trap);
        public static DrawingImage CardIcon_T_Counter => GetCardIcon(CardIconType.Counter_Trap);

        public static DrawingImage AttrIcon_Light => GetAttrIcon(Attribute.Light);
        public static DrawingImage AttrIcon_Dark => GetAttrIcon(Attribute.Dark);
        public static DrawingImage AttrIcon_Water => GetAttrIcon(Attribute.Water);
        public static DrawingImage AttrIcon_Fire => GetAttrIcon(Attribute.Fire);
        public static DrawingImage AttrIcon_Earth => GetAttrIcon(Attribute.Earth);
        public static DrawingImage AttrIcon_Wind => GetAttrIcon(Attribute.Wind);
        public static DrawingImage AttrIcon_Divine => GetAttrIcon(Attribute.Divine);
    }
}
