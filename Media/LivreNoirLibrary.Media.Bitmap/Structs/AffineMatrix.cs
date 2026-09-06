using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Text;

namespace LivreNoirLibrary.Media
{
    public readonly struct AffineMatrix : IEquatable<AffineMatrix>
    {
        public readonly double M11;
        public readonly double M12;
        public readonly double M21;
        public readonly double M22;
        public readonly double OffsetX;
        public readonly double OffsetY;
        private readonly AffineMatrixType _type;

        public double Determinant => M11 * M22 - M12 * M21;
        public bool HasInverse => Determinant != 0;

        internal AffineMatrix(double m11, double m12, double m21, double m22, double ox, double oy, AffineMatrixType type)
        {
            M11 = m11;
            M12 = m12;
            M21 = m21;
            M22 = m22;
            OffsetX = ox;
            OffsetY = oy;
            _type = type;
        }

        public AffineMatrix(double m11, double m12, double m21, double m22, double offsetX, double offsetY) : this(m11, m12, m21, m22, offsetX, offsetY, AffineMatrixType.Unknown) { }

        public static readonly AffineMatrix Identity = new(1, 0, 0, 1, 0, 0, AffineMatrixType.Identity);

        public static AffineMatrix Offset(double x, double y) => new(1, 0, 0, 1, x, y, AffineMatrixType.Offset);

        public static AffineMatrix Scale(double x, double y) => new(x, 0, 0, y, 0, 0, AffineMatrixType.Scale);

        public static AffineMatrix ScaleAt(double scaleX, double scaleY, double offsetX, double offsetY)
        {
            return new(scaleX, 0, 0, scaleY, offsetX - offsetX * scaleX, offsetY - offsetY * scaleY, AffineMatrixType.OffsetAndScale);
        }

        public static AffineMatrix Rotate(double radian)
        {
            var (sin, cos) = Math.SinCos(radian);
            return new(cos, -sin, sin, cos, 0, 0);
        }

        public static AffineMatrix RotateAt(double radian, double centerX, double centerY)
        {

            var (sin, cos) = Math.SinCos(radian);
            return new(cos, -sin, sin, cos, centerX - centerX * cos + centerY * sin, centerY - centerX * sin - centerY * cos);
        }

        public static AffineMatrix SkewX(double radian) => new(1, Math.Tan(radian), 0, 1, 0, 0);
        public static AffineMatrix SkewY(double radian) => new(1, 0, Math.Tan(radian), 1, 0, 0);
        public static AffineMatrix Skew(double xRadian, double yRadian) => new(1, Math.Tan(xRadian), Math.Tan(yRadian), 1, 0, 0);

        public AffineMatrix Prepend(AffineMatrix previous) => this * previous;
        public AffineMatrix Append(AffineMatrix next) => next * this;

        public bool TryGetInverse(out AffineMatrix matrix)
        {
            var det = Determinant;
            if (det == 0)
            {
                matrix = default;
                return false;
            }
            else
            {
                switch (_type)
                {
                    case AffineMatrixType.Identity:
                        matrix = this;
                        break;
                    case AffineMatrixType.Scale:
                        matrix = new(1.0 / M11, 0, 0, 1.0 / M22, 0, 0, _type);
                        break;
                    case AffineMatrixType.Offset:
                        matrix = new(1, 0, 0, 1, -OffsetX, -OffsetY, _type);
                        break;
                    default:
                        var invDet = 1 / det;
                        var m11 = M22 * invDet;
                        var m12 = -M12 * invDet;
                        var m21 = -M21 * invDet;
                        var m22 = M11 * invDet;
                        var ox = (M12 * OffsetY - M22 * OffsetX) * invDet;
                        var oy = (M21 * OffsetX - M11 * OffsetY) * invDet;
                        matrix = new(m11, m12, m21, m22, ox, oy);
                        break;
                }
                return true;
            }
        }

        public (double X, double Y) Apply(double x, double y)
        {
            return _type switch
            {
                AffineMatrixType.Identity => (x, y),
                AffineMatrixType.Offset => (x + OffsetX, y + OffsetY),
                AffineMatrixType.Scale => (x * M11, y * M22),
                _ => (M11 * x + M12 * y + OffsetX, M21 * x + M22 * y + OffsetY),
            };
        }

        public static AffineMatrix operator *(AffineMatrix left, AffineMatrix right)
        {
            // {(A, B, X), (C, D, Y), (0, 0, 1)} * {(a, b, x), (c, d, y), (0, 0, 1)}
            // m11 = Aa + Bc
            // m12 = Ab + Bd
            // m21 = Ca + Dc
            // m22 = Cb + Dd
            // ox = Ax + By + X
            // oy = Cx + Dy + Y
            var m11 = left.M11 * right.M11 + left.M12 * right.M21;
            var m12 = left.M11 * right.M12 + left.M12 * right.M22;
            var m21 = left.M21 * right.M11 + left.M22 * right.M21;
            var m22 = left.M21 * right.M12 + left.M22 * right.M22;
            var ox = left.M11 * right.OffsetX + left.M12 * right.OffsetY + left.OffsetX;
            var oy = left.M21 * right.OffsetX + left.M22 * right.OffsetY + left.OffsetY;
            return new(m11, m12, m21, m22, ox, oy);
        }

        public bool Equals(AffineMatrix other) => this == other;
        public override bool Equals([NotNullWhen(true)] object? obj) => obj is AffineMatrix other && this == other;
        public override int GetHashCode() => HashCode.Combine(M11, M12, M21, M22, OffsetX, OffsetY);

        public static bool operator ==(AffineMatrix left, AffineMatrix right) => 
            left.M11 == right.M11 && left.M12 == right.M12 && 
            left.M21 == right.M21 && left.M22 == right.M22 && 
            left.OffsetX == right.OffsetX && left.OffsetY == right.OffsetY;

        public static bool operator !=(AffineMatrix left, AffineMatrix right) => !(left == right);
    }
}
