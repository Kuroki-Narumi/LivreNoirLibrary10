using System;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public interface ICard
    {
        int Id { get; }
        string Name { get; }
        string Ruby { get; }
        string EnName { get; }
        CardType CardType { get; }
        string Text { get; }
        bool Unusable { get; }

        Attribute Attribute { get; }
        MonsterType MonsterType { get; }
        bool HasEffect { get; }
        Ability Ability { get; }
        int Level { get; }
        int Atk { get; }
        int Def { get; }
        int PendulumScale { get; }
        string PendulumText { get; }
    }

    public static partial class ICardExtensions
    {
        public static string NameWithBracket(this ICard obj) => $"《{obj.Name}》";

        public static string RemoveBracket(this string name) => Regex_Bracket.Replace(name, "$1");

        [GeneratedRegex(@"^《(.+)》$")]
        private static partial Regex Regex_Bracket { get; }
    }
}
