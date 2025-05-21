using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace LivreNoirLibrary.Media
{
    public static partial class MediaUtils
    {
        private static readonly Dictionary<Color, SolidColorBrush> _brush_cache = [];
        private static readonly Dictionary<(Brush, double), Pen> _pen_cache = [];

        public static T Freeze<T>(T obj)
            where T : Freezable
        {
            obj.Freeze();
            return obj;
        }

        public static SolidColorBrush GetBrush(Color color)
        {
            if (!_brush_cache.TryGetValue(color, out var brush))
            {
                brush = new(color);
                brush.Freeze();
                _brush_cache.Add(color, brush);
            }
            return brush;
        }

        public static SolidColorBrush GetBrush(byte a, byte r, byte g, byte b) => GetBrush(Color.FromArgb(a, r, g, b));
        public static SolidColorBrush GetBrush(string colorCode) => GetBrush(colorCode.ToColor());

        public static Pen? GetPen(Brush? brush, double th)
        {
            if (brush is not null && th > 0)
            {
                var key = (brush, th);
                if (!_pen_cache.TryGetValue(key, out var pen))
                {
                    pen = new(brush, th);
                    pen.Freeze();
                    _pen_cache.Add(key, pen);
                }
                return pen;
            }
            return null;
        }

        public static Pen? GetPen(Color color, double th) => GetPen(GetBrush(color), th);

        public static DrawingBrush CreateHorizontalDashBrush(Color? color1, Color? color2, double len1 = 4, double len2 = 4)
        {
            DrawingGroup group = new();
            RenderOptions.SetEdgeMode(group, EdgeMode.Aliased);

            RectangleGeometry rect = new(new(0, 0, len1, 1));
            rect.Freeze();
            GeometryDrawing gd = new()
            {
                Brush = color1 is Color c1 ? GetBrush(c1) : Brushes.Transparent,
                Geometry = rect,
            };
            gd.Freeze();
            group.Children.Add(gd);

            rect = new(new(len1, 0, len2, 1));
            rect.Freeze();
            gd = new()
            {
                Brush = color2 is Color c2 ? GetBrush(c2) : Brushes.Transparent,
                Geometry = rect,
            };
            gd.Freeze();
            group.Children.Add(gd);

            group.Freeze();
            DrawingBrush brush = new(group)
            {
                Viewport = new Rect(0, 0, len1 + len2, 1),
                TileMode = TileMode.Tile,
                ViewportUnits = BrushMappingMode.Absolute,
            };
            brush.Freeze();
            return brush;
        }

        public static DrawingBrush CreateVerticalDashBrush(Color? color1, Color? color2, double len1 = 4, double len2 = 4)
        {
            DrawingGroup group = new();
            RenderOptions.SetEdgeMode(group, EdgeMode.Aliased);

            RectangleGeometry rect = new(new(0, 0, 1, len1));
            rect.Freeze();
            GeometryDrawing gd = new()
            {
                Brush = color1 is Color c1 ? GetBrush(c1) : Brushes.Transparent,
                Geometry = rect,
            };
            gd.Freeze();
            group.Children.Add(gd);

            rect = new(new(0, len1, 1, len2));
            rect.Freeze();
            gd = new()
            {
                Brush = color2 is Color c2 ? GetBrush(c2) : Brushes.Transparent,
                Geometry = rect,
            };
            gd.Freeze();
            group.Children.Add(gd);

            group.Freeze();
            DrawingBrush brush = new(group)
            {
                Viewport = new Rect(0, 0, 1, len1 + len2),
                TileMode = TileMode.Tile,
                ViewportUnits = BrushMappingMode.Absolute,
            };
            brush.Freeze();
            return brush;
        }
    }
}
