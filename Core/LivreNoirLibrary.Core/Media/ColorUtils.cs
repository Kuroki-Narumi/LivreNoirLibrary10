using System;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Media
{
    public static class ColorUtils
    {
        public const float InvertFactor = 1f / 255f;
        public const float InvertFactor4bits = 1f / 15f;

        public static float NormalizeHue(float h)
        {
            h %= 360;
            if (h is < 0)
            {
                h += 360;
            }
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (float H, float S, float V) CalcHSV(float r, float g, float b, float fallbackH = 0, float fallbackS = 0)
        {
            var min = r;
            var max = r;
            if (g < min) { min = g; }
            if (b < min) { min = b; }
            if (g > max) { max = g; }
            if (b > max) { max = b; }

            float h;
            var mm = max - min;
            if (mm is 0)
            {
                h = fallbackH;
            }
            else
            {
                h = (max == r) ? 60 * (g - b) / mm :
                    (max == g) ? 60 * (b - r) / mm + 120 :
                                 60 * (r - g) / mm + 240;
                if (h is < 0) { h += 360; }
            }
            return (h, max is 0 ? fallbackS : mm / max, max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (float R, float G, float B) CalcRGB(float h, float s, float v)
        {
            var max = v;
            var min = v - v * s;
            var ratio = (max - min) / 60;
            return h switch
            {
                < 60  => (                    max,         h * ratio + min,                     min),
                < 120 => ((120 - h) * ratio + min,                     max,                     min),
                < 180 => (                    min,                     max, (h - 120) * ratio + min),
                < 240 => (                    min, (240 - h) * ratio + min,                     max),
                < 300 => ((h - 240) * ratio + min,                     min,                     max),
                _     => (                    max,                     min, (360 - h) * ratio + min),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (float R, float G, float B) GetHueChanged(float r, float g, float b, float h)
        {
            var (h1, s, v) = CalcHSV(r, g, b);
            return CalcRGB(h1 + h, s, v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (byte R, byte G, byte B) GetHueChanged(byte r, byte g, byte b, float h)
        {
            var (r2, g2, b2) = GetHueChanged(GetFloat(r), GetFloat(g), GetFloat(b), h);
            return (GetByte(r2), GetByte(g2), GetByte(b2));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (float R, float G, float B) GetHsvChanged(float r, float g, float b, float h, float s, float v)
        {
            var (h1, s1, v1) = CalcHSV(r, g, b);
            return CalcRGB(h1 + h, s1 + s, v1 + v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (byte R, byte G, byte B) GetHsvChanged(byte r, byte g, byte b, float h, float s, float v)
        {
            var (r2, g2, b2) = GetHsvChanged(GetFloat(r), GetFloat(g), GetFloat(b), h, s, v);
            return (GetByte(r2), GetByte(g2), GetByte(b2));
        }

        public static float Blend(float value1, float value2, float ratio = 0.5f) => value1 * (1 - ratio) + value2 * ratio;
        public static byte Blend(byte value1, byte value2, float ratio = 0.5f) => (byte)Math.Clamp(MathF.Round(value1 * (1 - ratio) + value2 * ratio), 0, 255);

        public static string GetColroCode(byte r, byte g, byte b) => $"#{r:X2}{g:X2}{b:X2}";
        public static string GetColroCode(byte a, byte r, byte g, byte b) => $"#{a:X2}{r:X2}{g:X2}{b:X2}";
        public static string GetColorCode(float r, float g, float b) => $"#{GetByte(r):X2}{GetByte(g):X2}{GetByte(b):X2}";
        public static string GetColorCode(float a, float r, float g, float b) => $"#{GetByte(a):X2}{GetByte(r):X2}{GetByte(g):X2}{GetByte(b):X2}";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValidColorCode(string? text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            var span = text.AsSpan();
            if (span[0] is '#')
            {
                span = span[1..];
            }
            return span.Length is 3 or 4 or 6 or 8 && uint.TryParse(span, System.Globalization.NumberStyles.HexNumber, null, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseColorCodeToByte(string colorCode, out byte a, out byte r, out byte g, out byte b)
        {
            var span = colorCode.AsSpan();
            if (span[0] is '#')
            {
                span = span[1..];
            }
            if (uint.TryParse(span, System.Globalization.NumberStyles.HexNumber, null, out var value))
            {
                unchecked
                {
                    switch (span.Length)
                    {
                        case 3: // rgb
                            a = 255;
                            r = (byte)(((value >> 8) & 0xF) * 0x11);
                            g = (byte)(((value >> 4) & 0xF) * 0x11);
                            b = (byte)((value & 0xF) * 0x11);
                            return true;
                        case 4: // argb
                            a = (byte)(((value >> 12) & 0xF) * 0x11);
                            r = (byte)(((value >> 8) & 0xF) * 0x11);
                            g = (byte)(((value >> 4) & 0xF) * 0x11);
                            b = (byte)((value & 0xF) * 0x11);
                            return true;
                        case 6: // rrggbb
                            a = 255;
                            r = (byte)(value >> 16);
                            g = (byte)(value >> 8);
                            b = (byte)value;
                            return true;
                        case 8: // aarrggbb
                            a = (byte)(value >> 24);
                            r = (byte)(value >> 16);
                            g = (byte)(value >> 8);
                            b = (byte)value;
                            return true;
                    }
                }
            }
            a = r = g = b = 0;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseColorCode(string colorCode, out float a, out float r, out float g, out float b)
        {
            var span = colorCode.AsSpan();
            if (span[0] is '#')
            {
                span = span[1..];
            }
            if (uint.TryParse(span, System.Globalization.NumberStyles.HexNumber, null, out var value))
            {
                switch (span.Length)
                {
                    case 3: // rgb
                        a = 1;
                        r = ((value >> 8) & 0xF) * InvertFactor4bits;
                        g = ((value >> 4) & 0xF) * InvertFactor4bits;
                        b = (value & 0xF) * InvertFactor4bits;
                        return true;
                    case 4: // argb
                        a = ((value >> 12) & 0xF) * InvertFactor4bits;
                        r = ((value >> 8) & 0xF) * InvertFactor4bits;
                        g = ((value >> 4) & 0xF) * InvertFactor4bits;
                        b = (value & 0xF) * InvertFactor4bits;
                        return true;
                    case 6: // rrggbb
                        a = 1;
                        r = ((value >> 16) & 0xFF) * InvertFactor;
                        g = ((value >> 8) & 0xFF) * InvertFactor;
                        b = (value & 0xFF) * InvertFactor;
                        return true;
                    case 8: // aarrggbb
                        a = ((value >> 24) & 0xFF) * InvertFactor;
                        r = ((value >> 16) & 0xFF) * InvertFactor;
                        g = ((value >> 8) & 0xFF) * InvertFactor;
                        b = (value & 0xFF) * InvertFactor;
                        return true;
                }
            }
            a = r = g = b = 0;
            return false;
        }

        public static byte GetByte(float value) => (byte)MathF.Round(255 * value);
        public static int GetInt(float value) => (int)MathF.Round(255 * value);
        public static float GetFloat(byte value) => value * InvertFactor;
        public static float GetFloat(int value) => value * InvertFactor;
    }
}
