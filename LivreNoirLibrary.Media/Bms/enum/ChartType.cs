using System;

namespace LivreNoirLibrary.Media.Bms
{
    public enum ChartType
    {
        Unknown = 0,
        Beat,
        Popn,
        Generic,
    }

    public static class ChartTypeExtensions
    {
        public const string Fallback_Popn = "Pop";

        public static ChartType Parse(string typeName)
        {
            if (Enum.TryParse<ChartType>(typeName, true, out var type))
            {
                return type;
            }
            // fallback
            if (typeName.Equals(Fallback_Popn, StringComparison.OrdinalIgnoreCase))
            {
                return ChartType.Popn;
            }
            return ChartType.Unknown;
        }
    }
}
