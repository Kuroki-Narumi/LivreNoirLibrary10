using System;
using System.Windows;
using System.Windows.Media;

namespace LivreNoirLibrary.Media
{
    public static partial class MediaUtils
    {
        public static StreamGeometry CreateGeometry(string data)
        {
            var g = (StreamGeometry)Geometry.Parse(data);
            g.Freeze();
            return g;
        }

        public static RectangleGeometry CreateRectGeometry(Rect rect)
        {
            RectangleGeometry g = new(rect);
            g.Freeze();
            return g;
        }

        public static RectangleGeometry CreateRectGeometry(double x, double y, double width, double height) => CreateRectGeometry(new(x, y, width, height));

        public static void DrawTriangle(this StreamGeometryContext ctx, double ox, double oy, double dx1, double dy1, double dx2, double dy2, bool isFilled = true, bool isStroked = true, bool isSmoothJoin = false)
        {
            ctx.BeginFigure(new(ox, oy), isFilled, true);
            ctx.LineTo(new(ox + dx1, oy + dy1), isStroked, isSmoothJoin);
            ctx.LineTo(new(ox + dx2, oy + dy2), isStroked, isSmoothJoin);
        }
    }
}
