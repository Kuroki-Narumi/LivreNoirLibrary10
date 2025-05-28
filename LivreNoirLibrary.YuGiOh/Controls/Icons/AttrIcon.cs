using System;
using System.Collections.Generic;
using System.Windows.Media;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.YuGiOh.Controls
{
    public static partial class Icons
    {
        private static readonly Dictionary<Attribute, RadialGradientBrush> _attr_brushes = new()
        {
            { Attribute.None, CreateAttrBrush("#ddd", "#aaa", "#444") },
            { Attribute.Light, CreateAttrBrush("#ff4", "#ff0", "#990") },
            { Attribute.Dark, CreateAttrBrush("#e05", "#d0e", "#70a") },
            { Attribute.Water, CreateAttrBrush("#bbf", "#88f", "#22a") },
            { Attribute.Fire, CreateAttrBrush("#f80", "#f00", "#800") },
            { Attribute.Earth, CreateAttrBrush("#a98", "#842", "#330") },
            { Attribute.Wind, CreateAttrBrush("#dfd", "#8f8", "#080") },
            { Attribute.Divine, CreateAttrBrush("#fff", "#8ff", "#888") },
        };
        private static RadialGradientBrush CreateAttrBrush(string c1, string c2, string c3)
        {
            RadialGradientBrush brush = new()
            {
                GradientOrigin = new(0.25, 0.25),
            };
            GradientStop g = new(ColorExtension.ToColor(c1), 0.0);
            g.Freeze();
            brush.GradientStops.Add(g);
            g = new(ColorExtension.ToColor(c2), 0.6);
            g.Freeze();
            brush.GradientStops.Add(g);
            g = new(ColorExtension.ToColor(c3), 1.0);
            g.Freeze();
            brush.GradientStops.Add(g);
            brush.Freeze();
            return brush;
        }

        private static readonly Dictionary<Attribute, DrawingImage> _attr_cache = [];
        public static DrawingImage GetAttrIcon(Attribute attr)
        {
            if (!_attr_cache.TryGetValue(attr, out var di))
            {
                DrawingGroup dg = new();
                using (var ctx = dg.Open())
                {
                    double s = IconWidth / 2;
                    ctx.DrawEllipse(Brush_CardFrame, null, new(s, s), s, s);
                    if (_attr_brushes.TryGetValue(attr, out var brush))
                    {
                        ctx.DrawEllipse(brush, null, new(s, s), s - 1, s - 1);
                    }
                }
                dg.Freeze();
                di = new(dg);
                di.Freeze();
                _attr_cache.Add(attr, di);
            }
            return di;
        }
    }
}
