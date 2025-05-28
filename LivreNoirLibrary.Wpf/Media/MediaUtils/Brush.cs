using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace LivreNoirLibrary.Media
{
    public static partial class MediaUtils
    {
        public const double DefaultDashLength = 4;
        public const double DefaultCheckerSize = 8;

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
            if (brush is not null && th is > 0)
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
        public static Pen? GetPen(string colorCode, double th) => GetPen(GetBrush(colorCode), th);

        public static DrawingBrush CreateHorizontalDashBrush(Color color1, Color color2, double len1 = DefaultDashLength, double len2 = DefaultDashLength)
        {
            DrawingGroup group = new();
            RenderOptions.SetEdgeMode(group, EdgeMode.Aliased);

            group.Children.Add(Freeze(new GeometryDrawing()
            {
                Brush = GetBrush(color1),
                Geometry = CreateRectGeometry(new(0, 0, len1, 1)),
            }));
            group.Children.Add(Freeze(new GeometryDrawing()
            {
                Brush = GetBrush(color2),
                Geometry = CreateRectGeometry(new(len1, 0, len2, 1)),
            }));
            group.Freeze();

            return Freeze(new DrawingBrush(group)
            {
                Viewport = new(0, 0, len1 + len2, 1),
                TileMode = TileMode.Tile,
                ViewportUnits = BrushMappingMode.Absolute,
            });
        }
        public static DrawingBrush CreateHorizontalDashBrush(string color1Code, string color2Code, double len1 = DefaultDashLength, double len2 = DefaultDashLength)
            => CreateHorizontalDashBrush(color1Code.ToColor(), color2Code.ToColor(), len1, len2);

        public static DrawingBrush CreateVerticalDashBrush(Color color1, Color color2, double len1 = DefaultDashLength, double len2 = DefaultDashLength)
        {
            DrawingGroup group = new();
            RenderOptions.SetEdgeMode(group, EdgeMode.Aliased);

            group.Children.Add(Freeze(new GeometryDrawing()
            {
                Brush = GetBrush(color1),
                Geometry = CreateRectGeometry(new(0, 0, 1, len1)),
            }));
            group.Children.Add(Freeze(new GeometryDrawing()
            {
                Brush = GetBrush(color2),
                Geometry = CreateRectGeometry(new(0, len1, 1, len2)),
            }));
            group.Freeze();

            return Freeze(new DrawingBrush(group)
            {
                Viewport = new(0, 0, 1, len1 + len2),
                TileMode = TileMode.Tile,
                ViewportUnits = BrushMappingMode.Absolute,
            });
        }
        public static DrawingBrush CreateVerticalDashBrush(string color1Code, string color2Code, double len1 = DefaultDashLength, double len2 = DefaultDashLength)
            => CreateVerticalDashBrush(color1Code.ToColor(), color2Code.ToColor(), len1, len2);

        public static DrawingBrush TransparentCheckerBrush { get; } = CreateTransparentCheckerBrush("#eee", "#ccc");

        public static DrawingBrush CreateTransparentCheckerBrush(Color color1, Color color2, double size = DefaultCheckerSize)
        {
            DrawingGroup dg = new();
            RenderOptions.SetEdgeMode(dg, EdgeMode.Aliased);

            Rect viewport = new(0, 0, size * 2, size * 2);
            dg.Children.Add(Freeze(new GeometryDrawing() 
            {
                Brush = GetBrush(color1),
                Geometry = CreateRectGeometry(viewport),
            }));
            var rectExpr = $"h{size} v{size} h-{size} Z";
            dg.Children.Add(Freeze(new GeometryDrawing()
            {
                Brush = GetBrush(color2),
                Geometry = CreateGeometry($"M0,0 {rectExpr} M{size},{size} {rectExpr}"),
            }));
            dg.Freeze();

            return Freeze(new DrawingBrush(dg)
            {
                Viewport = viewport,
                TileMode = TileMode.Tile,
                ViewportUnits = BrushMappingMode.Absolute,
            });
        }
        public static DrawingBrush CreateTransparentCheckerBrush(string color1Code, string color2Code, double size = DefaultCheckerSize)
            => CreateTransparentCheckerBrush(color1Code.ToColor(), color2Code.ToColor(), size);
    }
}
