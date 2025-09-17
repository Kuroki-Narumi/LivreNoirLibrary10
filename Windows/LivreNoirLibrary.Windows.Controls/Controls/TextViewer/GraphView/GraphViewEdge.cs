using System;
using System.Windows.Media;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Windows.Controls
{
    public class GraphViewEdge(GraphEdge edge)
    {
        public string Name { get; set; } = edge.Name;
        public GraphNode Start { get; set; } = edge.Start;
        public GraphNode End { get; set; } = edge.End;
        public double CX { get; set; } = double.NaN;
        public double CY { get; set; } = double.NaN;
        public bool IsSelected { get; set; }

        private double _x1;
        private double _y1;
        private double _x2;
        private double _y2;
        public Geometry? Geometry { get; private set; }

        public bool ContainsPoint(double x, double y, double th)
        {
            var dx = x - CX;
            var dy = y - CY;
            return (dx * dx + dy * dy) < th;
        }

        public void PrepareRender(GraphViewNodeGetter getter, double ox, double oy)
        {
            if (getter(Start) is GraphViewNode n1 && getter(End) is GraphViewNode n2)
            {
                CX = double.IsFinite(CX) ? CX - (_x1 + _x2) / 2 : 0;
                CY = double.IsFinite(CY) ? CY - (_y1 + _y2) / 2 : 0;
                _x1 = n1.X;
                _y1 = n1.Y;
                _x2 = n2.X;
                _y2 = n2.Y;
                CX += (_x1 + _x2) / 2;
                CY += (_y1 + _y2) / 2;
                StreamGeometry sg = new();
                using (var ctx = sg.Open())
                {
                    ctx.BeginFigure(new(_x1 + ox, _y1 + oy), false, false);
                    ctx.LineTo(new(CX + ox, CY + oy), true, false);
                    ctx.LineTo(new(_x2 + ox, _y2 + oy), true, false);
                }
                sg.Freeze();
                Geometry = sg;
            }
            else
            {
                CX = double.NaN;
                CY = double.NaN;
                Geometry = null;
            }
        }
    }
}
