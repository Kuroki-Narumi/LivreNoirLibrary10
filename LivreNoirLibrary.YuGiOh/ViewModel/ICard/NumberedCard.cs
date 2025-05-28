using System;
using System.Text.RegularExpressions;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    public partial class NumberedCard(Card card, string number) : CardWrapperBase(card)
    {
        [ObservableProperty]
        private string _number = number;

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
