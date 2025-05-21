using System;
using System.Windows.Media;
using Drawing = System.Drawing;
using static LivreNoirLibrary.Media.ColorUtils;

namespace LivreNoirLibrary.Media
{
    public readonly partial struct HsvColor
    {
        public readonly float A;
        public readonly float R;
        public readonly float G;
        public readonly float B;
        public readonly float H;
        public readonly float S;
        public readonly float V;

        public byte ByteA => GetByte(A);
        public byte ByteR => GetByte(R);
        public byte ByteG => GetByte(G);
        public byte ByteB => GetByte(B);

        private HsvColor(bool fromRGB, float a, float v1, float v2, float v3)
        {
            A = Math.Clamp(a, 0, 1);
            if (fromRGB)
            {
                R = Math.Clamp(v1, 0, 1);
                G = Math.Clamp(v2, 0, 1);
                B = Math.Clamp(v3, 0, 1);
                (H, S, V) = CalcHSV(v1, v2, v3);
            }
            else
            {
                H = v1;
                S = Math.Clamp(v2, 0, 1);
                V = Math.Clamp(v3, 0, 1);
                (R, G, B) = CalcRGB(v1, v2, v3);
            }
        }

        private HsvColor(byte a, byte r, byte g, byte b)
        {
            A = GetFloat(a);
            R = GetFloat(r);
            G = GetFloat(g);
            B = GetFloat(b);
            H = S = V = 0;
        }

        private HsvColor(Color c) : this(c.A, c.R, c.G, c.B) { }
        private HsvColor(Drawing.Color c) : this(c.A, c.R, c.G, c.B) { }

        public static HsvColor FromRgb(byte r, byte g, byte b) => new(true, 1, GetFloat(r), GetFloat(g), GetFloat(b));
        public static HsvColor FromRgb(float r, float g, float b) => new(true, 1, r, g, b);
        public static HsvColor FromArgb(byte a, byte r, byte g, byte b) => new(true, GetFloat(a), GetFloat(r), GetFloat(g), GetFloat(b));
        public static HsvColor FromArgb(float a, float r, float g, float b) => new(true, a, r, g, b);
        public static HsvColor FromHsv(float h, float s, float v) => new(false, 1, h, s, v);
        public static HsvColor FromAhsv(float a, float h, float s, float v) => new(false, a, h, s, v);
        public static HsvColor FromColor(Color color) => FromArgb(color.A, color.R, color.G, color.B);
        public static HsvColor FromColor(Drawing.Color color) => FromArgb(color.A, color.R, color.G, color.B);

        public static implicit operator HsvColor(in Color color) => FromColor(color);
        public static implicit operator HsvColor(in Drawing.Color color) => FromColor(color);
        public static explicit operator Color(in HsvColor color) => color.ToColor();
        public static explicit operator Drawing.Color(in HsvColor color) => color.ToDrawingColor();

        public void Deconstruct(out float a, out float r, out float g, out float b)
        {
            a = A;
            r = R;
            g = G;
            b = B;
        }

        private (byte, byte, byte, byte) GetBytes() => (ByteA, ByteR, ByteG, ByteB);

        public Color ToColor()
        {
            var (a, r, g, b) = GetBytes();
            return Color.FromArgb(a, r, g, b);
        }

        public Drawing.Color ToDrawingColor()
        {
            var (a, r, g, b) = GetBytes();
            return Drawing.Color.FromArgb(a, r, g, b);
        }

        public string GetColorCode(bool alpha = true)
        {
            var (a, r, g, b) = GetBytes();
            return alpha ? $"#{a:X2}{r:X2}{g:X2}{b:X2}" : $"#{r:X2}{g:X2}{b:X2}";
        }

        public static bool TryParseColorCode(string colorCode, out HsvColor color)
        {
            if (ColorUtils.TryParseColorCode(colorCode, out var a, out var r, out var g, out var b))
            {
                color = new(true, a, r, g, b);
                return true;
            }
            color = default;
            return false;
        }

        public static HsvColor FromColorCode(string colorCode)
        {
            if (ColorUtils.TryParseColorCode(colorCode, out var a, out var r, out var g, out var b))
            {
                return new(true, a, r, g, b);
            }
            throw new FormatException($"wrong color code style: {colorCode}");
        }

        public HsvColor GetHueChanged(float h)
        {
            return FromHsv(H + h, S, V);
        }

        public static Color GetHueChanged(Color color, float h)
        {
            var (r, g, b) = ColorUtils.GetHueChanged(color.R, color.G, color.B, h);
            return Color.FromArgb(color.A, r, g, b);
        }

        public static Color GetBlended(Color color1, Color color2, float ratio = 0.5f)
        {
            return GetBlended(FromColor(color1), FromColor(color2), ratio).ToColor();
        }

        public static HsvColor GetBlended(HsvColor color1, HsvColor color2, float ratio = 0.5f)
        {
            return FromAhsv(Blend(color1.A, color2.A, ratio), 
                            Blend(color1.H, color2.H, ratio),
                            Blend(color1.S, color2.S, ratio),
                            Blend(color1.V, color2.V, ratio));
        }

        public static LinearGradientBrush CreateGradientBrush(Color color1, Color color2)
        {
            LinearGradientBrush brush = new();
            var stops = brush.GradientStops;
            stops.Add(new(color1, 0));
            stops.Add(new(color2, 1));
            CreateGradientBrush_General(stops, FromColor(color1), FromColor(color2));
            stops.Freeze();
            brush.Freeze();
            return brush;
        }

        public static LinearGradientBrush CreateGradientBrush(HsvColor color1, HsvColor color2)
        {
            LinearGradientBrush brush = new();
            var stops = brush.GradientStops;
            stops.Add(new(color1.ToColor(), 0));
            stops.Add(new(color2.ToColor(), 1));
            CreateGradientBrush_General(stops, color1, color2);
            stops.Freeze();
            brush.Freeze();
            return brush;
        }

        private static void CreateGradientBrush_General(GradientStopCollection stops, HsvColor c1, HsvColor c2)
        {
            var y1 = c1.H / 60f;
            var y2 = c2.H / 60f;
            // Hueが60の倍数を越える点でグラデーションを追加
            var (min, max) = y1 > y2 ? (y2, y1) : (y1, y2);
            for (int y = (int)min; y < max; y++)
            {
                var x = (y - y1) / (y2 - y1);
                if (x > 0 && x < 1)
                {
                    GradientStop stop = new(GetBlended(c1, c2, x).ToColor(), x);
                    stop.Freeze();
                    stops.Add(stop);
                }
            }
        }
    }
}
