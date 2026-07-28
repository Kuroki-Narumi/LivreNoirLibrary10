using LivreNoirLibrary.Text;
using System;
using System.Text.RegularExpressions;

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
        public const string AtkDef = "攻+守";
        public const string Scale = "スケール";
        public const string Scale_Short = $"P{Scale}";
        public const string Scale_Full = $"{Pendulum}{Scale}";
        public const string PText = $"P{Effect}";

        public const string Separators = "/／|｜,、";
        [GeneratedRegex($"[{Separators}]")]
        public static partial Regex Regex_Separators { get; }

        public static string GetLevelName(CardType type) => type switch { CardType.Link_Monster => Link, CardType.Xyz_Monster => Rank, _ => Level };

        public static string GetStatusText(int value) => value is < 0 ? Unknown : value.ToString();

        private static readonly SelectCharsStringConverter _textCounter = new(static c => !char.IsWhiteSpace(c));

        public static int GetTextLength(ReadOnlySpan<char> text) => _textCounter.GetCharCount(text);
    }
}
