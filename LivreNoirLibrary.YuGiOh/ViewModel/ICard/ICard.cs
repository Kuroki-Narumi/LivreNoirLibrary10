using System;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    public interface ICard
    {
        public int Id { get; }
        public string Name { get; }
        public CardType CardType { get; }
        public string Text { get; }
    }

    public static partial class ICardExtensions
    {
        public static string NameWithBracket(this ICard obj) => $"《{obj.Name}》";

        public static string RemoveBracket(this string name) => Regex_Bracket.Replace(name, "$1");

        [GeneratedRegex(@"^《(.+)》$")]
        private static partial Regex Regex_Bracket { get; }
    }
}
