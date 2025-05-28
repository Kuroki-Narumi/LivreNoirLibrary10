using System;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.YuGiOh.DuelLog;

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

        public static string GetName(Result value) => GetEnumName(value, _result2name);
        public static Result GetResult(string? name) => GetEnumValue(name, _name2result);

        private static readonly Dictionary<Result, string> _result2name = new()
        {
            { DuelLog.Result.Lose, Lose },
            { DuelLog.Result.Win, Win },
            { DuelLog.Result.Draw, Draw },
            { DuelLog.Result.DiscLose, DiscLose },
            { DuelLog.Result.DiscWin, DiscWin },
        };

        private static readonly Dictionary<string, Result> _name2result = CreateName2Result();

        private static Dictionary<string, Result> CreateName2Result()
        {
            var dic = _result2name.Invert();
            AppendEnglishNames(dic);
            return dic;
        }
    }
}
