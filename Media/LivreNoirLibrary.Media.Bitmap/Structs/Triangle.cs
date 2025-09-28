using System;
using System.Runtime.Intrinsics;
using System.Drawing;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public readonly struct Triangle(int x0, int y0, int x1, int y1, int x2, int y2) : IEquatable<Triangle>
    {
        public readonly int X0 = x0;
        public readonly int Y0 = y0;
        public readonly int X1 = x1;
        public readonly int Y1 = y1;
        public readonly int X2 = x2;
        public readonly int Y2 = y2;

        public Triangle(Point p0, Point p1, Point p2) : this(p0.X, p0.Y, p1.X, p1.Y, p2.X, p2.Y) { }
        public Triangle((int, int) p0, (int, int) p1, (int, int) p2) : this(p0.Item1, p0.Item2, p1.Item1, p1.Item2, p2.Item1, p2.Item2) { }
        public Triangle((double, double) p0, (double, double)p1, (double, double)p2) : this((int)p0.Item1, (int)p0.Item2, (int)p1.Item1, (int)p1.Item2, (int)p2.Item1, (int)p2.Item2) { }

        public override int GetHashCode() => HashCode.Combine(X0, Y0, X1, Y1, X2, Y2);
        public override bool Equals(object? obj) => obj is Triangle r && Equals(r);
        public unsafe bool Equals(Triangle other)
        {
            if (Vector256<int>.IsSupported)
            {
                var leftVector = Vector256.Create(X0, Y0, X1, Y1, X2, Y2, 0, 0);
                var rightVector = Vector256.Create(other.X0, other.Y0, other.X1, other.Y1, other.X2, other.Y2, 0, 0);
                return leftVector == rightVector;
            }
            else if (Vector128<int>.IsSupported)
            {
                var left = this;
                var leftVector = *(Vector128<int>*)&left;
                var rightVector = *(Vector128<int>*)&other;
                return leftVector == rightVector && X2 == other.X2 && Y2 == other.Y2;
            }
            else
            {
                return X0 == other.X0 && Y0 == other.Y0 && X1 == other.X1 && Y1 == other.Y1 && X2 == other.X2 && Y2 == other.Y2;
            }
        }

        public static bool operator ==(Triangle left, Triangle right) => left.Equals(right);
        public static bool operator !=(Triangle left, Triangle right) => !left.Equals(right);

        public override string ToString() => $"{{({X0}, {Y0}), ({X1}, {Y1}), ({X2}, {Y2})}}";

        public static implicit operator Triangle((int, int, int, int, int, int)value) => new(value.Item1, value.Item2, value.Item3, value.Item4, value.Item5, value.Item6);
        public static implicit operator Triangle((Point, Point, Point) value) => new(value.Item1, value.Item2, value.Item3);
        public static implicit operator Triangle(((int, int), (int, int), (int, int)) value) => new(value.Item1, value.Item2, value.Item3);
        public static implicit operator Triangle(((double, double), (double, double), (double, double)) value) => new(value.Item1, value.Item2, value.Item3);

        public void Deconstruct(out int x0, out int y0, out int x1, out int y1, out int x2, out int y2)
        {
            x0 = X0;
            y0 = Y0;
            x1 = X1;
            y1 = Y1;
            x2 = X2;
            y2 = Y2;
        }

        public Enumerator GetEnumerator() => new(this);

        public struct Enumerator
        {
            private readonly int _x0;
            private readonly int _x1;
            private readonly int _x2;
            private readonly int _y1;
            private readonly int _y2;
            private readonly double _x1_x0;
            private readonly double _x2_x0;
            private readonly double _x2_x1;
            private readonly double _x0y1_x1y0;
            private readonly double _x0y2_x2y0;
            private readonly double _x1y2_x2y1;
            private readonly double _i_y1_y0;
            private readonly double _i_y2_y0;
            private readonly double _i_y2_y1;
            private int _mode;
            private int _left;
            private int _right;
            private int _y;

            public readonly (int Left, int Right, int Y) Current => (_left, _right, _y);

            internal Enumerator(Triangle triangle)
            {
                var (x0, y0, x1, y1, x2, y2) = triangle;
                // 3点が一直線上にある場合
                if ((y1 - y0) * (x2 - x0) == (x1 - x0) * (y2 - y0))
                {
                    (x1, x2) = (Math.Min(x0, Math.Min(x1, x2)), Math.Max(x0, Math.Max(x1, x2)));
                    _mode = 3;
                }
                else
                {
                    // 上から順に並び替える
                    if (y1 > y2)
                    {
                        (x1, y1, x2, y2) = (x2, y2, x1, y1);
                    }
                    if (y0 > y2)
                    {
                        (x0, y0, x2, y2) = (x2, y2, x0, y0);
                    }
                    if (y0 > y1)
                    {
                        (x0, y0, x1, y1) = (x1, y1, x0, y0);
                    }
                    var dx0 = (double)x0;
                    var dx1 = (double)x1;
                    var dx2 = (double)x2;
                    var dy0 = (double)y0;
                    var dy1 = (double)y1;
                    var dy2 = (double)y2;
                    _x1_x0 = dx1 - dx0;
                    _x2_x0 = dx2 - dx0;
                    _x2_x1 = dx2 - dx1;
                    _x0y1_x1y0 = dx0 * dy1 - dx1 * dy0;
                    _x0y2_x2y0 = dx0 * dy2 - dx2 * dy0;
                    _x1y2_x2y1 = dx1 * dy2 - dx2 - dy1;
                    if (y0 == y1)
                    {
                        _mode = 1;
                        _i_y1_y0 = 0;
                    }
                    else
                    {
                        _mode = 2;
                        _i_y1_y0 = 1d / (dy1 - dy0);
                    }
                    _i_y2_y0 = 1d / (dy2 - dy0);
                    if (y1 == y2)
                    {
                        _i_y2_y1 = 0;
                    }
                    else
                    {
                        _i_y2_y1 = 1d / (dy2 - dy1);
                    }
                }
                _y = y0 - 1;
                _x0 = x0;
                _x1 = x1;
                _x2 = x2;
                _y1 = y1;
                _y2 = y2;
            }

            public bool MoveNext()
            {
                _y++;
                switch (_mode)
                {
                    case 1: // upper half (horizontal)
                        (_left, _right) = _x0 <= _x1 ? (_x0, _x1) : (_x1, _x0);
                        _mode = (_y1 == _y2) ? 5 : 4;
                        return true;
                    case 2: // upper half
                            // 現在の y から左右端を計算
                        _left = ((_x1_x0 * _y + _x0y1_x1y0) * _i_y1_y0).RoundToInt();
                        _right = ((_x2_x0 * _y + _x0y2_x2y0) * _i_y2_y0).RoundToInt();
                        if (_left > _right)
                        {
                            (_left, _right) = (_right, _left);
                        }
                        // 次の呼び出しでモードが変わる場合
                        if (_y + 1 >= _y1)
                        {
                            _mode = (_y1 == _y2) ? 3 : 4;
                        }
                        return true;
                    case 3: // lower half (horizontal)
                        (_left, _right) = _x1 <= _x2 ? (_x1, _x2) : (_x2, _x1);
                        _mode = 5;
                        return true;
                    case 4: // lower half
                            // 現在の y から左右端を計算
                        _left = ((_x2_x0 * _y + _x0y2_x2y0) * _i_y2_y0).RoundToInt();
                        _right = ((_x2_x1 * _y + _x1y2_x2y1) * _i_y2_y1).RoundToInt();
                        if (_left > _right)
                        {
                            (_left, _right) = (_right, _left);
                        }
                        // 下端に到達
                        if (_y >= _y2)
                        {
                            _mode = 5;
                        }
                        return true;
                    default:
                        return false;
                }
            }
        }
    }
}
