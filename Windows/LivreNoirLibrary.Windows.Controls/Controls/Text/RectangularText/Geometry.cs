using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media.VectorGraphics;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class RectangularText
    {
        public static void DrawToGeometry(StreamGeometryContext ctx, double ox, double oy, ReadOnlySpan<char> text, double th = DefaultThickness, double th2 = DefaultStrokeThickness)
        {
            var x = ox + th2;
            var y = oy + th2;
            foreach (var c in text)
            {
                if (c is '\n' or '\f')
                {
                    x = ox + th2;
                    y += 5 * th + th2;
                }
                else
                {
                    double w;
                    if (_path_data.TryGetValue(c, out var data))
                    {
                        data.Parser.Draw(ctx, x, y, th);
                        w = data.Width;
                    }
                    else
                    {
                        w = 3;
                    }
                    x += w * th + th2;
                }
            }
        }

        public static StreamGeometry CreateGeometry(double offsetX, double offsetY, ReadOnlySpan<char> text, double th = DefaultThickness, double th2 = DefaultStrokeThickness)
        {
            var g = CreateGometryCore(text, th, th2);
            TranslateTransform t = new(offsetX, offsetY);
            t.Freeze();
            g.Transform = t;
            g.Freeze();
            return g;
        }

        public static StreamGeometry CreateGeometry(ReadOnlySpan<char> text, double th, double th2)
        {
            var g = CreateGometryCore(text, th, th2);
            g.Freeze();
            return g;
        }

        private static readonly Dictionary<(double, double), Dictionary<string, StreamGeometry>.AlternateLookup<ReadOnlySpan<char>>> _geometries = [];
        private static Dictionary<string, StreamGeometry>.AlternateLookup<ReadOnlySpan<char>> CreateGeometryLookup((double, double) _)
        {
            Dictionary<string, StreamGeometry> dic = new(StringComparer.OrdinalIgnoreCase);
            return dic.GetAlternateLookup<ReadOnlySpan<char>>();
        }

        private static StreamGeometry CreateGometryCore(ReadOnlySpan<char> text, double th, double th2)
        {
            var key = (th, th2);
            var dic = _geometries.GetOrAdd(key, CreateGeometryLookup);
            if (!dic.TryGetValue(text, out var geometry))
            {
                geometry = new();
                using (var ctx = geometry.Open())
                {
                    DrawToGeometry(ctx, 0, 0, text, th, th2);
                }
                geometry.Freeze();
                dic[text] = geometry;
            }
            return geometry;
        }

        public static (double Width, double Height) CalcSize(string text, double th1 = DefaultThickness, double th2 = DefaultStrokeThickness)
        {
            var x = th2;
            var y = th2;
            var max = 0.0;
            void nextChar(double width)
            {
                x += width * th1 + th2;
            }
            void nextLine()
            {
                if (x > max)
                {
                    max = x;
                }
                x = th2;
                y += 5 * th1 + th2;
            }
            foreach (char c in text)
            {
                if (c is '\n' or '\f')
                {
                    nextLine();
                }
                else if (_path_data.TryGetValue(c, out var data))
                {
                    nextChar(data.Width);
                }
                else
                {
                    nextChar(3);
                }
            }
            nextLine();
            return (max, y);
        }

        private static readonly Dictionary<char, (double Width, Path Parser)> _path_data = new()
        {
            {'0', (3, "M0,0 h3 v5 h-3 Z M1,1 h1 v3 h-1 Z")},
            {'1', (3, "M1,0 h1 v5 h-1 Z")},
            {'2', (3, "M0,0 h3 v3 h-2 v1 h2 v1 h-3 v-3 h2 v-1 h-2 Z")},
            {'3', (3, "M0,0 h3 v5 h-3 v-1 h2 v-1 h-2 v-1 h2 v-1 h-2 Z")},
            {'4', (3, "M0,0 h1 v2 h1 v-2 h1 v5 h-1 v-2 h-2 Z")},
            {'5', (3, "M0,0 h3 v1 h-2 v1 h2 v3 h-3 v-1 h2 v-1 h-2 Z")},
            {'6', (3, "M0,0 h3 v1 h-2 v1 h2 v3 h-3 Z M1,3 h1 v1 h-1 Z")},
            {'7', (3, "M0,0 h3 v5 h-1 v-4 h-2 Z")},
            {'8', (3, "M0,0 h3 v5 h-3 Z M1,1 h1 v1 h-1 Z M1,3 h1 v1 h-1 Z")},
            {'9', (3, "M0,0 h3 v5 h-3 v-1 h2 v-1 h-2 Z M1,1 h1 v1 h-1 Z")},

            {'(', (2, "M1,0 L0,1 V4 L1,5 H2 V4 H1.3 L1,3.7 V1.3 L1.3,1 H2 V0 Z")},
            {')', (2, "M1,0 L2,1 V4 L1,5 H0 V4 H0.7 L1,3.7 V1.3 L0.7,1 H0 V0 Z")},
            {'.', (1, "M0,4 h1 v1 h-1 Z")},
            {',', (1, "M0,3 0,4 0.5,4 0.5,4.5 0,5 0.7,5 1,4.7 1,3 Z")},
            {':', (1, "M0,1 h1 v1 h-1 Z M0,3 h1 v1 h-1 Z")},
            {';', (1, "M0,1 h1 v1 h-1 Z M0,3 0,4 0.5,4 0.5,4.5 0,5 0.7,5 1,4.7 1,3 Z")},
            {'+', (3, "M1,1 h1 v1 h1 v1 h-1 v1 h-1 v-1 h-1 v-1 h1 Z")},
            {'-', (3, "M0,2 h3 v1 h-3 Z")},
            {'*', (3, "M0,1 0.7,1 1.5,1.8 2.3,1 3,1 3,1.7 2.2,2.5 3,3.3 3,4 2.3,4 1.5,3.2 0.7,4 0,4 0,3.3 0.8,2.5 0,1.7 Z")},
            {'\\', (3, "M0.7,0 3,4.3 3,5 2.3,5 0,0.7 0,0 Z")},
            {'/', (3, "M2.3,0 0,4.3 0,5 0.7,5 3,0.7 3,0 Z")},
            {'^', (3, "M1,0 0,1 0,2 1,2 1,1 2,1 2,2 3,2 3,1 2,0 Z")},
            {'=', (3, "M0,1 h3 v1 h-3 Z M0,3 h3 v1 h-3 Z")},

            {'A', (3, "M1,0 0,1 0,5 1,5 1,3 2,3 2,5 3,5 3,1 2,0 Z M1,1 h1 v1 h-1 Z")},
            {'B', (3, "M0,0 0,5 2,5 3,4 3,3 2.5,2.5 3,2 3,1 2,0 Z M1,1 h1 v1 h-1 Z M1,3 h1 v1 h-1 Z")},
            {'C', (3, "M1,0 0,1 0,4 1,5 2,5 3,4 3,3 2,3 2,4 1,4 1,1 2,1 2,2 3,2 3,1 2,0 Z")},
            {'D', (3, "M0,0 0,5 2,5 3,4 3,1 2,0 Z M1,1 h1 v3 h-1 Z")},
            {'E', (3, "M0,0 h3 v1 h-2 v1 h2 v1 h-2 v1 h2 v1 h-3 Z")},
            {'F', (3, "M0,0 h3 v1 h-2 v1 h1.5 v1 h-1.5 v2 h-1 Z")},
            {'G', (3, "M1,0 0,1 0,4 1,5 2,5 3,4 3,2.5 1.5,2.5 1.5,3.3 2,3.3 2,4 1,4 1,1 2,1 2,1.5 3,1.5 3,1 2,0 Z")},
            {'H', (3, "M0,0 h1 v2 h1 v-2 h1 v5 h-1 v-2 h-1 v2 h-1 Z")},
            {'I', (3, "M0,0 h3 v1 h-1 v3 h1 v1 h-3 v-1 h1 v-3 h-1 Z")},
            {'J', (3, "M2,0 h1 v5 h-3 v-2 h1 v1 h1 Z")},
            {'K', (3, "M0,0 0,5 1,5 1,3 2,4 2,5 3,5 3,3.7 1.8,2.5 3,1.3 3,0 2,0 2,1 1,2 1,0 Z")},
            {'L', (3, "M0,0 h1 v4 h2 v1 h-3 Z")},
            {'M', (3, "M0,0 0,5 1,5 1,4.5 0.7,2 1,2 1.3,4.5 1.7,4.5 2,2 2.3,2 2,4.5 2,5 3,5 3,0 2,0 1.5,1.5 1,0 Z")},
            {'N', (3, "M0,0 0,5 1,5 1,2.5 2,5 3,5 3,0 2,0 2,2.5 1,0 Z")},
            {'O', (3, "M1,0 0,1 0,4 1,5 2,5 3,4 3,1 2,0 Z M1,1 h1 v3 h-1 Z")},
            {'P', (3, "M0,0 0,5 1,5 1,3 2,3 3,2 3,1 2,0 Z M1,1 h1 v1 h-1 Z")},
            {'Q', (3, "M1,0 0,1 0,4 1,5 2,5 1,4 1,1 2,1 2,3 3,4 3,1 2,0 Z M3,5 3,4.5 2,3.5 1.5,3.5 1.5,4 2.5,5 Z")},
            {'R', (3, "M0,0 0,5 1,5 1,3 1.5,3 2,3.5 2,5 3,5 3,3 2.5,2.5 3,2 3,1 2,0 Z M1,1 h1 v1 h-1 Z")},
            {'S', (3, "M1,0 0,1 0,2 1,3 2,3 2,4 0,4 0,5 2,5 3,4 3,3 2,2 1,2 1,1 3,1 3,0 Z")},
            {'T', (3, "M0,0 h3 v1 h-1 v4 h-1 v-4 h-1 Z")},
            {'U', (3, "M0,0 0,4 1,5 2,5 3,4 3,0 2,0 2,4 1,4 1,0 Z")},
            {'V', (3, "M0,0 0,2 1.3,5 1.7,5 3,2 3,0 2,0 2,2 1.5,3.3 1,2 1,0 Z")},
            {'W', (3, "M0,0 0,3 0.8,5 1.2,5 1.5,4 1.8,5 2.2,5 3,3 3,0 2,0 2,0.5 2.3,3 2,3 1.7,0.5 1.3,0.5 1,3 0.7,3 1,0.5 1,0 Z")},
            {'X', (3, "M0,0 0,1 1,2.5 0,4 0,5 1,5 1.5,3.5 2,5 3,5 3,4 2,2.5 3,1 3,0 2,0 1.5,1.5 1,0 Z")},
            {'Y', (3, "M0,0 0,1 1,3 1,5 2,5 2,3 3,1 3,0 2,0 2,1 1.5,2 1,1 1,0 Z")},
            {'Z', (3, "M0,0 0,1 2,1 0,4 0,5 3,5 3,4 1,4 3,1 3,0 Z")},

            {'!', (3, "M1,0 h1 v3 h-1 Z M1,4 h1 v1 h-1 Z")},
            {'?', (3, "M0,0 h3 v3 h-2 v-1 h1 v-1 h-2 Z M1,4 h1 v1 h-1 Z")},
            {'#', (3, "M0,1 h0.5 v-1 h0.75 v1 h0.5 v-1 h0.75 v1 h0.5 v1 h-0.5 v1 h0.5 v1 h-0.5 v1 h-0.75 v-1 h-0.5 v1 h-0.75 v-1 h-0.5 v-1 h0.5 v-1 h-0.5 Z M1.25,2 h0.5 v1 h-0.5 Z")},
        };

        private static void InitializePathData()
        {
            var offset = 'A' - 'a';
            for (var c = 'a'; c <= 'z'; c++)
            {
                _path_data[c] = _path_data[(char)(c + offset)];
            }
        }

        private class Path(string data)
        {
            public static implicit operator Path(string data) => new(data);

            private readonly List<PathData> _data = PathParser.Parse(data);

            public void Draw(StreamGeometryContext ctx, double ox, double oy, double th)
            {
                // current position
                var x = ox;
                var y = oy;

                static void Update(ref double current, double origin, float value, double th, bool relative)
                {
                    current = (relative ? current : origin) + value * th;
                }

                foreach (var data in _data.AsSpan())
                {
                    switch (data.Command)
                    {
                        case PathCommand.Moveto:
                            Update(ref x, ox, data.Arg1, th, data.Relative);
                            Update(ref y, oy, data.Arg2, th, data.Relative);
                            ctx.BeginFigure(new(x, y), true, true);
                            continue;
                        case PathCommand.Lineto:
                            Update(ref x, ox, data.Arg1, th, data.Relative);
                            Update(ref y, oy, data.Arg2, th, data.Relative);
                            break;
                        case PathCommand.HorizontalLineto:
                            Update(ref x, ox, data.Arg1, th, data.Relative);
                            break;
                        case PathCommand.VerticalLineto:
                            Update(ref y, oy, data.Arg1, th, data.Relative);
                            break;
                    }
                    ctx.LineTo(new(x, y), true, false);
                }
            }
        }
    }
}