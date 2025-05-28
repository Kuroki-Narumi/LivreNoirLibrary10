using System;
using System.Collections.Generic;
using System.Windows.Media;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.YuGiOh.Controls
{
    public static partial class Icons
    {
        public static readonly SolidColorBrush Link_On_Fill = MediaUtils.GetBrush(255, 255, 0, 0);
        public static readonly SolidColorBrush Link_On_Stroke = MediaUtils.GetBrush(255, 192, 64, 64);
        public static readonly SolidColorBrush Link_Off_Fill = MediaUtils.GetBrush(128, 128, 128, 128);
        public static readonly SolidColorBrush Link_Off_Stroke = MediaUtils.GetBrush(192, 64, 64, 64);

        private static readonly Dictionary<LinkDirection, DrawingImage> _link_cache = [];
        public static DrawingImage GetLinkIcon(LinkDirection dir)
        {
            if (!_link_cache.TryGetValue(dir, out var di))
            {
                DrawingGroup dg = new();
                using (var ctx = dg.Open())
                {
                    ctx.DrawGeometry(Link_Off_Stroke, null, GetLinkGeometry(~dir, IconWidth, IconHeight));
                    ctx.DrawGeometry(Link_On_Fill, null, GetLinkGeometry(dir, IconWidth, IconHeight));
                }
                dg.Freeze();
                di = new(dg);
                di.Freeze();
                _link_cache.Add(dir, di);
            }
            return di;
        }

        private static void LineTriangle(StreamGeometryContext ctx, double ox, double oy, double dx1, double dy1, double dx2, double dy2)
        {
            ctx.BeginFigure(new(ox, oy), true, true);
            ctx.LineTo(new(ox + dx1, oy + dy1), true, false);
            ctx.LineTo(new(ox + dx2, oy + dy2), true, false);
        }

        public static Geometry GetLinkGeometry(LinkDirection dir, double width, double height)
        {
            StreamGeometry g = new();
            var w1 = width / 4.0;
            var w2 = width / 5.0;
            var x1 = width / 40.0;
            var x2 = width / 2.0;
            var x3 = x1 * 39.0;
            var h1 = height / 4.0;
            var h2 = height / 5.0;
            var y1 = height / 40.0;
            var y2 = height / 2.0;
            var y3 = y1 * 39.0;
            using (var ctx = g.Open())
            {
                if ((dir & LinkDirection.LowerLeft) is not 0)
                {
                    LineTriangle(ctx, x1, y3, 0, -h1, w1, 0);
                }
                if ((dir & LinkDirection.Lower) is not 0)
                {
                    LineTriangle(ctx, x2, height, -w2, -h2, w2, -h2);
                }
                if ((dir & LinkDirection.LowerRight) is not 0)
                {
                    LineTriangle(ctx, x3, y3, 0, -h1, -w1, 0);
                }
                if ((dir & LinkDirection.Left) is not 0)
                {
                    LineTriangle(ctx, 0, y2, w2, -h2, w2, h2);
                }
                if ((dir & LinkDirection.Right) is not 0)
                {
                    LineTriangle(ctx, width, y2, -w2, -h2, -w2, h2);
                }
                if ((dir & LinkDirection.UpperLeft) is not 0)
                {
                    LineTriangle(ctx, x1, y1, 0, h1, w1, 0);
                }
                if ((dir & LinkDirection.Upper) is not 0)
                {
                    LineTriangle(ctx, x2, 0, -w2, h2, w2, h2);
                }
                if ((dir & LinkDirection.UpperRight) is not 0)
                {
                    LineTriangle(ctx, x3, y1, 0, h1, -w1, 0);
                }
            }
            g.Freeze();
            return g;
        }
    }
}
