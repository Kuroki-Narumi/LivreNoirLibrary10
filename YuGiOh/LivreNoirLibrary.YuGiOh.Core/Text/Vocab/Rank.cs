using System;
using LivreNoirLibrary.YuGiOh.MasterDuel;

namespace LivreNoirLibrary.YuGiOh
{
    public static partial class Vocab
    {
        public static string GetName(Rank value) => value.ToString();
        public static Rank GetRank(ReadOnlySpan<char> name) => Enum.TryParse(name, true, out Rank rank) ? rank : 0;
    }
}
