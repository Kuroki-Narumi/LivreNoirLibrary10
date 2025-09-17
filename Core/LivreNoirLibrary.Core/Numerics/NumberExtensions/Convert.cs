using System;
using System.Numerics;

namespace LivreNoirLibrary.Numerics
{
    public static partial class NumberExtensions
    {
        public static float Validate(this float value, float @default = 0) => float.IsFinite(value) ? value : @default;
        public static double Validate(this double value, double @default = 0) => double.IsFinite(value) ? value : @default;

        public static bool TryGetInt(object? obj, out int value)
        {
            switch (obj)
            {
                case int v:
                    value = v;
                    return true;
                case string v:
                    return int.TryParse(v, out value);
                case byte v:
                    value = v;
                    return true;
                case sbyte v:
                    value = v;
                    return true;
                case short v:
                    value = v;
                    return true;
                case ushort v:
                    value = v;
                    return true;
                case uint v:
                    value = (int)v;
                    return true;
                case long v:
                    value = (int)v;
                    return true;
                case ulong v:
                    value = (int)v;
                    return true;
                case float v:
                    value = (int)v;
                    return true;
                case double v:
                    value = (int)v;
                    return true;
                case decimal v:
                    value = (int)v;
                    return true;
                default:
                    value = default;
                    return false;
            }
        }

        public static bool TryGetLong(object? obj, out long value)
        {
            switch (obj)
            {
                case long v:
                    value = v;
                    return true;
                case string v:
                    return long.TryParse(v, out value);
                case byte v:
                    value = v;
                    return true;
                case sbyte v:
                    value = v;
                    return true;
                case short v:
                    value = v;
                    return true;
                case ushort v:
                    value = v;
                    return true;
                case int v:
                    value = v;
                    return true;
                case uint v:
                    value = v;
                    return true;
                case ulong v:
                    value = (long)v;
                    return true;
                case float v:
                    value = (long)v;
                    return true;
                case double v:
                    value = (long)v;
                    return true;
                case decimal v:
                    value = (long)v;
                    return true;
                default:
                    value = default;
                    return false;
            }
        }

        public static bool TryGetDouble(object? obj, out double value)
        {
            switch (obj)
            {
                case double v:
                    value = v;
                    return true;
                case string v:
                    return double.TryParse(v, out value);
                case byte v:
                    value = v;
                    return true;
                case sbyte v:
                    value = v;
                    return true;
                case short v:
                    value = v;
                    return true;
                case ushort v:
                    value = v;
                    return true;
                case int v:
                    value = v;
                    return true;
                case uint v:
                    value = v;
                    return true;
                case long v:
                    value = v;
                    return true;
                case ulong v:
                    value = v;
                    return true;
                case float v:
                    value = v;
                    return true;
                case decimal v:
                    value = (double)v;
                    return true;
                default:
                    value = default;
                    return false;
            }
        }
    }
}
