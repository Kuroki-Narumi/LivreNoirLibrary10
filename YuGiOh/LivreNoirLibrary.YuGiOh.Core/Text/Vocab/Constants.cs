using System;

namespace LivreNoirLibrary.YuGiOh
{
    public static partial class Vocab
    {
        public const string Unknown = "?";
        public const string None = "-";

        public const string Level = "レベル";
        public const string Rank = "ランク";
        public const string Atk = "攻撃力";
        public const string Def = "守備力";
        public const string AD = "攻+守=";
        public const string Scale = "スケール";
        public const string Scale_Short = $"P{Scale}";
        public const string Scale_Full = $"{Pendulum}{Scale}";

        public const string Unusable = "使用不可";
        public const string Forbidden = "禁止";
        public const string Limit1 = "制限";
        public const string Limit2 = "準制限";
        public const string Unlimited = "無制限";
        public const string Specified = "特別指定";
    }
}
