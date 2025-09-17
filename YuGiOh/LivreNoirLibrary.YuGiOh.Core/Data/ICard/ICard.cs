using System;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public interface ICard
    {
        public int Id { get; }
        public string Name { get; }
        public string Ruby { get; }
        public string EnName { get; }
        public CardType CardType { get; }
        public string Text { get; }
        public bool Unusable { get; }

        public Attribute Attribute { get; }
        public MonsterType MonsterType { get; }
        public bool HasEffect { get; }
        public Ability Ability { get; }
        public int Level { get; }
        public int Atk { get; }
        public int Def { get; }
        public int PendulumScale { get; }
        public string PendulumText { get; }
    }

    public static partial class ICardExtensions
    {
        public static string NameWithBracket(this ICard obj) => $"《{obj.Name}》";

        public static string RemoveBracket(this string name) => Regex_Bracket.Replace(name, "$1");

        [GeneratedRegex(@"^《(.+)》$")]
        private static partial Regex Regex_Bracket { get; }
    }
}
