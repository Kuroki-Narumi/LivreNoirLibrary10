using System.Windows.Media;
using LivreNoirLibrary.YuGiOh;

namespace LivreNoirLibrary.Windows.YuGiOh
{
    public static partial class Icons
    {
        public static Drawing LimitIcon_Unusable => GetLimitIcon(LimitCount.Unusable);
        public static Drawing LimitIcon_Forbidden => GetLimitIcon(LimitCount.Forbidden);
        public static Drawing LimitIcon_Limit1 => GetLimitIcon(LimitCount.Limit1);
        public static Drawing LimitIcon_Limit2 => GetLimitIcon(LimitCount.Limit2);
        public static Drawing LimitIcon_Specified => GetLimitIcon(LimitCount.Specified);

        public static Drawing CardIcon_Normal => GetCardIcon(CardIconType.Normal);
        public static Drawing CardIcon_Token => GetCardIcon(CardIconType.Token);
        public static Drawing CardIcon_Effect => GetCardIcon(CardIconType.Effect);
        public static Drawing CardIcon_Fusion => GetCardIcon(CardIconType.Fusion);
        public static Drawing CardIcon_Ritual => GetCardIcon(CardIconType.Ritual);
        public static Drawing CardIcon_Synchro => GetCardIcon(CardIconType.Synchro);
        public static Drawing CardIcon_Xyz => GetCardIcon(CardIconType.Xyz);
        public static Drawing CardIcon_Link => GetCardIcon(CardIconType.Link);
        public static Drawing CardIcon_Pendulum => GetCardIcon(CardIconType.Pendulum);
        public static Drawing CardIcon_P_Effect => GetCardIcon(CardIconType.Effect_Pendulum);
        public static Drawing CardIcon_P_Fusion => GetCardIcon(CardIconType.Fusion_Pendulum);
        public static Drawing CardIcon_P_Ritual => GetCardIcon(CardIconType.Ritual_Pendulum);
        public static Drawing CardIcon_P_Synchro => GetCardIcon(CardIconType.Synchro_Pendulum);
        public static Drawing CardIcon_P_Xyz => GetCardIcon(CardIconType.Xyz_Pendulum);
        public static Drawing CardIcon_Spell => GetCardIcon(CardIconType.Spell);
        public static Drawing CardIcon_S_Field => GetCardIcon(CardIconType.Field_Spell);
        public static Drawing CardIcon_S_Equip => GetCardIcon(CardIconType.Equip_Spell);
        public static Drawing CardIcon_S_Continuous => GetCardIcon(CardIconType.Continuous_Spell);
        public static Drawing CardIcon_S_Quick => GetCardIcon(CardIconType.Quick_Spell);
        public static Drawing CardIcon_S_Ritual => GetCardIcon(CardIconType.Ritual_Spell);
        public static Drawing CardIcon_Trap => GetCardIcon(CardIconType.Trap);
        public static Drawing CardIcon_T_Continuous => GetCardIcon(CardIconType.Continuous_Trap);
        public static Drawing CardIcon_T_Counter => GetCardIcon(CardIconType.Counter_Trap);

        public static Drawing AttrIcon_Light => GetAttrIcon(Attribute.Light);
        public static Drawing AttrIcon_Dark => GetAttrIcon(Attribute.Dark);
        public static Drawing AttrIcon_Water => GetAttrIcon(Attribute.Water);
        public static Drawing AttrIcon_Fire => GetAttrIcon(Attribute.Fire);
        public static Drawing AttrIcon_Earth => GetAttrIcon(Attribute.Earth);
        public static Drawing AttrIcon_Wind => GetAttrIcon(Attribute.Wind);
        public static Drawing AttrIcon_Divine => GetAttrIcon(Attribute.Divine);
    }
}
