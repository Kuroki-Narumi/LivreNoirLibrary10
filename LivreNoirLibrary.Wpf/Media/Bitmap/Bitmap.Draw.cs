using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public static partial class Bitmap
    {
        public const int StackLimit = 4096;

        public static unsafe void FillRect(this WriteableBitmap bitmap, int x, int y, int width, int height, Color color)
        {
            AdjustRect(bitmap, ref x, ref y, ref width, ref height);
            var stride = bitmap.PixelWidth;
            var c = ColorOperation.ToInt(color);
            bitmap.Lock();
            try
            {
                var offset = GetIntPtr(bitmap, x, y);
                for (var yy = 0; yy < height; yy++)
                {
                    SimdOperations.CopyFrom(offset, c, width);
                    offset += stride;
                }
            }
            finally
            {
                bitmap.AddDirtyRect(new(x, y, width, height));
                bitmap.Unlock();
            }
        }

        public static void FillRect(this WriteableBitmap bitmap, Int32Rect rect, Color color) => FillRect(bitmap, rect.X, rect.Y, rect.Width, rect.Height, color);

        public static unsafe void FillTriangle(this WriteableBitmap bitmap, int x0, int y0, int x1, int y1, int x2, int y2, Color color)
        {
            // 3点が直線上にある場合は何もしない
            if ((y1 - y0) * (x2 - x0) == (x1 - x0) * (y2 - y0))
            {
                return;
            }
            // 描画矩形
            var (left, right) = MinMax(x0, x1, x2);
            var (top, bottom) = MinMax(y0, y1, y2);
            Int32Rect rect = new(left, top, right - left, bottom - top);

            var srcWidth = bitmap.PixelWidth;
            var srcHeight = bitmap.PixelHeight;
            var c = ColorOperation.ToInt(color);
            bitmap.Lock();
            try
            {
                foreach (var (left1, right1, y) in Enum_FillTriangle(x0, y0, x1, y1, x2, y2))
                {
                    if ((uint)y < (uint)srcHeight)
                    {
                        var left2 = Math.Clamp(left1, 0, srcWidth - 1);
                        var width = Math.Clamp(right1, 0, srcWidth - 1) - left2 + 1;
                        var ptr = GetIntPtr(bitmap, left2, y);
                        new Span<int>(ptr + left2, width).Fill(c);
                    }
                }
            }
            finally
            {
                bitmap.AddDirtyRect(rect);
                bitmap.Unlock();
            }
        }

        public static unsafe void FillTriangle(this WriteableBitmap bitmap, int x0, int y0, int x1, int y1, int x2, int y2, HsvColor color1, HsvColor color2, bool radial = false)
        {
            // 同じ色が指定されている場合は単色用のメソッドを呼ぶ
            if (color1 == color2)
            {
                FillTriangle(bitmap, x0, y0, x1, y1, x2, y2, color1.ToColor());
                return;
            }
            // 3点が直線上にある場合は何もしない
            if ((y1 - y0) * (x2 - x0) == (x1 - x0) * (y2 - y0))
            {
                return;
            }
            // 描画矩形
            var (left, right) = MinMax(x0, x1, x2);
            var (top, bottom) = MinMax(y0, y1, y2);
            Int32Rect rect = new(left, top, right - left, bottom - top);

            var srcWidth = (uint)bitmap.PixelWidth;
            var srcHeight = (uint)bitmap.PixelHeight;
            bitmap.Lock();
            try
            {
                var e = radial ? Enum_FillTriangleG_Radial(x0, y0, x1, y1, x2, y2, color1, color2)
                               : Enum_FillTriangleG_Circular(x0, y0, x1, y1, x2, y2, color1, color2);
                foreach (var (x, y, color) in e)
                {
                    var c = ColorOperation.ToInt(color);
                    if ((uint)x < srcWidth && (uint)y < srcHeight)
                    {
                        var ptr = GetIntPtr(bitmap, x, y);
                        *ptr = c;
                    }
                }
            }
            finally
            {
                bitmap.AddDirtyRect(rect);
                bitmap.Unlock();
            }
        }

        public static void FillTriangle(this WriteableBitmap bitmap, int x0, int y0, int x1, int y1, int x2, int y2, Color color1, Color color2, bool radial = false)
        {
            FillTriangle(bitmap, x0, y0, x1, y1, x2, y2, HsvColor.FromColor(color1), HsvColor.FromColor(color2), radial);
        }

        private static (int Min, int Max) MinMax(int v1, int v2, int v3)
        {
            var min = v1;
            var max = v2;
            if (v2 < min)
            {
                min = v2;
            }
            if (v3 < min)
            {
                min = v3;
            }
            if (v2 > max)
            {
                max = v2;
            }
            if (v3 > max)
            {
                max = v3;
            }
            return (min, max);
        }

        private static IEnumerable<(int Left, int Right, int Y)> Enum_FillTriangle(int x0, int y0, int x1, int y1, int x2, int y2)
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
            // 最も上の点から中間の点まで(上半分)と中間の点から最も下の点(下半分)までで分ける
            int left, right;
            double dx0 = x0, dx1 = x1, dx2 = x2;
            // 上半分
            // (x₂ - x₁)y = (y₂ - y₁)x + x₂y₁ - x₁y₂
            // x = ((x₂ - x₁)y + x₁y₂ - x₂y₁) / (y₂ - y₁)
            if (y0 == y1)
            {
                (left, right) = x0 > x1 ? (x1, x0) : (x0, x1);
                yield return (left, right, y0);
            }
            else
            {
                for (int y = y0; y < y1; y++)
                {
                    left = (((dx1 - dx0) * y + dx0 * y1 - dx1 * y0) / (y1 - y0)).RoundToInt();
                    right = (((dx2 - dx0) * y + dx0 * y2 - dx2 * y0) / (y2 - y0)).RoundToInt();
                    if (left > right)
                    {
                        (left, right) = (right, left);
                    }
                    yield return (left, right, y);
                }
            }
            // 下半分
            if (y1 == y2)
            {
                (left, right) = x1 > x2 ? (x2, x1) : (x1, x2);
                yield return (left, right, y1);
            }
            else
            {
                for (int y = y1; y <= y2; y++)
                {
                    left = (((dx2 - dx0) * y + dx0 * y2 - dx2 * y0) / (y2 - y0)).RoundToInt();
                    right = (((dx2 - dx1) * y + dx1 * y2 - dx2 * y1) / (y2 - y1)).RoundToInt();
                    if (left > right)
                    {
                        (left, right) = (right, left);
                    }
                    yield return (left, right, y);
                }
            }
        }

        private static IEnumerable<(int X, int Y, Color Color)> Enum_FillTriangleG_Circular(int x0, int y0, int x1, int y1, int x2, int y2, HsvColor c1, HsvColor c2)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;
            var cross = x2 * y1 - x1 * y2;
            var (vertical, den) = Math.Abs(dx) >= Math.Abs(dy) ? (false, dx) : (true, dy);
            ColorGetter g = new(c1, c2, den);

            foreach (var (left, right, y) in Enum_FillTriangle(x0, y0, x1, y1, x2, y2))
            {
                var d = y - y0;
                for (int x = left; x <= right; x++)
                {
                    var e = (double)x - x0;
                    var f = (double)x * y0 - x0 * y;
                    var xx = (f * dx - cross * e) / (e * dy - d * dx);
                    int dif;
                    if (vertical)
                    {
                        double yy;
                        if (dx == 0)
                        {
                            yy = (d * xx + f) / e;
                        }
                        else
                        {
                            yy = (dy * xx + cross) / dx;
                        }
                        dif = (yy - y1).RoundToInt();
                    }
                    else
                    {
                        dif = (xx - x1).RoundToInt();
                    }
                    var color = g.Get(dif);
                    yield return (x, y, color);
                }
            }
        }

        private static IEnumerable<(int X, int Y, Color color)> Enum_FillTriangleG_Radial(int x0, int y0, int x1, int y1, int x2, int y2, HsvColor c1, HsvColor c2)
        {
            // 基準直線
            // (y2 - y1) * x + (x1 - x2) * y + x2 * y1 - x1 * y2
            var dx = x1 - x2;
            var dy = y2 - y1;
            var cross = x2 * y1 - x1 * y2;
            var den2 = Math.Sqrt(dx * dx + dy + dy) * 0.5;
            var den = Math.Abs(dy * x0 + dx * y0 + cross) / den2;
            ColorGetter g = new(c1, c2, (float)den);

            foreach (var (left, right, y) in Enum_FillTriangle(x0, y0, x1, y1, x2, y2))
            {
                for (int x = left; x <= right; x++)
                {
                    var d = den - Math.Abs(dy * x + dx * y + cross) / den2;
                    var color = g.Get(d.RoundToInt());
                    yield return (x, y, color);
                }
            }
        }

        internal readonly struct ColorGetter
        {
            private readonly Dictionary<int, Color> Colors = [];
            private readonly float Den;
            private readonly HsvColor Color1;
            private readonly HsvColor Color2;

            public ColorGetter(HsvColor color1, HsvColor color2, float den)
            {
                Color1 = color1;
                Color2 = color2;
                Den = den;
                Colors.Add(0, color1.ToColor());
            }

            public Color Get(int num)
            {
                if (!Colors.TryGetValue(num, out var color))
                {
                    color = HsvColor.GetBlended(Color1, Color2, num / Den).ToColor();
                    Colors.Add(num, color);
                }
                return color;
            }
        }
    }
}
