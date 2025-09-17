using System;
using System.Text.RegularExpressions;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public partial class NumberedCard(Card card, string number) : CardWrapperBase(card)
    {
        public string Number { get; set => SetValue(ref field, value); } = number;

        public int Index { get; } = GetIndex(card, number);

        public static int GetIndex(Card card, string number)
        {
            var match = Regex_Number.Match(number);
            if (match.Success)
            {
                return match.Value.ParseToInt(36);
            }
            else
            {
                return card.Id;
            }

        }

        [GeneratedRegex(@"([0-9a-zA-Z]{1,3})$")]
        private static partial Regex Regex_Number { get; }
    }
}