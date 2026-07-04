using LivreNoirLibrary.Media.VectorGraphics;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.YuGiOh.Media
{
    public static partial class Icons
    {
        public static CardIconType GetIconType(CardType type, bool effect = true, bool pendulum = false)
        {
            CardIconType result;
            switch (type)
            {
                case CardType.Main_Monster:
                    result = effect ? CardIconType.Effect : CardIconType.Normal;
                    break;
                case CardType.Fusion_Monster:
                    result = CardIconType.Fusion;
                    break;
                case CardType.Ritual_Monster:
                    result = CardIconType.Ritual;
                    break;
                case CardType.Synchro_Monster:
                    result = CardIconType.Synchro;
                    break;
                case CardType.Xyz_Monster:
                    result = CardIconType.Xyz;
                    break;
                case CardType.Link_Monster:
                    result = CardIconType.Link;
                    break;
                default:
                    return (CardIconType)type;
            }
            if (pendulum)
            {
                result |= CardIconType.Pendulum;
            }
            return result;
        }

        public static CardIconType GetIconType(ICard card) => GetIconType(card.CardType, card.HasEffect, card.Ability);

        public static CardIconType GetIconType(CardType type, bool effect, Ability ability) => GetIconType(type, effect, ability.IsPendulum());

        private static readonly Dictionary<CardIconType, string> _frame_colors = new()
        {
            { CardIconType.Normal, "#ffd880" },
            { CardIconType.Effect, "#ffb080" },
            { CardIconType.Fusion, "#d880ff" },
            { CardIconType.Ritual, "#acacff" },
            { CardIconType.Synchro, "#ffffff" },
            { CardIconType.Xyz, "#202020" },
            { CardIconType.Link, "#20d0ff" },
            { CardIconType.Spell, "#40ffb0" },
            { CardIconType.Trap, "#ff80d8" },
            { CardIconType.Token, "#a0a0a0" },
        };

        private static readonly Dictionary<CardIconType, IBrush> _frame_brushes = [];
        public static IBrush GetFrameBrush(CardIconType type)
        {
            if (!_frame_brushes.TryGetValue(type, out var brush))
            {
                {
                    if ((type & CardIconType.Pendulum) is not 0)
                    {
                        var c1 = _frame_colors[type & CardIconType.Monster];
                        var c2 = _frame_colors[CardIconType.Spell];
                        brush = new GradientBrush(
                            GradientType.Vertical,
                            (0, 0),
                            [new(0.5, c1), new(1, c2)]
                            );
                    }
                    else
                    {
                        var t = (type & CardIconType.Spell) is not 0 ? CardIconType.Spell :
                                (type & CardIconType.Trap) is not 0 ? CardIconType.Trap :
                                type;
                        brush = new SingleColorBrush(_frame_colors[t]);
                    }
                    _frame_brushes.Add(type, brush);
                }
            }
            return brush;
        }

        public const int CardFrameWidth = 24;
        public const int CardFrameHeight = 32;
        public const int CardIllustY = 8;
        public const int CardIllustSize = 12;
        public const int CardIllustX = (CardFrameWidth - CardIllustSize) / 2;

        const string _continuous_geometry =
            "M2,14 a6,6,0,0,0,10,4.472 a6,6,0,0,0,10,-4.472 a6,6,0,0,0,-10,-4.472 a6,6,0,0,0,-10,4.472 Z M5,14 a3,3,0,0,0,6,0 a3,3,0,0,0,-6,0 Z M13,14 a3,3,0,0,0,6,0 a3,3,0,0,0,-6,0 Z";

        private static readonly Dictionary<CardIconType, string> _card_geometries = new()
        {
            { CardIconType.Field_Spell, "M12,4 v20 l4,-6 l-8,-8 Z M2,14 h20 l-6,-4 l-8,8 Z" },
            { CardIconType.Equip_Spell, "M4,12 h6 v-6 h4 v6 h6 v4 h-6 v6 h-4 v-6 h-6" },
            { CardIconType.Continuous_Spell, _continuous_geometry },
            { CardIconType.Quick_Spell, "M12,6 l-6,6 l6,4 l-6,6 h6 l6,-6 l-6,-4 l6,-6 Z" },
            { CardIconType.Ritual_Spell, "M12,6 l-4,8 l-4,-4 l2,8 l6,4 l6,-4 l2,-8 l-4,4 Z" },
            { CardIconType.Continuous_Trap, _continuous_geometry },
            { CardIconType.Counter_Trap, "M2,16 l8,-8 v6 h4 l2,-2 l2,-8 l2,10 l-4,4 h-6 v6 Z" },
        };

        private static readonly string _card_frame_geometry = $"M0,0 h{CardFrameWidth} v{CardFrameHeight} h-{CardFrameWidth} Z";
        private static readonly string _card_inner_frame_geometry = $"M1,1 h{CardFrameWidth - 2} v{CardFrameHeight - 2} h-{CardFrameWidth - 2} Z";
        private static readonly string _card_illust_geometry = $"M{CardIllustX},{CardIllustY} h{CardIllustSize} v{CardIllustSize} h-{CardIllustSize} Z";

        private static readonly Dictionary<CardIconType, ElementGroup> _card_icons = [];

        public static ElementGroup GetCardIcon(CardIconType type)
        {
            if (!_card_icons.TryGetValue(type, out var icon))
            {
                GeometryElement[] children = 
                [
                    new(_card_frame_geometry, Brush_Gray),
                    new(_card_inner_frame_geometry, GetFrameBrush(type)),
                    new(_card_geometries.GetValueOrDefault(type, _card_illust_geometry), Brush_Gray),
                ];
                icon = new ElementGroup(children);
                _card_icons.Add(type, icon);
            }
            return icon;
        }

        public static ElementGroup GetCardIcon(CardType type, bool effect = true, bool pendulum = false) => GetCardIcon(GetIconType(type, effect, pendulum));
    }
}
