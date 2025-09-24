using System;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IDifference
    {
        public const string AddSymbol = "+";
        public const string RemoveSymbol = "-";
        public const string ChangeSymbol = "@";

        public static string GetSymbol(DiffType type) => type switch
        {
            DiffType.Added => AddSymbol,
            DiffType.Removed => RemoveSymbol,
            _ => ChangeSymbol
        };

        public DiffType DiffType { get; }
    }
}
