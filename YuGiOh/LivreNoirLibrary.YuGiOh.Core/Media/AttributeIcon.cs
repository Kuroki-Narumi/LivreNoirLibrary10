using System;
using System.Collections.Generic;
using System.Text;
using LivreNoirLibrary.Media.VectorGraphics;

namespace LivreNoirLibrary.YuGiOh.Media
{
    public static partial class Icons
    {
        private static readonly Dictionary<Attribute, GradientBrush> _attr_brushes = new()
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

        private static GradientBrush CreateAttrBrush(string c1, string c2, string c3)
        {
            GradientStop[] stops = [new(0, c1), new(0.6, c2), new(1, c3)];
            return new(GradientType.Radial, (0.25, 0.25), stops);
        }

        private static readonly Dictionary<Attribute, ElementGroup> _attr_icons = [];

        public static ElementGroup GetAttributeIcon(Attribute attr)
        {
            if (!_attr_icons.TryGetValue(attr, out var icon))
            {
                GeometryElement[] elements = 
                [
                    new(Geometries.Circle_16, Brush_Gray),
                    new(Geometries.Circle_15, _attr_brushes.GetValueOrDefault(attr)),
                ];
                icon = new(elements);
                _attr_icons.Add(attr, icon);
            }
            return icon;
        }
    }
}
