using System;
using LivreNoirLibrary.YuGiOh.DuelLog;

namespace LivreNoirLibrary.YuGiOh
{
    public static partial class Vocab
    {
        public static string GetName(Rank value) => value.ToString();
        public static Rank GetRank(string name) => Enum.TryParse(name, out Rank rank) ? rank : 0;
    }
}
