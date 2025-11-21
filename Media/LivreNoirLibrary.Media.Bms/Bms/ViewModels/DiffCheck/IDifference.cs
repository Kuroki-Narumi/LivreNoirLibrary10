using System;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IDifference
    {
        const string AddSymbol = "+";
        const string RemoveSymbol = "-";
        const string ChangeSymbol = "@";

        static string GetSymbol(DiffType type) => type switch
        {
            DiffType.Added => AddSymbol,
            DiffType.Removed => RemoveSymbol,
            _ => ChangeSymbol
        };

        DiffType DiffType { get; }
    }
}
