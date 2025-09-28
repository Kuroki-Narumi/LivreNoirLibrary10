using System;
using System.Drawing;

namespace LivreNoirLibrary.Media
{
    public static unsafe partial class BitmapOperation
    {
        public static void AlphaBlend(this LnBitmapData back, LnColor color) => ColorBlend.Blend(back, color, ColorBlend.Alpha);
        public static void AlphaBlend(this LnBitmapData back, Rectangle rect, LnColor color) => ColorBlend.Blend(back, rect, color, ColorBlend.Alpha);
        public static void AlphaBlend(this LnBitmapData back, LnBitmapData front) => ColorBlend.Blend(back, front, ColorBlend.Alpha);
        public static void AlphaBlend(this LnBitmapData back, LnBitmapData front, Point backPoint, Rectangle frontRect) => ColorBlend.Blend(back, front, backPoint, frontRect, ColorBlend.Alpha);

        public static void Add(this LnBitmapData back, LnColor color) => ColorBlend.Blend(back, color, ColorBlend.Add);
        public static void Add(this LnBitmapData back, Rectangle rect, LnColor color) => ColorBlend.Blend(back, rect, color, ColorBlend.Add);
        public static void Add(this LnBitmapData back, LnBitmapData front) => ColorBlend.Blend(back, front, ColorBlend.Add);
        public static void Add(this LnBitmapData back, LnBitmapData front, Point backPoint, Rectangle frontRect) => ColorBlend.Blend(back, front, backPoint, frontRect, ColorBlend.Add);

        public static void Subtract(this LnBitmapData back, LnColor color) => ColorBlend.Blend(back, color, ColorBlend.Subtract);
        public static void Subtract(this LnBitmapData back, Rectangle rect, LnColor color) => ColorBlend.Blend(back, rect, color, ColorBlend.Subtract);
        public static void Subtract(this LnBitmapData back, LnBitmapData front) => ColorBlend.Blend(back, front, ColorBlend.Subtract);
        public static void Subtract(this LnBitmapData back, LnBitmapData front, Point backPoint, Rectangle frontRect) => ColorBlend.Blend(back, front, backPoint, frontRect, ColorBlend.Subtract);

        public static void Multiply(this LnBitmapData back, LnColor color) => ColorBlend.Blend(back, color, ColorBlend.Multiply);
        public static void Multiply(this LnBitmapData back, Rectangle rect, LnColor color) => ColorBlend.Blend(back, rect, color, ColorBlend.Multiply);
        public static void Multiply(this LnBitmapData back, LnBitmapData front) => ColorBlend.Blend(back, front, ColorBlend.Multiply);
        public static void Multiply(this LnBitmapData back, LnBitmapData front, Point backPoint, Rectangle frontRect) => ColorBlend.Blend(back, front, backPoint, frontRect, ColorBlend.Multiply);

        public static void Screen(this LnBitmapData back, LnColor color) => ColorBlend.Blend(back, color, ColorBlend.Screen);
        public static void Screen(this LnBitmapData back, Rectangle rect, LnColor color) => ColorBlend.Blend(back, rect, color, ColorBlend.Screen);
        public static void Screen(this LnBitmapData back, LnBitmapData front) => ColorBlend.Blend(back, front, ColorBlend.Screen);
        public static void Screen(this LnBitmapData back, LnBitmapData front, Point backPoint, Rectangle frontRect) => ColorBlend.Blend(back, front, backPoint, frontRect, ColorBlend.Screen);

        public static void Overlay(this LnBitmapData back, LnColor color) => ColorBlend.Blend(back, color, ColorBlend.Overlay);
        public static void Overlay(this LnBitmapData back, Rectangle rect, LnColor color) => ColorBlend.Blend(back, rect, color, ColorBlend.Overlay);
        public static void Overlay(this LnBitmapData back, LnBitmapData front) => ColorBlend.Blend(back, front, ColorBlend.Overlay);
        public static void Overlay(this LnBitmapData back, LnBitmapData front, Point backPoint, Rectangle frontRect) => ColorBlend.Blend(back, front, backPoint, frontRect, ColorBlend.Overlay);

        public static void Darken(this LnBitmapData back, LnColor color) => ColorBlend.Blend(back, color, ColorBlend.Darken);
        public static void Darken(this LnBitmapData back, Rectangle rect, LnColor color) => ColorBlend.Blend(back, rect, color, ColorBlend.Darken);
        public static void Darken(this LnBitmapData back, LnBitmapData front) => ColorBlend.Blend(back, front, ColorBlend.Darken);
        public static void Darken(this LnBitmapData back, LnBitmapData front, Point backPoint, Rectangle frontRect) => ColorBlend.Blend(back, front, backPoint, frontRect, ColorBlend.Darken);

