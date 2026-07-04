using System;

namespace LivreNoirLibrary.YuGiOh.Media
{
    public struct LinkArrowEnumerator
    {
        readonly double width, height, w1, w2, x1, x2, x3, h1, h2, y1, y2, y3;
        int _index;
        LinkDirection _currentDirection;
        string _currentGeometry = "";

        public readonly (LinkDirection Direction, string Geometry) Current => (_currentDirection, _currentGeometry);

        public LinkArrowEnumerator(double width, double height)
        {
            this.width = width;
            this.height = height;
            w1 = width / 4.0;
            w2 = width / 5.0;
            x1 = width / 40.0;
            x2 = width / 2.0;
            x3 = x1 * 39.0;
            h1 = height / 4.0;
            h2 = height / 5.0;
            y1 = height / 40.0;
            y2 = height / 2.0;
            y3 = y1 * 39.0;
        }

        public bool MoveNext()
        {
            if (_index < 9)
            {
                (_currentDirection, _currentGeometry) = _index switch
                {
                    0 => (LinkDirection.LowerLeft, GetGeometry(x1, y3, 0, -h1, w1, 0)),
                    1 => (LinkDirection.Lower, GetGeometry(x2, height, -w2, -h2, w2, -h2)),
                    2 => (LinkDirection.LowerRight, GetGeometry(x3, y3, 0, -h1, -w1, 0)),
                    3 => (LinkDirection.Left, GetGeometry(0, y2, w2, -h2, w2, h2)),
                    4 => (LinkDirection.Right, GetGeometry(width, y2, -w2, -h2, -w2, h2)),
                    5 => (LinkDirection.UpperLeft, GetGeometry(x1, y1, 0, h1, w1, 0)),
                    6 => (LinkDirection.Upper, GetGeometry(x2, 0, -w2, h2, w2, h2)),
                    7 => (LinkDirection.UpperRight, GetGeometry(x3, y1, 0, h1, -w1, 0)),
                    _ => (LinkDirection.None, ""),
                };
                _index++;
                return true;
            }
            return false;
        }

        static string GetGeometry(double x0, double y0, double dx1, double dy1, double dx2, double dy2)
        {
            return $"M{x0:0.###},{y0:0.###} L{x0 + dx1:0.###},{y0 + dy1:0.###} L{x0 + dx2:0.###},{y0 + dy2:0.###} Z ";
        }

        public readonly LinkArrowEnumerator GetEnumerator() => this;
    }
}
