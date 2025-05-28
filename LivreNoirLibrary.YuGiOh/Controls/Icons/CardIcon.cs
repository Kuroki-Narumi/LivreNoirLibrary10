using System;
using System.Collections.Generic;
using System.Windows.Media;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.YuGiOh.Controls
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

        public static CardIconType GetIconType(CardType type, bool effect, Ability ability) => GetIconType(type, effect, ability.IsPendulum());

        private static readonly Dictionary<CardIconType, Color> _frame_colors = new()
        {
            { CardIconType.Normal, Color.FromArgb(255, 255, 216, 128) }, // #ffd880
            { CardIconType.Effect, Color.FromArgb(255, 255, 176, 128) }, // #ffb060
            { CardIconType.Fusion, Color.FromArgb(255, 216, 128, 255) }, // #d880ff
            { CardIconType.Ritual, Color.FromArgb(255, 172, 172, 255) }, // #ababff
            { CardIconType.Synchro, Color.FromArgb(255, 255, 255, 255) }, // #ffffff
            { CardIconType.Xyz, Color.FromArgb(255, 32, 32, 32) }, // #404040
            { CardIconType.Link, Color.FromArgb(255, 32, 208, 255) }, // #20d0ff
            { CardIconType.Spell, Color.FromArgb(255, 64, 255, 176) }, // #40ffb0
            { CardIconType.Trap, Color.FromArgb(255, 255, 128, 216) }, // #ff80d8
            { CardIconType.Token, Color.FromArgb(255, 160, 160, 160) }, // #a0a0a0
        };
        private static readonly Dictionary<CardIconType, Brush> _frame_brushes = [];
        public static Brush GetFrameBrush(CardIconType type)
        {
            if (!_frame_brushes.TryGetValue(type, out var brush))
            {
                if ((type & CardIconType.Pendulum) is not 0)
                {
                    var c1 = _frame_colors[type & CardIconType.Monster];
                    var c2 = _frame_colors[CardIconType.Spell];
                    brush = new LinearGradientBrush()
                    {
                        StartPoint = new(0, 0),
                        EndPoint = new(0, 1),
                        GradientStops =
                        [
                            new(c1, 0.5),
                            new(c2, 1.0),
                        ],
                    };
                }
                else
                {
                    var t = (type & CardIconType.Spell) is not 0 ? CardIconType.Spell :
                            (type & CardIconType.Trap) is not 0 ? CardIconType.Trap :
                            type;
                    brush = new SolidColorBrush(_frame_colors[t]);
                }
                brush.Freeze();
                _frame_brushes.Add(type, brush);
            }
            return brush;
        }

        public const int CardFrameWidth = 12;
        public const int CardIllustY = 4;
        public const int CardIllustSize = 6;

        public static readonly SolidColorBrush Brush_CardFrame = MediaUtils.GetBrush(255, 128, 128, 128);
        public static readonly SolidColorBrush Brush_Illust = MediaUtils.GetBrush(255, 128, 128, 128);

        private static readonly Dictionary<CardIconType, Geometry> _card_geometries = new()
        {
            { CardIconType.Field_Spell, CreateGeometry("M6,2 v10 l2,-3 l-4,-4 Z M1,7 h10 l-3,-2 l-4,4 Z") },
            { CardIconType.Equip_Spell, CreateGeometry("M2,6 h3 v-3 h2 v3 h3 v2 h-3 v3 h-2 v-3 h-3") },
            { CardIconType.Continuous_Spell, CreateGeometry("M1,7 a3,3,0,0,0,5,2.236 a3,3,0,0,0,5,-2.236 a3,3,0,0,0,-5,-2.236 a3,3,0,0,0,-5,2.236 Z M2.5,7 a1.5,1.5,0,0,0,3,0 a1.5,1.5,0,0,0,-3,0 Z M6.5,7 a1.5,1.5,0,0,0,3,0 a1.5,1.5,0,0,0,-3,0 Z") },
            { CardIconType.Quick_Spell, CreateGeometry("M6,3 l-3,3 l3,2 l-3,3 h3 l3,-3 l-3,-2 l3,-3 Z") },
            { CardIconType.Ritual_Spell, CreateGeometry("M6,3 l-2,4 l-2,-2 l1,4 l3,2 l3,-2 l1,-4 l-2,2 Z") },
            { CardIconType.Continuous_Trap, CreateGeometry("M1,7 a3,3,0,0,0,5,2.236 a3,3,0,0,0,5,-2.236 a3,3,0,0,0,-5,-2.236 a3,3,0,0,0,-5,2.236 Z M2.5,7 a1.5,1.5,0,0,0,3,0 a1.5,1.5,0,0,0,-3,0 Z M6.5,7 a1.5,1.5,0,0,0,3,0 a1.5,1.5,0,0,0,-3,0 Z") },
            { CardIconType.Counter_Trap, CreateGeometry("M1,8 l4,-4 v3 h2 l1,-1 l1,-4 l1,5 l-2,2 h-3 v3 Z") },
        };

        private static readonly Dictionary<CardIconType, DrawingImage> _card_cache = [];
        public static DrawingImage GetCardIcon(CardIconType type)
        {
            if (!_card_cache.TryGetValue(type, out var di))
            {
                DrawingGroup dg = new();
                using (var ctx = dg.Open())
                {
                    ctx.DrawRectangle(Brush_CardFrame, null, new(0, 0, CardFrameWidth, IconHeight));
                    ctx.DrawRectangle(GetFrameBrush(type), null, new(1, 1, CardFrameWidth - 2, IconHeight - 2));
                    if (_card_geometries.TryGetValue(type, out var g))
                    {
                        ctx.DrawGeometry(Brush_Illust, null, g);
                    }
                    else
                    {
                        var x = (CardFrameWidth - CardIllustSize) / 2;
                        ctx.DrawRectangle(Brush_Illust, null, new(x, CardIllustY, CardIllustSize, CardIllustSize));
                    }
                }
                dg.Freeze();
                di = new(dg);
                di.Freeze();
                _card_cache.Add(type, di);
            }
            return di;
        }
        public static DrawingImage GetCardIcon(CardType type, bool effect = true, bool pendulum = false) => GetCardIcon(GetIconType(type, effect, pendulum));
    }
}