        public static void Lighten(this LnBitmapData back, LnColor color) => ColorBlend.Blend(back, color, ColorBlend.Lighten);
        public static void Lighten(this LnBitmapData back, Rectangle rect, LnColor color) => ColorBlend.Blend(back, rect, color, ColorBlend.Lighten);
        public static void Lighten(this LnBitmapData back, LnBitmapData front) => ColorBlend.Blend(back, front, ColorBlend.Lighten);
        public static void Lighten(this LnBitmapData back, LnBitmapData front, Point backPoint, Rectangle frontRect) => ColorBlend.Blend(back, front, backPoint, frontRect, ColorBlend.Lighten);

        public static void ColorDodge(this LnBitmapData back, LnColor color) => ColorBlend.Blend(back, color, ColorBlend.ColorDodge);
        public static void ColorDodge(this LnBitmapData back, Rectangle rect, LnColor color) => ColorBlend.Blend(back, rect, color, ColorBlend.ColorDodge);
        public static void ColorDodge(this LnBitmapData back, LnBitmapData front) => ColorBlend.Blend(back, front, ColorBlend.ColorDodge);
        public static void ColorDodge(this LnBitmapData back, LnBitmapData front, Point backPoint, Rectangle frontRect) => ColorBlend.Blend(back, front, backPoint, frontRect, ColorBlend.ColorDodge);

        public static void ColorBurn(this LnBitmapData back, LnColor color) => ColorBlend.Blend(back, color, ColorBlend.ColorBurn);
        public static void ColorBurn(this LnBitmapData back, Rectangle rect, LnColor color) => ColorBlend.Blend(back, rect, color, ColorBlend.ColorBurn);
        public static void ColorBurn(this LnBitmapData back, LnBitmapData front) => ColorBlend.Blend(back, front, ColorBlend.ColorBurn);
        public static void ColorBurn(this LnBitmapData back, LnBitmapData front, Point backPoint, Rectangle frontRect) => ColorBlend.Blend(back, front, backPoint, frontRect, ColorBlend.ColorBurn);

        public static void HardLight(this LnBitmapData back, LnColor color) => ColorBlend.Blend(back, color, ColorBlend.HardLight);
        public static void HardLight(this LnBitmapData back, Rectangle rect, LnColor color) => ColorBlend.Blend(back, rect, color, ColorBlend.HardLight);
        public static void HardLight(this LnBitmapData back, LnBitmapData front) => ColorBlend.Blend(back, front, ColorBlend.HardLight);
        public static void HardLight(this LnBitmapData back, LnBitmapData front, Point backPoint, Rectangle frontRect) => ColorBlend.Blend(back, front, backPoint, frontRect, ColorBlend.HardLight);

        public static void SoftLight(this LnBitmapData back, LnColor color) => ColorBlend.Blend(back, color, ColorBlend.SoftLight);
        public static void SoftLight(this LnBitmapData back, Rectangle rect, LnColor color) => ColorBlend.Blend(back, rect, color, ColorBlend.SoftLight);
        public static void SoftLight(this LnBitmapData back, LnBitmapData front) => ColorBlend.Blend(back, front, ColorBlend.SoftLight);
        public static void SoftLight(this LnBitmapData back, LnBitmapData front, Point backPoint, Rectangle frontRect) => ColorBlend.Blend(back, front, backPoint, frontRect, ColorBlend.SoftLight);

        public static void Difference(this LnBitmapData back, LnColor color) => ColorBlend.Blend(back, color, ColorBlend.Difference);
        public static void Difference(this LnBitmapData back, Rectangle rect, LnColor color) => ColorBlend.Blend(back, rect, color, ColorBlend.Difference);
        public static void Difference(this LnBitmapData back, LnBitmapData front) => ColorBlend.Blend(back, front, ColorBlend.Difference);
        public static void Difference(this LnBitmapData back, LnBitmapData front, Point backPoint, Rectangle frontRect) => ColorBlend.Blend(back, front, backPoint, frontRect, ColorBlend.Difference);

        public static void Exclusion(this LnBitmapData back, LnColor color) => ColorBlend.Blend(back, color, ColorBlend.Exclusion);
        public static void Exclusion(this LnBitmapData back, Rectangle rect, LnColor color) => ColorBlend.Blend(back, rect, color, ColorBlend.Exclusion);
        public static void Exclusion(this LnBitmapData back, LnBitmapData front) => ColorBlend.Blend(back, front, ColorBlend.Exclusion);
        public static void Exclusion(this LnBitmapData back, LnBitmapData front, Point backPoint, Rectangle frontRect) => ColorBlend.Blend(back, front, backPoint, frontRect, ColorBlend.Exclusion);
    }
}
