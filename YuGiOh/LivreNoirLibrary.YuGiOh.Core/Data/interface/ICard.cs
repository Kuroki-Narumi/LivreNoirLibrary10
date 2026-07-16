using LivreNoirLibrary.Media.VectorGraphics;
using System;
using System.Text.RegularExpressions;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public interface ICard
    {
        /// <summary>
        /// This property is used when you want to receive change notifications while binding the card itself.
        /// </summary>
        Card ThisCard { get; }
    }

    public static partial class Extensions
    {
        [GeneratedRegex(@"^《(.+)》$")]
        private static partial Regex Regex_Bracket { get; }
        public static string RemoveBracket(this string name) => Regex_Bracket.Replace(name, "$1");

        extension(ICard card)
        {
            public string NameWithBracket() => $"《{card.ThisCard.Name}》";

            public CardType BaseCardType => card.ThisCard.CardType & CardType.Type_Filter;

            public bool IsMonster() => card.ThisCard.CardType.IsMonster();
            public bool IsSpell() => card.ThisCard.CardType.IsSpell();
            public bool IsTrap() => card.ThisCard.CardType.IsTrap();
            public bool IsMainDeck() => card.ThisCard.CardType.IsMainDeck();
            public bool IsMainDeckMonster() => card.ThisCard.CardType.IsMainDeckMonster();
            public bool IsExtraDeck() => card.ThisCard.CardType.IsExtraDeck();

            public bool IsMainMonster() => card.ThisCard.CardType.IsMainMonster();
            public bool IsFusion() => card.ThisCard.CardType.IsFusion();
            public bool IsRitual() => card.ThisCard.CardType.IsRitual();
            public bool IsSynchro() => card.ThisCard.CardType.IsSynchro();
            public bool IsXyz() => card.ThisCard.CardType.IsXyz();
            public bool IsLink() => card.ThisCard.CardType.IsLink();
            public bool IsToken() => card.ThisCard.CardType.IsToken();
            public bool HasLevel() => card.ThisCard.CardType.HasLevel();
            public bool HasDef() => card.ThisCard.CardType.HasDef();

            public bool IsPendulum() => card.ThisCard.Ability.IsPendulum();
            public bool IsTuner() => card.ThisCard.Ability.IsTuner();
            public bool IsSpecualSummon() => card.ThisCard.Ability.IsSpecualSummon();
            public bool IsFlip() => card.ThisCard.Ability.IsFlip();
            public bool IsSpirit() => card.ThisCard.Ability.IsSpirit();
            public bool IsUnion() => card.ThisCard.Ability.IsUnion();
            public bool IsGemini() => card.ThisCard.Ability.IsGemini();
            public bool IsToon() => card.ThisCard.Ability.IsToon();

            public bool IsOcgReleased() => card.ThisCard.PackInfo.ContainsOcg;
            public bool IsTcgReleased() => card.ThisCard.PackInfo.ContainsTcg;

            public LinkDirection GetLinkDirections() => (LinkDirection)card.ThisCard.Def;

            public bool SetLinkDirections(LinkDirection direction)
            {
                var value = Math.Clamp((int)direction, 0, 255);
                var c = card.ThisCard;
                if (c.Def != value)
                {
                    c.Def = value;
                    c.Level = direction.GetCount();
                    return true;
                }
                return false;
            }

            public Media.CardIconType GetFrameType() => Media.Icons.GetIconType(card.ThisCard);
        }
    }
}
