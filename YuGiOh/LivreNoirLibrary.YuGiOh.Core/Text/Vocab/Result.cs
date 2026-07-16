using System;
using System.Collections.Generic;
using LivreNoirLibrary.YuGiOh.MasterDuel;

namespace LivreNoirLibrary.YuGiOh
{
    public static partial class Vocab
    {
        public const string Result = "結果";

        public const string Lose = "負け";
        public const string Win = "勝ち";
        public const string Draw = "引き分け";
        public const string DiscLose = "切断(負)";
        public const string DiscWin = "切断(勝)";

        private static readonly Dictionary<Result, string> _result2name = new()
        {
            { MasterDuel.Result.Lose, Lose },
            { MasterDuel.Result.Win, Win },
            { MasterDuel.Result.Draw, Draw },
            { MasterDuel.Result.DiscLose, DiscLose },
            { MasterDuel.Result.DiscWin, DiscWin },
        };
        private static readonly Dictionary<string, Result>.AlternateLookup<ReadOnlySpan<char>> _name2result = CreateInvertedDictionary(_result2name);

        public static string GetName(this Result value) => GetEnumName(value, _result2name);
        public static Result GetResult(ReadOnlySpan<char> name) => GetEnumValue(name, _name2result);
    }
}
